using System.Text;

public class ElasticQueryDto
{
    public Dictionary<string, IEnumerable<string>> Headers { get; set; }
    public HttpMethod Method { get; set; }
    public string Address { get; set; }
    public string Data { get; set; }

    public ElasticQueryDto(AuthenticationCredentials credentials, HttpMethod method, string address, string data)
    {
        Headers = new Dictionary<string, IEnumerable<string>>();
        Address = address;
        Method = method;
        Headers.Add(credentials.Type, new[] { credentials.Token });
        Data = data;
    }

    public ElasticQueryDto(AuthenticationCredentials credentials, HttpMethod method, Uri elasticUri, string data, string index, string type = "_doc",
        Guid? docId = null)
    {
        Headers = new Dictionary<string, IEnumerable<string>>();
        Address = BuildUri(elasticUri.ToString(), index, type, docId.ToString()).ToString();
        Method = method;
        //creds
        Headers.Add(credentials.Type, new[] { credentials.Token });
        Data = data;
    }

    public ElasticQueryDto(AuthenticationCredentials credentials, HttpMethod method, Uri elasticUri, string data, string index, Guid? docId = null)
    {
        Headers = new Dictionary<string, IEnumerable<string>>();
        Address = BuildUri(elasticUri.ToString(), index, "_doc", docId.ToString()).ToString();
        Method = method;
        //creds
        Headers.Add(credentials.Type, new[] { credentials.Token });
        Data = data;
    }

    private static Uri BuildUri(string elasticAddress, string index, string type = "_doc", string docId = null)
    {
        if (elasticAddress.EndsWith("/"))
        {
            elasticAddress = elasticAddress.Substring(0, elasticAddress.Length - 1);
        }

        return docId != null
            ? new Uri($"{elasticAddress}/{index}/{type}/{docId}")
            : new Uri($"{elasticAddress}/{index}/{type}");
    }
}


public abstract class AuthenticationCredentials
{
    public abstract string Type { get; }
    public abstract string Token { get; set; }
    public virtual string ToHeaderValue() => $"{Type} {Token}";
    public virtual Dictionary<string, IEnumerable<string>> ToDictionary() => new Dictionary<string, IEnumerable<string>>() { { Type, new[] { Token } } };
        
}

public class ApiKeyCredentials : AuthenticationCredentials
{
    public override string Type => "ApiKey";
    public override string Token { get; set; }

    public ApiKeyCredentials(string token)
    {
        Token = token;
    }

    public ApiKeyCredentials(string userName, string password)
    {
        var bytes = Encoding.UTF8.GetBytes($"{userName}:{password}");
        Token = Convert.ToBase64String(bytes);
    }
}

public class BarrierCredentials : AuthenticationCredentials
{
    public override string Type => "Barrier";
    public override string Token { get; set; }

    public BarrierCredentials(string token)
    {
        Token = token;
    }
}

public class BasicCredentials : AuthenticationCredentials
{
    public override string Type => "Basic";
    public override string Token { get; set; }

    public BasicCredentials(string token)
    {
        Token = token;
    }

    public BasicCredentials(string userName, string password)
    {
        var bytes = Encoding.UTF8.GetBytes($"{userName}:{password}");
        Token = Convert.ToBase64String(bytes);
    }
}
