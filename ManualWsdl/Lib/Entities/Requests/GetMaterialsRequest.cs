﻿using System.Xml.Serialization;

namespace ManualWsdl.Lib.Entities.Requests
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }

        [XmlAttribute(AttributeName = "soapenv", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soapenv { get; set; } = "http://schemas.xmlsoap.org/soap/envelope/";

        [XmlAttribute(AttributeName = "urn", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Urn { get; set; } = "urn:sap-com:document:sap:rfc:functions";
    }

    public class Header
    {
    }

    public class Body
    {
        [XmlElement(ElementName = "ZXR_WS305_GET_MATERIALS", Namespace = "urn:sap-com:document:sap:rfc:functions")]
        public ZXR_WS305_GET_MATERIALS ZXR_WS305_GET_MATERIALS { get; set; }
    }

    public class ZXR_WS305_GET_MATERIALS
    {
        [XmlElement(ElementName = "IT_MATNR", Namespace = "")]
        public IT_MATNR IT_MATNR { get; set; }
    }

    public class IT_MATNR
    {
        [XmlElement(ElementName = "item", Namespace = "")]
        // [XmlArrayItem("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Item[] Item { get; set; }
    }

    public class Item
    {
        [XmlElement(ElementName = "MATNR", Namespace = "")]
        public string MATNR { get; set; }
    }
}