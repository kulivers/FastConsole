using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Comindware.Configs.Core
{
    public static class PlatformYmlSerializer
    {
        private static IDeserializer Deserializer { get; }
        private static ISerializer Serializer { get; }

        static PlatformYmlSerializer()
        {
            Deserializer = new DeserializerBuilder()
                            .WithTypeConverter(new VersionYamlConverter())
                            .WithTypeConverter(new TimeSpanYamlConverter())
                            .WithTypeConverter(new TimeOfDayYamlConverter())
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
            Serializer = new SerializerBuilder()
                            .WithTypeConverter(new VersionYamlConverter())
                            .WithTypeConverter(new TimeSpanYamlConverter())
                            .WithTypeConverter(new TimeOfDayYamlConverter())
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
        }

        public static string Serialize(object obj)
        {
            var emitter = new DotMappingEmitter();
            Serializer.Serialize(emitter, obj);
            return emitter.GetSerializedObject();
        }

        public static bool WriteToFile(string path, object toWrite)
        {
            PrepareFilePath(path);
            var content = Serialize(toWrite);
            File.WriteAllText(path, content);
            return true;
        }

        public static bool WriteChanges<TModel>(string path, TModel model)
        {
            if (File.Exists(path))
            {
                return WriteToFile(path, model);
            }

            TModel existingModel;
            try
            {
                existingModel = ReadContent<TModel>(File.ReadAllText(path));
            }
            catch
            {
                return WriteToFile(path, model);
            }

            
            return true;
        }

        private static Dictionary<string, string> GetChangedProps<TModel>(TModel source, TModel target)
        {
            // source.
            throw new NotImplementedException();
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

            return ReadContent<T>(content);
        }

        public static T ReadContent<T>(string content)
        {
            var parser = new DotMappingParser(content);
            return Deserializer.Deserialize<T>(parser);
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
}