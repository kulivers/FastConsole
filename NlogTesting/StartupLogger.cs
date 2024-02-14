using NLog;
using NLog.Targets;
using NLog.Web;

internal class StartupLogger : IDisposable
{
    private readonly Logger _logger;

    public StartupLogger()
    {
        LogManager.Setup().LoadConfigurationFromAppSettings();
        _logger = LogManager.GetLogger("Adapter");
        Test();
    }

    private static void Test()
    {
        try
        {
            Console.WriteLine($"Created logger, LogManager.Configuration.FileNamesToWatch = {LogManager.Configuration?.FileNamesToWatch}");
            var configuration = new LogFactory().Configuration;
            var render = configuration.FindTargetByName<FileTarget>("adapterFile")?.FileName.Render(LogEventInfo.CreateNullEvent());
            Console.WriteLine($"adapters writing to {render}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void LogError(string message)
    {
        Console.WriteLine(message);
        _logger.Error(message);
        LogManager.Flush();
    }

    public void LogInfo(string message)
    {
        Console.WriteLine(message);
        _logger.Info(message);
        LogManager.Flush();
    }

    public void LogWarning(string message, Exception ex)
    {
        Console.WriteLine(message);
        _logger.Warn(message, ex);
        LogManager.Flush();
    }

    public void Dispose()
    {
        Console.WriteLine("Shutdown logger");
        LogManager.Shutdown();
    }
}