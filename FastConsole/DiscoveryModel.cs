using System.Xml.Serialization;

namespace FastConsole;

// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(WopiDiscovery));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (WopiDiscovery)serializer.Deserialize(reader);
// }

[XmlRoot(ElementName="action")]
public class Action { 

	[XmlAttribute(AttributeName="name")] 
	public string Name { get; set; } 

	[XmlAttribute(AttributeName="ext")] 
	public string Ext { get; set; } 

	[XmlAttribute(AttributeName="urlsrc")] 
	public string Urlsrc { get; set; } 

	[XmlAttribute(AttributeName="targetext")] 
	public string Targetext { get; set; } 

	[XmlAttribute(AttributeName="requires")] 
	public string Requires { get; set; } 

	[XmlAttribute(AttributeName="default")] 
	public bool Default { get; set; } 
}

[XmlRoot(ElementName="app")]
public class App { 

	[XmlElement(ElementName="action")] 
	public List<Action> Action { get; set; } 

	[XmlAttribute(AttributeName="name")] 
	public string Name { get; set; } 

	[XmlAttribute(AttributeName="favIconUrl")] 
	public string FavIconUrl { get; set; } 
}

[XmlRoot(ElementName="net-zone")]
public class Netzone { 

	[XmlElement(ElementName="app")] 
	public List<App> App { get; set; } 

	[XmlAttribute(AttributeName="name")] 
	public string Name { get; set; } 
}

[XmlRoot(ElementName="proof-key")]
public class Proofkey { 

	[XmlAttribute(AttributeName="oldvalue")] 
	public string Oldvalue { get; set; } 

	[XmlAttribute(AttributeName="oldmodulus")] 
	public string Oldmodulus { get; set; } 

	[XmlAttribute(AttributeName="oldexponent")] 
	public string Oldexponent { get; set; } 

	[XmlAttribute(AttributeName="value")] 
	public string Value { get; set; } 

	[XmlAttribute(AttributeName="modulus")] 
	public string Modulus { get; set; } 

	[XmlAttribute(AttributeName="exponent")] 
	public string Exponent { get; set; } 
}

[XmlRoot(ElementName="wopi-discovery")]
public class Wopidiscovery { 

	[XmlElement(ElementName="net-zone")] 
	public Netzone Netzone { get; set; } 

	[XmlElement(ElementName="proof-key")] 
	public Proofkey Proofkey { get; set; } 
}

