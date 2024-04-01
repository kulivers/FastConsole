using System;
using System.Xml;
using System.Xml.Serialization;
using SoapXmlGenerator;

namespace WsdlTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var envelope = new Envelope
            {
                Header = new Header(),
                Body = new Body
                {
                    ZXR_WS305_GET_MATERIALS = new SoapXmlGenerator.ZXR_WS305_GET_MATERIALS
                    {
                        IT_MATNR = new IT_MATNR
                        {
                            Item = new Item
                            {
                                MATNR = "116R00003"
                            }
                        }
                    }
                }
            };

            // Serialize the object to XML
            var serializer = new XmlSerializer(typeof(Envelope));
            var namespaces = new XmlSerializerNamespaces();
            // namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            // namespaces.Add("urn", "urn:sap-com:document:sap:rfc:functions");

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(Console.Out, settings))
            {
                serializer.Serialize(writer, envelope, namespaces);
            }
        }
    }
}