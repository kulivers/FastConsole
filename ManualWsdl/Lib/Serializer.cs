using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ManualWsdl.Lib.Entities.Requests;
using ManualWsdl.Lib.Entities.Responses;
using Body = ManualWsdl.Lib.Entities.Requests.Body;
using Envelope = ManualWsdl.Lib.Entities.Requests.Envelope;
using Header = ManualWsdl.Lib.Entities.Requests.Header;

namespace SoapXmlGenerator.Lib
{
    public class GetMaterialsSerializer
    {
        public GetMaterialsSerializer()
        {
            // Serialize the object to XML
        }

        public string Serialize(ZXR_WS305_GET_MATERIALS request)
        {
            var serializer = new XmlSerializer(typeof(Envelope));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                IndentChars = "    "
            };
            var envelope = new Envelope
            {
                Header = new Header(),
                Body = new Body
                {
                    ZXR_WS305_GET_MATERIALS = request
                }
            };

            using (var stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(writer, envelope, namespaces);
                }

                return stringWriter.ToString();
            }
        }

        public ZXR_WS305_GET_MATERIALSResponse Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof(ManualWsdl.Lib.Entities.Responses.Envelope), "http://schemas.xmlsoap.org/soap/envelope/");
            using var reader = new StringReader(xml);
            var deserialize = (ManualWsdl.Lib.Entities.Responses.Envelope)serializer.Deserialize(reader);
            return deserialize?.Body?.ZXR_WS305_GET_MATERIALS;
        }
    }
}