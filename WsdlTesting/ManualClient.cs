using System;
using System.Xml;
using System.Xml.Serialization;

namespace SoapXmlGenerator
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "xmlns:soapenv")]
        public string Soapenv { get; set; } = "http://schemas.xmlsoap.org/soap/envelope/";
        [XmlAttribute(AttributeName = "xmlns:urn")]
        public string Urn { get; set; } = "urn:sap-com:document:sap:rfc:functions";
    }

    public class Header { }

    public class Body
    {
        [XmlElement(ElementName = "ZXR_WS305_GET_MATERIALS", Namespace = "urn:sap-com:document:sap:rfc:functions")]
        public ZXR_WS305_GET_MATERIALS ZXR_WS305_GET_MATERIALS { get; set; }
    }

    public class ZXR_WS305_GET_MATERIALS
    {
        [XmlElement(ElementName = "IT_MATNR", Namespace = "urn:sap-com:document:sap:rfc:functions")]
        public IT_MATNR IT_MATNR { get; set; }
    }

    public class IT_MATNR
    {
        [XmlElement(ElementName = "item", Namespace = "urn:sap-com:document:sap:rfc:functions")]
        public Item Item { get; set; }
    }

    public class Item
    {
        [XmlElement(ElementName = "MATNR", Namespace = "urn:sap-com:document:sap:rfc:functions")]
        public string MATNR { get; set; }
    }

}
