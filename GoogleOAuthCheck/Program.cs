using System;
using System.Net;
using System.Threading;

namespace ConsoleApplication1
{
    internal class Program
    {
        private const string _unsuccessfullLoginRedirection = "/signin/oauth/error";
        private const string _googleAuthEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string _redirectUri = "https://localhost:44328/signin-channel.3";
        private const string _clientId = "424993899577-o99c4v3hc47duhjaq8sv55lrmqclr7cn.apps.googleusercontent.com";
        public static void Main(string[] args)
        {
            var authEndpoint = new Uri(_googleAuthEndpoint, UriKind.Absolute);
            var relativeUri = $"?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope=openid%20profile%20email";
            if (Uri.TryCreate(authEndpoint, relativeUri, out var requestUri))
            {
                var req = WebRequest.Create(requestUri);
                var resp = (HttpWebResponse)req.GetResponse();
                var respPath = resp.ResponseUri.AbsolutePath;

                if (respPath != _unsuccessfullLoginRedirection)
                {
                    Console.WriteLine("SuccessfullTestResult();");
                    return;
                }
            }

            Console.WriteLine(" FailedTestResult;");
        }
    }
}