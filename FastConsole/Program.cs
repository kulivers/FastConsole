using System.Net;
using System.Text;
using System.Web;
using FastConsole;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var exists = Directory.Exists("/home/ekul/aaaa");
            Console.WriteLine(exists);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}