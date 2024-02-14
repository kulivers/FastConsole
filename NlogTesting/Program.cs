// See https://aka.ms/new-console-template for more information

internal class Program
{
    public static void Main(string[] args)
    {
        var logger = new StartupLogger();
        logger.LogInfo("inited");
    }
}