using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FastScratchMVC
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var json = File.ReadAllText(
                "D:\\Playground\\Garbage\\FastConsole\\FastScratchFramework\\mockResponse.json");
            var deserialized = JsonConvert.DeserializeObject<HttpRequestMessageWrapper>(json);
            // deserialized.Version
            var version = new Version();
            
        }

        public static void AddMockHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Connection.Add("keep-alive");

            client.DefaultRequestHeaders.Add("Cookie",
                "_ym_uid=1662392300728254634; _ym_d=1662392300; _ym_isad=1; Language=ru; __RequestVerificationToken=e_2XRZPyvW8jqSjwTrox0jLhHVSzrR-zNj07XBF7-xSEIBjhbKW-zwULhNvVO1GlQDVzKM2n-3RayjqvxUNICCkSdkXJjKmj7mWMbdW1kI41; SessionId=403ba3d8-b4bf-49ca-8a6b-e190b46c6499; UserId=account.1; TZ=180; ProtectorId=403ba3d8-b4bf-49ca-8a6b-e190b46c6499; _ym_visorc=w");

            client.DefaultRequestHeaders.Host = "localhost:8081";
            client.DefaultRequestHeaders.Referrer = new Uri("http://localhost:8081");


            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            client.DefaultRequestHeaders.Add("request-signature", "bC/D9ZaItcYQD7qldyDsTiiSqLg=");
            client.DefaultRequestHeaders.Add("request-token", "c426f14a97274af7ac03b128f14f4570");
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");

            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:C0m1ndw4r3Pl@tf0rm"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }
    }
}