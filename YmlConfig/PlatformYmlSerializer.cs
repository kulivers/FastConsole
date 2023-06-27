using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class PlatformYmlSerializer
{
    private static IDeserializer Deserializer { get; set; }
    private static ISerializer Serializer { get; }

    static PlatformYmlSerializer()
    {
        Deserializer = new DeserializerBuilder().WithTypeConverter(new VersionYamlConverter()).WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        Serializer = new SerializerBuilder().WithTypeConverter(new VersionYamlConverter()).WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
    }

    public static string Serialize(object obj)
    {
        return Serializer.Serialize(obj);
    }

    public static bool WriteToFile(string path, object toWrite)
    {
        PrepareFilePath(path);
        var content = Serializer.Serialize(toWrite);
        File.WriteAllText(path, content);
        return true;
    }


    public static bool WriteToFile(string path, string content)
    {
        PrepareFilePath(path);
        File.WriteAllText(path, content);
        return true;
    }

    public static T ReadModel<T>(string path)
    {
        if (!File.Exists(path))
        {
            return default;
        }

        var content = File.ReadAllText(path);
        if (string.IsNullOrEmpty(content))
        {
            throw new FileLoadException(path);
        }

        return Deserializer.Deserialize<T>(content);
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