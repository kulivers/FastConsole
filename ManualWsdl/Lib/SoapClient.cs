using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ManualWsdl.Lib.Entities.Requests;
using ManualWsdl.Lib.Entities.Responses;

namespace SoapXmlGenerator.Lib
{
    public class SoapClient
    {
        private HttpClient _httpClient;

        private Uri GetMaterialsRelativeEndpoint =>
            new(
                "XISOAPAdapter/MessageServlet?channel=:WebSite:CC_OUT_GDO_WS305_GET_MATERIALS&amp;version=3.0&amp;Sender.Service=WebSite&amp;Interface=http%3A%2F%2Fxerox.com%2Feurasia%2Fxi%2FCRM%2FGDO%5Emi_out_gdo_ws305_get_materials",
                UriKind.Relative);

        private Uri GetMaterialsEndpoint => new Uri(_baseUri, GetMaterialsRelativeEndpoint);

        private readonly Uri _createMaterialsEndpoint = //todo egor
            new(
                "XISOAPAdapter/MessageServlet?channel=:WebSite:CC_OUT_GDO_WS136_PURCH_REQ_CREATE&version=3.0&Sender.Service=WebSite&Interface=http%3A%2F%2Fxerox.com%2Feurasia%2Fxi%2FCRM%2FGDO%5Emi_out_gdo_ws136_purch_req_create",
                UriKind.Relative);

        private Uri _baseUri;
        private readonly GetMaterialsSerializer _getMaterialsSerializer;

        public SoapClient(string baseUri, string username, string password)
        {
            _baseUri = new Uri(baseUri, UriKind.Absolute);
            var credentials = $"{username}:{password}";
            var credentialsBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentialsBase64);
            _httpClient.DefaultRequestHeaders.Add("SOAPAction", "http://sap.com/xi/WebService/soap1.1");
            _getMaterialsSerializer = new GetMaterialsSerializer();
        }

        public ZXR_WS305_GET_MATERIALSResponse GetMaterials(ZXR_WS305_GET_MATERIALS request)
        {
            var httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = GetMaterialsEndpoint,
                Content = new StringContent(_getMaterialsSerializer.Serialize(request)),
                Method = HttpMethod.Post
            };
            var responseContent = _httpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult().Content;
            var responseXml = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
            return _getMaterialsSerializer.Deserialize(responseXml);
        }
    }
}