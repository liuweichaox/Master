using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a DateTime to a Unix Timestamp
        /// </summary>
        /// <param name="time">This DateTime</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime time)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long ticks = (time - start.Add(new TimeSpan(8, 0, 0))).Ticks;
            return ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Converts a Unix Timestamp in to a DateTime
        /// </summary>
        /// <param name="unixTime">This Unix Timestamp</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this long unixTime)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            TimeSpan span = new TimeSpan(long.Parse(unixTime + "0000000"));
            return start.Add(span).Add(new TimeSpan(8, 0, 0));
        }
    }
}
