using Comindware.Bootloading.Core.Configuration;
using Comindware.Bootloading.Core.Configuration.Entities;
using Comindware.Configs.Core;
using YamlDotNet.RepresentationModel;

public class Program
{
    public const string ymlPath = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\mock.yml";
    public const string ymlPath2 = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\mock2.yml";

    public static void Main()
    {
        // var model = PlatformYmlSerializer.ReadContent<PlatformInstanceModel>(File.ReadAllText(ymlPath));
        var platformInstanceModel = new PlatformInstanceModel() { ConfigName = "322", Mq = new PlatformMessageQueueModel(){Name = "LIL UZI XYEVERT"}};
        var model2 = PlatformYmlSerializer.ChangeValues<PlatformInstanceModel>(File.ReadAllText(ymlPath), platformInstanceModel);
        // PlatformYmlSerializer.WriteToFile(ymlPath2, model);
        Console.WriteLine(File.ReadAllText(ymlPath2));
        return;
    }
}