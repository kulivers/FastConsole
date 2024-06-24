using Comindware.Bootloading.Core.Configuration.Entities;
using Comindware.Configs.Core;
using YamlDotNet.RepresentationModel;

public class Program
{
    private const string YmlPath = @"D:\dev\other\FastConsole\FastConsole\mock.yml";
    private const string YmlPath2 = @"C:\Users\kulivers\AppData\Roaming\JetBrains\Rider2023.3\scratches\instanceConfig2.yml";

    public static void Main()
    {
        var content = File.ReadAllText(YmlPath);
        var platformInstanceModel = new PlatformInstanceModel() {
            // ConfigName = "322",
            ElasticsearchUri = "1", Mq = new PlatformMessageQueueModel() { Server = "LIL UZI XYEVERT", Name = "XYEYM" } };
        var newContent = PlatformYmlSerializer.ApplyModelToContent(content, platformInstanceModel);
        File.WriteAllText(@"D:\dev\other\FastConsole\FastConsole\a.yml", newContent);
    }
}

public class ChangesCollectingVisitor : YamlVisitorBase
{
    public override void Visit(YamlStream stream)
    {
        base.Visit(stream);
    }

    public override void Visit(YamlDocument document)
    {
        base.Visit(document);
    }

    public override void Visit(YamlScalarNode scalar)
    {
        base.Visit(scalar);
    }

    public override void Visit(YamlSequenceNode sequence)
    {
        base.Visit(sequence);
    }

    public YamlMappingNode Mapping;

    public override void Visit(YamlMappingNode mapping)
    {
        Mapping = mapping;
        base.Visit(mapping);
    }
}