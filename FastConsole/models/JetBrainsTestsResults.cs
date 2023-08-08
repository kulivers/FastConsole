using System.Xml.Serialization;

[XmlRoot(ElementName="Test")]
public class Test { 

    [XmlElement(ElementName="TestId")] 
    public List<string> TestId { get; set; } 
}