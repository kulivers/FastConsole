using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Comindware.Configs
{
    public class TimeSpanYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(TimeSpan?);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.Current is Scalar)
            {
                var timeSpanString = parser.Consume<Scalar>().Value;
                if (string.IsNullOrEmpty(timeSpanString))
                {
                    return null;
                }

                return TimeSpan.Parse(timeSpanString);
            }
            else
            {
                return ReadTimeSpan(parser);
            }

            throw new YamlException("Expected scalar value");
        }

        private TimeSpan? ReadTimeSpan(IParser parser)
        {
            int days = 0;
            int hours = 0;
            int minutes = 0;

            if (!parser.TryConsume<MappingStart>(out _))
            {
                return null;
            }

            while (!parser.TryConsume<MappingEnd>(out _))
            {
                var stringValue = parser.Consume<Scalar>().Value;
                if (stringValue == nameof(TimeSpan.Days).ToLower())
                {
                    days = int.Parse(parser.Consume<Scalar>().Value);
                }

                if (stringValue == nameof(TimeSpan.Hours).ToLower())
                {
                    hours = int.Parse(parser.Consume<Scalar>().Value);
                }

                if (stringValue == nameof(TimeSpan.Minutes).ToLower())
                {
                    minutes = int.Parse(parser.Consume<Scalar>().Value);
                }
            }

            return new TimeSpan(days, hours, minutes, 0);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var timeSpan = (TimeSpan?)value;
            var anchor = timeSpan == null ? string.Empty : timeSpan.ToString();
            emitter.Emit(new Scalar(anchor));
        }
    }
}
