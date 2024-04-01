using System;
using System.IO;
using System.Xml.Serialization;

public class Program
{
    public static void Main()
    {
        var xml = File.ReadAllText(@"D:\Playground\Garbage\FastConsole\ManualWsdl\MockResponse.xml");
        var serializer = new XmlSerializer(typeof(Envelope), "http://schemas.xmlsoap.org/soap/envelope/");
        using (var reader = new StringReader(xml))
        {
            var deserialize = (Envelope)serializer.Deserialize(reader);
            var itemMatnr = deserialize?.Body?.ZXR_WS305_GET_MATERIALS?.ET_MATNR?.item?.MATNR;
            if (deserialize?.Body?.ZXR_WS305_GET_MATERIALS?.ET_MATNR != null)
            {
                throw new Exception("DONE");
            }
        }
    }
}

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope
{
    public Header Header { get; set; }

    public Body Body { get; set; }

    [XmlAttribute(AttributeName = "soapenv", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Soapenv { get; set; } = "http://schemas.xmlsoap.org/soap/envelope/";

    [XmlAttribute(AttributeName = "n0", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string N0 { get; set; } = "urn:sap-com:document:sap:rfc:functions";
}

public class Header
{
}

public class Body
{
    [XmlElement(Namespace = "urn:sap-com:document:sap:rfc:functions", ElementName = "ZXR_WS305_GET_MATERIALSResponse")]
    public ZXR_WS305_GET_MATERIALSResponse ZXR_WS305_GET_MATERIALS { get; set; }
}

public class ZXR_WS305_GET_MATERIALSResponse
{
    public ET_MATNR ET_MATNR { get; set; }
}

public class ET_MATNR
{
    public Item item { get; set; }
}

public class Item
{
    public string MATNR { get; set; }
    public string DESCR { get; set; }
    public string MAT_GR { get; set; }
    public string UNIT { get; set; }
    public string PRICE { get; set; }
    public string PR_DATE { get; set; }
    public string MOLCOM { get; set; }
    public string MSGTY { get; set; }
    public string MSGTX { get; set; }
}