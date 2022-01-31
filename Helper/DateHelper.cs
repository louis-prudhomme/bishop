using System;
using System.Globalization;

namespace Bishop.Helper
{
    public class DateHelper
    {
        private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        
        public static DateTime FromTimestampToDateTime(long timestamp)
        {
            return DateTimeOffset
                .FromUnixTimeSeconds(timestamp).LocalDateTime;;
        }

        public static long FromDateTimeToTimestamp(DateTime date)
        {
            var span = date - UnixStart;
            return Convert.ToInt64(span.TotalSeconds);
        }

        public static string FromDateTimeToString(DateTime date)
        {
            return date.ToString("d", CultureInfo.GetCultureInfo("fr-FR"));
        }

        public static DateTime FromOldStringToDateTime(string oldDate)
        {
            return DateTime.Parse(oldDate, CultureInfo.GetCultureInfo("fr-FR"));
        }


    }
}