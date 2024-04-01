using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ManualWsdl.Lib.Entities.Requests;
using SoapXmlGenerator.Lib;
using Item = ManualWsdl.Lib.Entities.Requests.Item;

public class Program
{
    public static void Main()
    {
        var username = "website";
        var password = "ru2qZlBlCwxiZ8GD";
        var soapClient = new SoapClient("https://prt-tst.portal.technoevolab.ru", username, password);
        var res = soapClient.GetMaterials(new ZXR_WS305_GET_MATERIALS() { IT_MATNR = new IT_MATNR() { Item = new Item() { MATNR = "116R00003" } } });
    }
}