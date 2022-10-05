using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Gateway.Api.Platform;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Comindware.Gateway.Api;

public class PlatformClient : HttpClient
{
    private const int FileSizeLimit = 314572800;

    private const string StoreStreamsPath = "api/AttachmentApi/StoreStreams";
    private const string MarkStreamsAsRejectedPath = "api/AttachmentApi/MarkStreamsAsRejected";
    private const string ListStreamInfoPath = "api/AttachmentApi/ListStreamInfo";
    private const string AuthenticationPath = "Home/ExternalLogin";
    private const string JsonContent = "application/json";
    private const string RequestSignature = "request-signature";
    private const string SignatureBody = "Request_{0}_{1}_{2}";
    private const string RequestTokenName = "request-token";
    private const string Basic = "Basic";
    private const string AuthError = "Not authenticated. Bad credentials";
    private static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromMinutes(5);

    private static readonly HttpClientHandler Handler = new() { CookieContainer = new CookieContainer(), UseCookies = true };
    private readonly Uri _authenticationUri;
    private readonly SHA1 _hasher = SHA1.Create();
    private readonly Uri _listStreamInfoUri;
    private readonly Uri _markStreamsAsRejectedUri;


    private readonly Uri _storeStreamsToUri;

    private bool _disposed;

    private IEnumerable<string> _requestToken = null!;
    private string _sessionToken = null!;

    public PlatformClient() : base(Handler)
    {
        Timeout = DefaultRequestTimeout;
        var builder = new UriBuilder { Host = "localhost", Scheme = "http", Port = 8081 };

        builder.Path = StoreStreamsPath;
        _storeStreamsToUri = builder.Uri;
        builder.Path = MarkStreamsAsRejectedPath;
        _markStreamsAsRejectedUri = builder.Uri;
        builder.Path = AuthenticationPath;
        _authenticationUri = builder.Uri;
        builder.Path = ListStreamInfoPath;
        _listStreamInfoUri = builder.Uri;
    }

    public async Task<ProcessedFilesResult> ProcessAcceptedFilesAsync(Directory acceptedFilesDirectory, CancellationToken token)
    {
        try
        {
            return await StoreAcceptedFilesAsync(acceptedFilesDirectory, token);
        }
        catch (Exception e)
        {
            return new ProcessedFilesResult(ResultType.Exception, e);
        }
    }

    public async Task<ProcessedFilesResult> ProcessRejectedFilesAsync(Directory rejectedFilesDirectory, CancellationToken token)
    {
        try
        {
            return await MarkFilesAsRejectedAsync(rejectedFilesDirectory, token);
        }
        catch (Exception e)
        {
            return new ProcessedFilesResult(ResultType.Exception, e);
        }
    }

