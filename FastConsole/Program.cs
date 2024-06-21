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
        var platformInstanceModel = new PlatformInstanceModel() { ConfigName = "322", ElasticsearchUri = "1", Mq = new PlatformMessageQueueModel(){Server = "LIL UZI XYEVERT", Name = "XYEYM"}};
        var changedContent = PlatformYmlSerializer.ChangeValues(File.ReadAllText(ymlPath), platformInstanceModel);
        Console.WriteLine(changedContent);
    }
}