using FastConsole.models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Version = System.Version;

namespace FastConsole;

public static class YAMLHelper
{
    private static IDeserializer Deserializer { get; }
    private static ISerializer Serializer { get; set; }

    static YAMLHelper()
    {
        Deserializer = new DeserializerBuilder().WithTypeConverter(new VersionYamlConverter()).WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        Serializer = new SerializerBuilder().WithTypeConverter(new VersionYamlConverter()).WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
    }

    public static string SerializeNComment(CloudConfig cloudConfig)
    {
        var content = Serialize(cloudConfig);
        var newContent = new List<string>();
        foreach (var line in content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith(CamelCaseNamingConvention.Instance.Apply(nameof(CloudConfig.Instance))) 
                || line.StartsWith(CamelCaseNamingConvention.Instance.Apply(nameof(CloudConfig.Product))) 
                || line.StartsWith(CamelCaseNamingConvention.Instance.Apply(nameof(CloudConfig.Template))))
            {
                newContent.Add($"{line}\n");
            }
            else
            {
                newContent.Add($"#{line}\n");
            }
        }
        return string.Concat(newContent);
    }
    public static bool WriteToFile(string path, object toWrite)
    {
        PrepareFilePath(path);
        var content = Serializer.Serialize(toWrite);
        File.WriteAllText(path, content);
        return true;
    }
    public static string Serialize(object obj)
    {
        return Serializer.Serialize(obj);
    }
    public static T ReadModel<T>(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        var config = File.ReadAllText(path);
        if (string.IsNullOrEmpty(config))
        {
            throw new FileLoadException(path);
        }

        var result = Deserializer.Deserialize<T>(config);
        return result;
    }
    public static T ReadContent<T>(string content)
    {
        return Deserializer.Deserialize<T>(content);
    }

    private static void PrepareFilePath(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException();
        }

        var fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            return;
        }

        if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }

        File.Create(fileInfo.FullName).Dispose();
    }
}
public class VersionYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(Version);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        if (parser.Current is Scalar scalar)
        {
            var versionString = parser.Consume<Scalar>().Value;
            if (string.IsNullOrEmpty(versionString))
            {
                return null;
            }
            return Version.Parse(versionString);
        }
        throw new YamlException("Expected scalar value");
    }
    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var version = (Version)value;
        var anchor = version == null ? string.Empty : version.ToString();
        emitter.Emit(new Scalar(anchor));
    }
}