    //TODO Add authentication middleware
    public async Task AuthenticateAsync(CancellationToken token)
    {
        var credentialsBytes = Encoding.UTF8.GetBytes("admin:C0m1ndw4r3Pl@tf0rm");
        var credentials = Convert.ToBase64String(credentialsBytes);
        DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Basic, credentials);
        var responseMessage = await GetAsync(_authenticationUri, token);
        DefaultRequestHeaders.Remove(RequestTokenName);
        if (responseMessage.Headers.TryGetValues(RequestTokenName, out _requestToken!))
            foreach (var tok in _requestToken)
            {
                DefaultRequestHeaders.Add(RequestTokenName, tok);
                _sessionToken = tok;
            }
        else
            throw new Exception(AuthError);
    }

    private async Task<HttpResponseMessage> SendFiles(List<StreamInfo> streamInfos, List<byte[]> streams, CancellationToken token)
    {
        var json = JsonConvert.SerializeObject(streamInfos);
        var jsonContent = new StringContent(json, Encoding.UTF8, JsonContent);
        using var multiPartContent = new MultipartFormDataContent();
        multiPartContent.Add(jsonContent);
        streams.ForEach(s => multiPartContent.Add(new ByteArrayContent(s)));
        var byteContent = await multiPartContent.ReadAsByteArrayAsync(token);

        var hash = ComputeHash(byteContent);
        AddSignatureHeader(hash, _storeStreamsToUri);
        return await PostAsync(_storeStreamsToUri, multiPartContent, token);
    }

    private async Task<ProcessedFilesResult> StoreAcceptedFilesAsync(Directory acceptedDirectory,
        CancellationToken token)
    {
        var revisionResult = await ListRevisionsAsync(acceptedDirectory.GetFilesIds(), token);
        var streamsInfo = revisionResult.Data;
        if (revisionResult.Type != ResultType.Success || streamsInfo == null) return new ProcessedFilesResult(revisionResult);

        long length = FileSizeLimit;
        var result = new ProcessedFilesResult(ResultType.Success);
        var streamsInfoToSend = new List<StreamInfo>();
        var streamsToSend = new List<byte[]>();

        foreach (var streamInfo in streamsInfo)
        {
            token.ThrowIfCancellationRequested();
            length -= acceptedDirectory.GetLength(streamInfo.StreamId);

            if (length < 0)
            {
                var response = await SendFiles(streamsInfoToSend, streamsToSend, token);
                ProcessSendingResponseAsync(response, result, streamsInfoToSend, token);
                streamsInfoToSend.Clear();
                streamsToSend.Clear();
                length = FileSizeLimit;
            }

            streamsInfoToSend.Add(streamInfo);
            streamsToSend.Add(acceptedDirectory.GetContent(streamInfo.StreamId));
        }

        if (streamsToSend.Count != 0)
        {
            var responseRemaining = await SendFiles(streamsInfoToSend, streamsToSend, token);
            ProcessSendingResponseAsync(responseRemaining, result, streamsInfoToSend, token);
            streamsInfoToSend.Clear();
            streamsToSend.Clear();
        }

        return result;
    }

    private async void ProcessSendingResponseAsync(HttpResponseMessage response, ProcessedFilesResult result, List<StreamInfo> streamsInfo,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (response.StatusCode != HttpStatusCode.OK)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized) await AuthenticateAsync(token);
            var ex = new Exception(response.ReasonPhrase);
            var processedStreams = streamsInfo.Select(s => new ProcessedStream(s, ex));
            result.UploadedWithErrorFiles.AddRange(processedStreams);
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync(token);
            var processedStreams = JsonSerializer.Deserialize<List<ProcessedStream>>(responseContent);
            if (processedStreams == null) return;
            foreach (var processedStream in processedStreams)
                if (!string.IsNullOrEmpty(processedStream.ErrorMessage))
                    result.UploadedWithErrorFiles.Add(processedStream);
                else
                    result.UploadedFiles.Add(processedStream);
        }
    }

    private async Task<ProcessedFilesResult> MarkFilesAsRejectedAsync(Directory rejectedDirectory,
        CancellationToken token)
    {
        var revisionResult = await ListRevisionsAsync(rejectedDirectory.GetFilesIds(), token);
        var streamsInfo = revisionResult.Data;
        if (revisionResult.Type != ResultType.Success || streamsInfo == null) return new ProcessedFilesResult(revisionResult);

        var result = new ProcessedFilesResult(ResultType.Success);

        var json = JsonConvert.SerializeObject(streamsInfo);
        var jsonContent = new StringContent(json, Encoding.UTF8, JsonContent);

        var hash = ComputeHash(json);
        AddSignatureHeader(hash, _markStreamsAsRejectedUri);

        var responseMessage = await PostAsync(_markStreamsAsRejectedUri, jsonContent, token);

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized) await AuthenticateAsync(token);
            var ex = new Exception(responseMessage.ReasonPhrase);
            var processedStreams = streamsInfo.Select(s => new ProcessedStream(s, ex));
            result.MarkedAsRejectedWithErrorFiles.AddRange(processedStreams);
        }
        else
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync(token);
            var processedStreams = JsonConvert.DeserializeObject<List<ProcessedStream>>(responseContent);
            if (processedStreams != null)
                foreach (var processedStream in processedStreams)
                    if (!string.IsNullOrEmpty(processedStream.ErrorMessage))
                        result.MarkedAsRejectedWithErrorFiles.Add(processedStream);
                    else
                        result.MarkedAsRejectedFiles.Add(processedStream);
        }

        return result;
    }

    private async Task<Result<IList<StreamInfo>>> ListRevisionsAsync(IList<string> streamIds, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(streamIds);
        var body = new StringContent(json, Encoding.UTF8, JsonContent);

        var hash = ComputeHash(json);
        AddSignatureHeader(hash, _listStreamInfoUri);

        var responseMessage = await PostAsync(_listStreamInfoUri, body, token);

        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized) await AuthenticateAsync(token);
            return new Result<IList<StreamInfo>>(ResultType.Exception, new Exception(responseMessage.ReasonPhrase));
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync(token);

        var streamInfo = JsonConvert.DeserializeObject<List<StreamInfo>>(responseContent);
        return new Result<IList<StreamInfo>>(ResultType.Success, streamInfo);
    }

    private void AddSignatureHeader(byte[] dataHash, Uri uri)
    {
        DefaultRequestHeaders.Remove(RequestSignature);

        var hashString = Convert.ToBase64String(dataHash);
        var urlPath = uri.LocalPath;
        var signature = string.Format(SignatureBody, urlPath, _sessionToken, hashString);
        var hash = ComputeHash(signature);
        var sb = Convert.ToBase64String(hash);

        DefaultRequestHeaders.TryAddWithoutValidation(RequestSignature, sb);
    }

    private byte[] ComputeHash(string data)
    {
        return _hasher.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    private byte[] ComputeHash(byte[] data)
    {
        return _hasher.ComputeHash(data);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            _hasher.Dispose();
        }

        base.Dispose(disposing);
    }
}