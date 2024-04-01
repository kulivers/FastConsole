// using System;
// using System.IO;
// using System.Xml;
// using System.Xml.Serialization;
// using ManualWsdl.Lib.Entities.Requests;
//
// namespace SoapXmlGenerator.Lib
// {
//     public class GetMaterialsSerializer
//     {
//         private XmlSerializer serializer;
//         private XmlSerializerNamespaces _namespaces;
//         private XmlWriterSettings _settings;
//
//         public GetMaterialsSerializer()
//         {
//             // Serialize the object to XML
//             serializer = new XmlSerializer(typeof(ManualWsdl.Lib.Entities.Requests.Envelope));
//             var namespaces = new XmlSerializerNamespaces();
//             _namespaces = namespaces;
//             _namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
//
//             _settings = new XmlWriterSettings
//             {
//                 Indent = true,
//                 OmitXmlDeclaration = true,
//                 IndentChars = "    "
//             };
//         }
//
//         public string Serialize(ZXR_WS305_GET_MATERIALS request)
//         {
//             var envelope = new ManualWsdl.Lib.Entities.Requests.Envelope
//             {
//                 Header = new ManualWsdl.Lib.Entities.Requests.Header(),
//                 Body = new ManualWsdl.Lib.Entities.Requests.Body
//                 {
//                     ZXR_WS305_GET_MATERIALS = request
//                 }
//             };
//             
//             using (var stringWriter = new StringWriter())
//             {
//                 using (var writer = XmlWriter.Create(stringWriter, _settings))
//                 {
//                     serializer.Serialize(writer, envelope, _namespaces);
//                 }
//                 return stringWriter.ToString();
//             }
//         }
//     }
// }