using System;
using System.Globalization;
using System.ServiceModel;

namespace WsdlTesting
{
    public class SoapClient
    {
        public const string Login = "website";
        public const string Password = "ru2qZlBlCwxiZ8GD";
        public const string WrongMockId = "116R0023213";
        public const string MockId = "116R00003";

        private const string GetMaterialsEndpoint = "https://prt-tst.portal.technoevolab.ru/XISOAPAdapter/MessageServlet?channel=:WebSite:CC_OUT_GDO_WS305_GET_MATERIALS&amp;version=3.0&amp;Sender.Service=WebSite&amp;Interface=http%3A%2F%2Fxerox.com%2Feurasia%2Fxi%2FCRM%2FGDO%5Emi_out_gdo_ws305_get_materials";
        private const string CreateMaterialsEndpoint = "https://prt-tst.portal.technoevolab.ru/XISOAPAdapter/MessageServlet?channel=:WebSite:CC_OUT_GDO_WS136_PURCH_REQ_CREATE&version=3.0&Sender.Service=WebSite&Interface=http%3A%2F%2Fxerox.com%2Feurasia%2Fxi%2FCRM%2FGDO%5Emi_out_gdo_ws136_purch_req_create";

        private const string GetMaterialsBinding = "mi_out_gdo_ws305_get_materialsBinding";
        private const string CreateRequestBinding = "mi_out_gdo_ws136_purch_req_createBinding";
        

        public ZXR_WS305_GET_MATERIALSResponse GetMaterials(string id = null)
        {
            var httpTransportSecurity = new HttpTransportSecurity() { ClientCredentialType = HttpClientCredentialType.Basic };
            var basicHttpsSecurity = new BasicHttpsSecurity() { Transport = httpTransportSecurity };
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport) { Name = GetMaterialsBinding, Security = basicHttpsSecurity };
            var endpointAddress = new EndpointAddress(GetMaterialsEndpoint);

            var miOutGdoWs305GetMaterialsClient = new mi_out_gdo_ws305_get_materialsClient(binding, endpointAddress)
            {
                ClientCredentials =
                {
                    UserName = { UserName = Login, Password = Password },
                    UseIdentityConfiguration = true
                },
            };

            miOutGdoWs305GetMaterialsClient.ChannelFactory.Endpoint.Address = new EndpointAddress(GetMaterialsEndpoint);
            var itMatnr = id != null
                ? new[] { new ZMATNR() { MATNR = id }, new ZMATNR() { MATNR = WrongMockId }, new ZMATNR() { MATNR = MockId } }
                : new[] { new ZMATNR() { MATNR = WrongMockId }, new ZMATNR() { MATNR = MockId } };

            return miOutGdoWs305GetMaterialsClient.mi_out_gdo_ws305_get_materials(new ZXR_WS305_GET_MATERIALS()
                { IT_MATNR = itMatnr });
        }

        public ZGdoWs136PurchReqCreateResponse CreateMaterials()
        {
            var httpTransportSecurity = new HttpTransportSecurity() { ClientCredentialType = HttpClientCredentialType.Basic };
            var basicHttpsSecurity = new BasicHttpsSecurity() { Transport = httpTransportSecurity };
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport) { Name = CreateRequestBinding, Security = basicHttpsSecurity };
            var endpointAddress = new EndpointAddress(CreateMaterialsEndpoint);

            var client = new mi_out_gdo_ws136_purch_req_createClient(binding, endpointAddress)
            {
                ClientCredentials =
                {
                    UserName = { UserName = Login, Password = Password },
                    UseIdentityConfiguration = true
                },
            };

            client.ChannelFactory.Endpoint.Address = new EndpointAddress(CreateMaterialsEndpoint);
            var zgdoPrItemCr = new ZgdoPrItemCr()
            {
                Matnr = "F006R01160",
                MatGr = "006R01160",
                Unit = "ST",
                Batch = "",
                Lgort = "GH04",
                Quantity = 2,
                Tdline = "",
                ZzgdoIncident = "70342",
                ZzgdoReason = "02",
                PrItemId = "1",
            };
            var prDate = DateTime.TryParseExact("2024-03-18", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed);
            var dateTime = new DateTime(2024, 03, 18);
            var reqCreate = new ZGdoWs136PurchReqCreate()
            {
                ItPrItem = new [] { zgdoPrItemCr },
                IvDate = "2024-03-18",
                IvUrgency = "A",
                IvCrmId = "0090005312",
                IvPrType = "ZGV",
                IvMContractId = "0007100160"
            };
            
            var miOutGdoWs136PurchReqCreate = client.mi_out_gdo_ws136_purch_req_create(reqCreate);
            var client2 = new mi_out_gdo_ws136_purch_req_createClient(binding, endpointAddress)
            {
                ClientCredentials =
                {
                    UserName = { UserName = Login, Password = Password },
                    UseIdentityConfiguration = true
                },
            };

            client2.ChannelFactory.Endpoint.Address = new EndpointAddress(CreateMaterialsEndpoint);
            var zgdoPrItemCr2 = new ZgdoPrItemCr()
            {
                Matnr = "F006R01160",
                MatGr = "006R01160",
                Unit = "ST",
                Batch = "",
                Lgort = "GH04",
                Quantity = 2,
                Tdline = "",
                ZzgdoIncident = "70342",
                ZzgdoReason = "02",
                PrItemId = "1",
            };
            var reqCreate2= new ZGdoWs136PurchReqCreate()
            {
                ItPrItem = new [] { zgdoPrItemCr2 },
                IvDate = "2024-03-18",
                IvUrgency = "A",
                IvCrmId = "0090005312",
                IvPrType = "ZGV",
                IvMContractId = "0007100160"
            };
            var miOutGdoWs136PurchReqCreate2 = client2.mi_out_gdo_ws136_purch_req_create(reqCreate2);
            return miOutGdoWs136PurchReqCreate;
        }
    }
}