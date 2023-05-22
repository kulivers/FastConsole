using System.Diagnostics;

public class Program
{
    public static async Task Main()
    {
        var tester = new Tester();
        var sw = new Stopwatch();
        sw.Start();
        var fs = tester.FiveSec();
        var ts = tester.ThreeSec();
        await Task.WhenAll(fs, ts);
        sw.Stop();
        Console.WriteLine(sw.Elapsed.Seconds);
    }
}

public class Tester
{
    public async Task<int> ThreeSec()
    {
        var threeSec = 3;
        await Task.Delay(threeSec * 1000);
        return threeSec;
    }

    public async Task<int> FiveSec()
    {
        var threeSec = 5;
        await Task.Delay(threeSec * 1000);
        return threeSec;
    }
}