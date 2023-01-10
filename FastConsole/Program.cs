using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;

public struct Person
{
    public int Age { get; set; }

    public Person(int age)
    {
        Age = age;
    }
}

class Program
{
    private const int WIN_1252_CP = 1200; // Windows ANSI codepage 1252

    public static async Task Main(string[] args)
    {
        var path = Directory.GetCurrentDirectory() + "1.txt";
        
        var encoding2 = Encoding.GetEncodings();
        var encoding = Encoding.GetEncoding(WIN_1252_CP);
        var encodingEncodingName = encoding.EncodingName;
        Console.WriteLine(encodingEncodingName);
        var _writer = new StreamWriter(path, false, encoding);
    }
}