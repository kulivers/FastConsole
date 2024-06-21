using Comindware.Bootloading.Core.Configuration.Entities;
using Comindware.Configs.Core;

public class Program
{
    private const string YmlPath = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\instanceConfig.yml";
    private const string YmlPath2 = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\instanceConfig2.yml";

    public static void Main()
    {
        var platformInstanceModel = new PlatformInstanceModel() { ConfigName = "322", ElasticsearchUri = "1", Mq = new PlatformMessageQueueModel(){Server = "LIL UZI XYEVERT", Name = "XYEYM"}};
        var content = PlatformYmlSerializer.ApplyModelToContent(File.ReadAllText(YmlPath), platformInstanceModel);
        // var changedContent = PlatformYmlSerializer.ChangeValues(File.ReadAllText(YmlPath), platformInstanceModel);
        // Console.WriteLine(changedContent);
        Console.WriteLine(content);
    }
}