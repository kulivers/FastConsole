using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Comindware.Bootloading.Core.Configuration.Utils
{
    public static class PlatformYmlSerializer
    {
        private static IDeserializer Deserializer { get; set; }
        private static ISerializer Serializer { get; }

        static PlatformYmlSerializer()
        {
            Deserializer = new DeserializerBuilder()
                            .WithTypeConverter(new VersionYamlConverter())
                            .WithTypeConverter(new TimeSpanYamlConverter())
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
            Serializer = new SerializerBuilder()
                            .WithTypeConverter(new VersionYamlConverter())
                            .WithTypeConverter(new TimeSpanYamlConverter())
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