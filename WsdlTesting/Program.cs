using System;
using System.Configuration;

namespace WsdlTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var soapClient = new SoapClient();
            soapClient.GetMaterials();
        }
    }
}