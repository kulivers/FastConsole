using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Version = System.Version;

namespace Comindware.Bootloading.Core.Configuration.Utils
{
    public class VersionYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Version);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.Current is Scalar)
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
}
