using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using FastConsole;

public class Program
{

    public static async Task Main()
    {
        var path = @"D:\Work\master\Tests\Platform\bin\data\Temp\linkToNotEx";
        var resolveLinkTarget = Directory.ResolveLinkTarget(path, true);
        
    }
}
