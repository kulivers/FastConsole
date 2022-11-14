using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Pers
{
    public int Age { get; set; }
    public string Name { get; set; }
    public List<int> Ints { get; set; }

    public Pers()
    {
        var ints = new List<int>() { 1, 231, 31, 31, 31, 31, 31, -1 };
        Ints = ints;
    }
}

public class PredicateDto
{
    public string[] Name { get; set; }
}

internal class Program
{
    public static async Task Main(string[] args)
    {
        string test = "dadladadasajljqlnqrqwq";
        var json = JsonConvert.SerializeObject(new {test});
        var jObject = JObject.Parse(json);
        var o = jObject.First.ToObject<string>();
    }

    public void dosome()
    {
        
    }
}
