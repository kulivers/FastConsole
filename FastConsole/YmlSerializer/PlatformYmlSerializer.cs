using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
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
            var emitter = new DotMappingEmitter(new ParsingEventsConverter());
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

        public static string ChangeValues<T>(string content, T obj)
        {
            var stream = new YamlStream();
            stream.Load(new StringReader(content));

            //content events reading
            var parsingEvents = YamlStreamConverter.ConvertFromDotMapping(stream);
            var converter = new ParsingEventsConverter(false);
            var contentEvents = converter.ConvertToDotMapping(parsingEvents).ToList();

            //model events reading
            var emitter = new DotMappingEmitter(new ParsingEventsConverter());
            Serializer.Serialize(emitter, obj);
            var modelEvents = emitter.GetConvertedEvents();
            var keyValueModelPairs = modelEvents.Where(@event => @event is Scalar).Cast<Scalar>().Select(scalar=>scalar.Value).Tupelize().ToDictionary(pair => pair.Item1, pair => pair.Item2);


            //getChangedFields 
            var changedFields = new Dictionary<string, string>();
            Scalar lastKey = null;
            foreach (var ev in contentEvents)
            {
                if (!(ev is Scalar scalar))
                {
                    // overridenContentEvents.Add(ev);
                    continue;
                }

                if (scalar.IsKey)
                {
                    lastKey = scalar;
                    continue;
                }


                var key = lastKey;
                if (keyValueModelPairs.TryGetValue(key.Value, out var newValue))
                {
                    changedFields.Add(key.Value, newValue);
                }
            }

            
            //get added fields
            var addedFields = keyValueModelPairs.Where(pair=>!changedFields.ContainsKey(pair.Key)).ToList();
            return default;
        }

        private static IEnumerable<Tuple<T, T>> Tupelize<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var item1 = enumerator.Current;

                    if (!enumerator.MoveNext())
                        throw new ArgumentException();

                    var item2 = enumerator.Current;

                    yield return new Tuple<T, T>(item1, item2);
                }
            }
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