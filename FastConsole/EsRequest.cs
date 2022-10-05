namespace FastConsole;
using System.Text;



public class EsRequest
{
    public HostConfig HostConfig { get; set;}
    public RequestParameters RequestParameters { get; set;}
    public string Data { get; set;}

    public EsRequest()
    {
        
    }

    public EsRequest(HostConfig hostConfig, RequestParameters requestParameters, string data)
    {
        HostConfig = hostConfig;
        RequestParameters = requestParameters;
        Data = data;
    }

    private Uri BuildUri() => BuildUri(HostConfig, RequestParameters);

    private Uri BuildUri(HostConfig host, RequestParameters request)
    {
        return request.DocId != null
            ? new Uri($"{host.Scheme}://{host.Host}:{host.Port}/{request.Index}/{request.Type}/{request.DocId}")
            : new Uri($"{host.Scheme}://{host.Host}:{host.Port}/{request.Index}/{request.Type}");
    }

    public HttpRequestMessage ToHttpRequestMessage()
    {
        var method = RequestParameters.DocId == null ? HttpMethod.Post : HttpMethod.Put;
        var uri = BuildUri();
        return new HttpRequestMessage(method, uri)
        {
            Content = new StringContent(Data, Encoding.UTF8, "application/json"),
        };
    }
}



public class HostConfig 
{
    public string Scheme { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

    public HostConfig()
    {
        
    }

    public HostConfig(string host, int port, string scheme = "https")
    {
        Scheme = scheme;
        Host = host;
        Port = port;
    }
}public class RequestParameters
{
    public string Index { get; set; }
    public string? Type { get; set; }
    public string? DocId { get; set; }

    public RequestParameters(string index, string? type = "_doc", string? id = null)
    {
        Index = index;
        Type = type;
        DocId = id;
    }
}