
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Comindware.Configs.Core
{
    public class TimeOfDayYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(TimeOfDay);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.Current is Scalar)
            {
                var timeOfDayString = parser.Consume<Scalar>().Value;
                if (string.IsNullOrEmpty(timeOfDayString))
                {
                    return null;
                }

                if (!TimeOfDay.TryParse(timeOfDayString, out var timeOfDay))
                {
                    throw new YamlException($"Failed to parse string value for attribute of type {nameof(TimeOfDay)}");
                }

                return timeOfDay;
            }

            throw new YamlException("Expected scalar value");
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var timeOfDay = (TimeOfDay)value;
            var anchor = timeOfDay == null ? string.Empty : timeOfDay.ToString();
            emitter.Emit(new Scalar(anchor));
        }
    }

    public class TimeOfDay
    {
        public int Hour { get; set; }
        public int Minute { get; set; }

        public TimeOfDay()
        {
        }

        public TimeOfDay(int hour, int minute)
        {
            if (!ValidateHour(hour))
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }

            if (!ValidateMinute(minute))
            {
                throw new ArgumentOutOfRangeException(nameof(minute));
            }

            Hour = hour;
            Minute = minute;
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}";
        }

        public static bool TryParse(string value, out TimeOfDay time)
        {
            time = null;
            var timeParts = value.Split(':');
            if (timeParts.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(timeParts[0], out var hour) || !ValidateHour(hour))
            {
                return false;
            }

            if (!int.TryParse(timeParts[1], out var minute) || !ValidateMinute(minute))
            {
                return false;
            }

            time = new TimeOfDay() { Hour = hour, Minute = minute };

            return true;
        }

        private static bool ValidateHour(int hour)
        {
            if (hour < 0 || hour > 23)
            {
                return false;
            }

            return true;
        }

        private static bool ValidateMinute(int minute)
        {
            if (minute < 0 || minute > 59)
            {
                return false;
            }

            return true;
        }
    }

    public enum DayOfWeek
    {
        Undefined,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
}