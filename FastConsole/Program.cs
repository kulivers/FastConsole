using FastConsole;
using SolTechnology.Avro;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var ints1 = new List<int>() { 1, 2, 3 }.Select(i => (object)i).ToArray();
        var ints2 = new List<int>() { 1, 2, 3 }.Select(i => (object)i).ToArray();
        var equals = ints1.Equals(ints2);
        var a = 1;
        while (true)
        {
            a++;
        }
    }
}