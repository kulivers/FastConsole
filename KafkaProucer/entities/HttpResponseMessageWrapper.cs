using System.Net;
using System.Net.Http.Headers;

public class HttpResponseMessageWrapper
{
    public readonly string ReasonPhrase;

    public readonly HttpStatusCode StatusCode;
    private HttpContent _content;

    public HttpResponseMessageWrapper(HttpResponseMessage message, string body)
    {
        Headers = message.Headers;
        Version = new VersionWrapper(message.Version);
        Content = message.Content;
        StatusCode = message.StatusCode;
        ReasonPhrase = message.ReasonPhrase;
        Body = body;
    }

    public HttpResponseMessageWrapper(HttpResponseMessage message)
    {
        Headers = message.Headers;
        Version = new VersionWrapper(message.Version);
        Content = message.Content;
        StatusCode = message.StatusCode;
        Content = message.Content;
        Body = message.Content.ReadAsStringAsync().Result;
    }

    public HttpContent Content
    {
        get
        {
            if (_content == null)
                return null;
            if (Body == null && _content != null)
                return _content;
            if (_content == null && Body != null)
                return new StringContent(Body);
            if (_content.Headers == null && Body != null)
                return new StringContent(Body);

            var content = new StringContent(Body);
            foreach (var headers in _content.Headers)
                try
                {
                    content.Headers.Add(headers.Key, headers.Value);
                }
                catch (FormatException) when (headers.Key == "Content-Type")
                {
                    //he doesnt like  "application/json; charset=utf-8", he likes only application/json  
                    var typeWithCharset = headers.Value.First();
                    var onlyType = typeWithCharset.Split(';').First();
                    content.Headers.ContentType = new MediaTypeHeaderValue(onlyType);
                }

            return content;
        }
        private set => _content = value;
    }

    public VersionWrapper Version { get; set; }

    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }
    public string Body { get; set; }
}

public class VersionWrapper
{
    public int Build;
    public int Major;
    public short MajorRevision;
    public int Minor;
    public short MinorRevision;
    public int Revision;

    public VersionWrapper(Version version)
    {
        Build = version.Build;
        Major = version.Major;
        Minor = version.Minor;
        Revision = version.Revision;
        MajorRevision = version.MajorRevision;
        MinorRevision = version.MinorRevision;
    }

    public VersionWrapper()
    {
    }

    public Version ToVersion()
    {
        return new(Major, Minor, Build, Revision);
    }
}