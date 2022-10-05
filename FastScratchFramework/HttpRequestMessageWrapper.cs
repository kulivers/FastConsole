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

            var saved = _content.Headers;
            var content = new StringContent(Body);
            foreach (var header in saved) content.Headers.Add(header.Key, header.Value);
            return content;
        }
        private set => _content = value;
    }

    public Version Version { get; set; }
    public HttpMethod Method { get; set; }
    public Uri RequestUri { get; set; }

    private IEnumerable<KeyValuePair<string, IEnumerable<string>>> _headers;
    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers
    {
        get => _headers;
        set
        {
            foreach (var pair in value) HttpRequestHeaders.Add(pair.Key, pair.Value);
            _headers = value;
        }
    }

    public HttpRequestHeaders HttpRequestHeaders { get; set; }
    public string Body { get; set; }

    public HttpRequestMessageWrapper(HttpRequestMessage message, string body)
    {
        HttpRequestHeaders = message.Headers;
        Version = message.Version;
        Method = message.Method;
        RequestUri = message.RequestUri;
        Content = message.Content;
        Body = body;
    }

    public HttpRequestMessageWrapper()
    {
        var message = new HttpRequestMessage();
        HttpRequestHeaders = message.Headers;
    }

    public HttpRequestMessageWrapper(HttpRequestMessage message)
    {
        HttpRequestHeaders = message.Headers;
        Version = message.Version;
        Method = message.Method;
        RequestUri = message.RequestUri;
        Content = message.Content;
        Body = message.Content.ReadAsStringAsync().Result;
    }

    public HttpRequestMessage ToHttpRequestMessage()
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            Content = Content, Version = Version, Method = Method, RequestUri = RequestUri,
        };
        foreach (var header in HttpRequestHeaders)
        {
            var key = header.Key;
            var value = header.Value;
            httpRequestMessage.Headers.Add(key, value);
        }

        return httpRequestMessage;
    }
}