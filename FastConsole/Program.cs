using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using FastConsole;

public static class DateTimeFactory
    {
        public static DateTime UniversalNow => DateTime.UtcNow;

        public static DateTime ServerNow => ToServerTime(DateTime.UtcNow);

        public static DateTime ServerMaxDateTime => ToServerTime(DateTime.MaxValue);

        public static DateTime ServerMinDateTime => ToServerTime(DateTime.MinValue);

        public static DateTime ToServerTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime;
            }
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                var tzServer = TimeZoneFactory.GetServerTimeZoneInfo();
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, tzServer);
            }
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }

        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            var tzServer = TimeZoneFactory.GetServerTimeZoneInfo();
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, tzServer);
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, tzServer);
            }
            return dateTime;
        }

        public static string ToUniversalTimeString(DateTime dateTime)
        {
            return ToUniversalTime(dateTime).ToString("O");
        }
        
        public static DateTime TruncateTime(DateTime dateTime)
        {
            var localDate = ToServerTime(dateTime).Date;
            var utcDate = ToUniversalTime(localDate);
            return utcDate;
        }

        public static bool IsWorkDay(DateTime dateTime)
        {
            return dateTime.DayOfWeek != DayOfWeek.Saturday && dateTime.DayOfWeek != DayOfWeek.Sunday;
        }

        public static IList<DateTime> CountWorkDays(DateTime startDateTime, DateTime endDateTime)
        {
            var workDays = new List<DateTime>();
            var currentDate = startDateTime;
            while (currentDate <= endDateTime)
            {
                if (IsWorkDay(currentDate))
                {
                    workDays.Add(currentDate.Date);
                }
                currentDate = currentDate.AddDays(1);
            }
            return workDays;
        }

        public static DateTime ConvertTime(DateTime sourceDateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone, DateTimeKind kind = DateTimeKind.Utc)
        {
            return DateTime.SpecifyKind(TimeZoneInfo.ConvertTime(sourceDateTime, sourceTimeZone, destinationTimeZone), kind);
        }

        #region date time milestones for now

        public static DateTime LastYear(DateTime dateTime)
        {
            return CurrentYear(dateTime).AddYears(-1);
        }
        public static DateTime LastQuarter(DateTime dateTime)
        {
            return CurrentQuarter(dateTime).AddMonths(-3);
        }
        public static DateTime LastMonth(DateTime dateTime)
        {
            return CurrentMonth(dateTime).AddMonths(-1);
        }

        public static DateTime LastWeek(DateTime dateTime)
        {
            return CurrentWeek(dateTime).AddDays(-7);
        }

        public static DateTime Yesterday(DateTime dateTime)
        {
            return dateTime.AddDays(-1).Date;
        }

        public static DateTime CurrentHour(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, dateTime.Hour, 0, 0);
        }

        public static DateTime Today(DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime CurrentWeek(DateTime dateTime)
        {
            return dateTime.AddDays(DayOfWeek.Monday - dateTime.DayOfWeek).Date;
        }

        public static DateTime CurrentMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime CurrentQuarter(DateTime dateTime)
        {
            var quarter = (dateTime.Month + 2) / 3;
            var quarterStartMonth = 3 * quarter - 2;
            return new DateTime(dateTime.Year, quarterStartMonth, 1);
        }

        public static DateTime CurrentYear(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public static DateTime Tommorow(DateTime dateTime)
        {
            return dateTime.AddDays(1).Date;
        }

        public static DateTime NextWeek(DateTime dateTime)
        {
            return CurrentWeek(dateTime).AddDays(7);
        }

        public static DateTime NextMonth(DateTime dateTime)
        {
            return CurrentMonth(dateTime).AddMonths(1);
        }

        public static DateTime NextQuarter(DateTime dateTime)
        {
            return CurrentQuarter(dateTime).AddMonths(3);
        }

        public static DateTime NextYear(DateTime dateTime)
        {
            return CurrentYear(dateTime).AddYears(1);
        }

        #endregion
public class Program
{

    public static async Task Main()
    {
        DateTimeFactory.UniversalNow - DateTimeFactory.ToUniversalTime(new DateTime(testCase.Timestamp))
    }

    public static void Clg(Tester tester)
    {
        tester.a = 1;
        tester.a++;
        tester.a++;
        tester.a++;
        tester.a++;
        tester.a++;
        Console.WriteLine(tester.a);
        tester.a = default;
    }

    public static void Change(Tester tester)
    {
        tester.a = 1;
        tester.b = "bas";
    }
}

public class Tester
{
    public int a { get; set; } = 0;
    public string b { get; set; }
}