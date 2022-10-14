using FastConsole;
using SolTechnology.Avro;

internal class Program
{
    public static string JsonPath => Directory.GetCurrentDirectory() + "/abra.txt";

    public static async Task Main(string[] args)
    {
        var fileStream = File.Create(JsonPath);
        fileStream.Dispose();
        File.WriteAllText(JsonPath, "dtoJson");
    }
}