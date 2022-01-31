using System;
using System.Globalization;

namespace Bishop.Helper
{
    public class DateHelper
    {
        /// <summary>
        ///     Unix Epoch.
        /// </summary>
        /// <seealso href="https://www.wikiwand.com/en/Unix_time" />
        /// <returns><see cref="DateTime" /> representing the Unix Epoch.</returns>
        private static readonly DateTime UnixStart = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Converts a unix timestamp to a <see cref="DateTime" />.
        ///     Should not be useful unless for debug purposes.
        /// </summary>
        /// <param name="timestamp">Unix timestamp to parse.</param>
        /// <returns>Corresponding <see cref="DateTime" />.</returns>
        public static DateTime FromTimestampToDateTime(long timestamp)
        {
            return DateTimeOffset
                .FromUnixTimeSeconds(timestamp).LocalDateTime;
            ;
        }

        /// <summary>
        ///     Converts a <see cref="DateTime" /> to the corresponding unix timestamp.
        /// </summary>
        /// <param name="date">To convert.</param>
        /// <returns>Timestamp.</returns>
        public static long FromDateTimeToTimestamp(DateTime date)
        {
            var span = date - UnixStart;
            return Convert.ToInt64(span.TotalSeconds);
        }

        /// <summary>
        ///     Formats a date to the <c>dd/mm/yyyy</c> representation.
        /// </summary>
        /// <param name="date">To format.</param>
        /// <returns>String representation of input <see cref="DateTime" /></returns>
        public static string FromDateTimeToString(DateTime date)
        {
            return date.ToString("d", CultureInfo.GetCultureInfo("fr-FR"));
        }

        /// <summary>
        ///     Used to parse and convert screxed up date format previously used into <see cref="DateTime" />
        /// </summary>
        /// <param name="oldDate">Old date format (should be <c>dd/mm/yyyy hh:mm:ss</c>)</param>
        /// <returns>Corresponding <see cref="DateTime" /> instance or throws</returns>
        [Obsolete]
        public static DateTime FromOldStringToDateTime(string oldDate)
        {
            return DateTime.Parse(oldDate, CultureInfo.GetCultureInfo("fr-FR"));
        }
    }
}