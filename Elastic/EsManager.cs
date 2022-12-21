using System.Net.Http.Headers;
using System.Text;

public class EsManager
{
    private string _esUrl;
    private HttpClient _client;

    public EsManager(string esUrl)
    {
        _esUrl = esUrl.LastOrDefault() == '/' ? esUrl[..^1] : esUrl;
        _client = new HttpClient();
    }

    public EsManager(string esUrl, HttpRequestHeaders headers)
    {
        _esUrl = esUrl.LastOrDefault() == '/' ? esUrl[..^1] : esUrl;

        _client = new HttpClient();
        foreach (var (key, value) in headers) _client.DefaultRequestHeaders.Add(key, value);
    }

    public async Task<HttpResponseMessage> PutAsync(string indexName, string? content = null)
    {
        var uri = BuildUri(indexName);
        return await SendToUri(uri, HttpMethod.Put, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string indexName, string docId, string? content = null)
    {
        var uri = BuildUri(indexName, docId);
        return await SendToUri(uri, HttpMethod.Put, content);
    }

    public async Task<HttpResponseMessage> SendAsync(string indexName, HttpMethod method, string? content = null)
    {
        var uri = BuildUri(indexName);
        return await SendToUri(uri, method, content);
    }

    public async Task<HttpResponseMessage> SendAsync(string indexName, string docId, HttpMethod method, string? content = null)
    {
        var uri = BuildUri(indexName, docId);
        return await SendToUri(uri, method, content);
    }

    private async Task<HttpResponseMessage> SendToUri(Uri uri, HttpMethod method, string? content = null)
    {
        var message = content != null
            ? new HttpRequestMessage(method, uri) { Content = new StringContent(content, Encoding.UTF8, "application/json") }
            : new HttpRequestMessage(method, uri);
        var responseMessage = await _client.SendAsync(message);
        return responseMessage;
    }


    private Uri BuildUri(string index)
    {
        return new Uri($"{_esUrl}/{index}");
    }

    private Uri BuildUri(string index, string docId)
    {
        return new Uri($"{_esUrl}/{index}/_doc/{docId}");
    }

    public List<string> GetIndexNames()
    {
        var getAllIndexesRequest = new HttpRequestMessage() { RequestUri = new Uri($"{_esUrl}/_cat/indices"), Method = HttpMethod.Get };
        var responseMessage = _client.Send(getAllIndexesRequest);
        var strings = responseMessage.Content.ReadAsStringAsync().Result.Split('\n');
        var indexesNames = new List<string>();
        foreach (var s in strings)
        {
            var start = s.IndexOf("cmw");
            if (start == -1)
            {
                continue;
            }

            var indexName = string.Concat(s.Skip(start).TakeWhile(c => c != ' '));
            indexesNames.Add(indexName);
        }

        return indexesNames;
    }
    
    
    public void DeleteIndexes(List<string> indexNames)
    {
        foreach (var indexName in indexNames)
        {
            DeleteIndex(indexName);
        }
    }
    public void DeleteIndex(string indexName)
    {
        _client.Send(new HttpRequestMessage(HttpMethod.Delete, $"{_esUrl}/{indexName}"));
    }
}