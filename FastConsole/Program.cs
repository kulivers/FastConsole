using Comindware.Bootloading.Core.Configuration.Entities;
using Comindware.Bootloading.Core.Configuration.Utils;

public class Program
{
    private const string YmlPath = @"C:\Users\ekul\AppData\Roaming\JetBrains\Rider2023.3\scratches\mock.yml";
    private const string YmlPath2 = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\instanceConfig2.yml";
    public static void Main()
    {
        var platformInstanceModel = new PlatformInstanceModel { ConfigName = "322", ElasticsearchUri = "1", Mq = new PlatformMessageQueueModel() { Server = "LIL UZI XYEVERT", Name = "XYEYM" } };
        var s = PlatformYmlSerializer.Serialize(platformInstanceModel);
        Console.WriteLine(File.ReadAllText(YmlPath2));
        // var changedContent = PlatformYmlSerializer.ChangeValues(File.ReadAllText(YmlPath), platformInstanceModel);
        // Console.WriteLine(changedContent);
    }
}