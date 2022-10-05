using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

public class HttpRequestMessageWrapper
{
    private HttpContent _content;

    public HttpContent Content
    {
        get
        {
            if (_content == null && Body == null)
                return null;
            if (Body == null && _content != null)
                return _content;
            if (_content == null && Body != null)
                return new StringContent(Body);
            if (_content.Headers == null && Body != null)
                return new StringContent(Body);

            var content = new StringContent(Body);
            foreach (var header in _content.Headers) content.Headers.Add(header.Key, header.Value);
            return content;
        }
        private set => _content = value;
    }


    public HttpMethod Method { get; set; }
    public Uri RequestUri { get; set; }

    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }
    public string Body { get; set; }

    // public HttpRequestMessageWrapper(HttpRequestMessage message, string body)
    // {
    //     Headers = message.Headers;
    //     Method = message.Method;
    //     RequestUri = message.RequestUri;
    //     Content = message.Content;
    //     Body = body;
    // }

    // public HttpRequestMessageWrapper()
    // {
        // var message = new HttpRequestMessage();
        // Headers = message.Headers;
    // }

    // public HttpRequestMessageWrapper(HttpRequestMessage message)
    // {
    //     Headers = message.Headers;
    //     Method = message.Method;
    //     RequestUri = message.RequestUri;
    //     Content = message.Content;
    //     Body = message.Content.ReadAsStringAsync().Result;
    // }

    public HttpRequestMessage ToHttpRequestMessage()
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            Content = Content, RequestUri = RequestUri,
        };
        httpRequestMessage.Method = Method;
        foreach (var header in Headers)
        {
            var key = header.Key;
            var value = header.Value;
            httpRequestMessage.Headers.Add(key, value);
        }

        return httpRequestMessage;
    }

    // public HttpRequestHeaders GetHttpRequestHeaders() => (HttpRequestHeaders)Headers;
}