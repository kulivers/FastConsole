using System.Diagnostics;

public class Program
{
    public static async Task Main()
    {
        var tester = new Tester();
        for (int i = 0; i < 1000; i++)
        {
            Task.Run(()=>Clg(tester));
        }

        await Task.Delay(3000);
    }

    public static void Clg(Tester tester)
    {
        tester.a = 1;
        tester.a++;
        tester.a++;
        tester.a++;
        tester.a++;
        tester.a++;
        Console.WriteLine(tester.a);
        tester.a = default;
    }
    public static void Change(Tester tester)
    {
        tester.a = 1;
        tester.b = "bas";
    }
}

public class Tester
{
    public int a { get; set; } = 0;
    public string b { get; set; }
}