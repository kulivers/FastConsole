using System.Net;
using System.Text;
using System.Web;
using FastConsole;

public class Program
{
    public static async Task Main()
    {
        var _linuxBashInvoker = new LinuxBashInvoker();
        var bash = _linuxBashInvoker.Bash($"ln -s /home/ekul/ /home/ekul/aaaa", true);
        Console.WriteLine(bash);
        Console.WriteLine("-=------------------------------------");
        var bash2 = _linuxBashInvoker.Bash($"ln -s qweqw qqweqw qewqewqw", true);
        Console.WriteLine(bash2);
    }
}