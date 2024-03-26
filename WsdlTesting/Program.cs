using System.Configuration;
using System.ServiceModel;

namespace WsdlTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var openMachineConfiguration = ConfigurationManager.OpenMachineConfiguration();
            var soapClient = new SoapClient();
            soapClient.Connect();
        }
    }

    public class SoapClient
    {
        public const string Login = "website";
        public const string Password = "ru2qZlBlCwxiZ8GD";
        public const string MockId = "116R00003";

        private const string EndpointAddress =
            "https://prt-tst.portal.technoevolab.ru/XISOAPAdapter/MessageServlet?channel=:WebSite:CC_OUT_GDO_WS305_GET_MATERIALS&amp;version=3.0&amp;Sender.Service=WebSite&amp;Interface=http%3A%2F%2Fxerox.com%2Feurasia%2Fxi%2FCRM%2FGDO%5Emi_out_gdo_ws305_get_materials";

        private const string BindingName = "mi_out_gdo_ws305_get_materialsBinding";

        public void Connect()
        {
            var httpTransportSecurity = new HttpTransportSecurity() { ClientCredentialType = HttpClientCredentialType.Basic };
            var basicHttpsSecurity = new BasicHttpsSecurity() { Transport = httpTransportSecurity };
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport) { Name = BindingName, Security = basicHttpsSecurity };
            var endpointAddress = new EndpointAddress(EndpointAddress);

            var miOutGdoWs305GetMaterialsClient = new mi_out_gdo_ws305_get_materialsClient(binding, endpointAddress)
            {
                ClientCredentials =
                {
                    UserName = { UserName = Login, Password = Password },
                    UseIdentityConfiguration = true
                },
            };
            var response = miOutGdoWs305GetMaterialsClient.mi_out_gdo_ws305_get_materials(new ZXR_WS305_GET_MATERIALS() { IT_MATNR = new[] { new ZMATNR() { MATNR = MockId } } });
        }
    }
}