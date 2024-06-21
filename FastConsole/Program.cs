using Comindware.Bootloading.Core.Configuration;
using Comindware.Configs.Core;
using YamlDotNet.RepresentationModel;

public class Program
{
    public const string ymlPath = @"C:\Users\ekul\AppData\Roaming\JetBrains\Rider2023.3\scratches\mock.yml";
    public const string ymlPath2 = @"C:\Users\ekul\AppData\Roaming\JetBrains\Rider2023.3\scratches\mock2.yml";

    public static void Try()
    {
        
    }
    public static void Main()
    {
        File.Delete(ymlPath2);
        File.Copy(ymlPath, ymlPath2);
        var parser = new DotMappingParser(File.ReadAllText(ymlPath2));
        var stream = new YamlStream();
        stream.Load(parser);

        var propPath = new[] { "configName" };
        var propPath2 = new[] { "mq.server" };
        var changes = new YamlFieldCollection();
        var field = changes.CreateField(propPath);
        var field2 = changes.CreateField(propPath2);
        field.Value = "configName23";
        field2.Value = "configName23";
        var configChanger = new ConfigChanger();
        configChanger.RewriteFile(ymlPath2, changes);
        Console.WriteLine(File.ReadAllText(ymlPath2));
    }
}