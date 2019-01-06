using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref=“DateTime”/>的扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 将日期时间转换为Unix时间戳
        /// </summary>
        /// <param name="time">此日期时间</param>
        /// <returns>UNIX时间戳</returns>
        public static long ToUnixTimestamp(this DateTime time)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long ticks = (time - start.Add(new TimeSpan(8, 0, 0))).Ticks;
            return ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// 将中的Unix时间戳转换为日期时间
        /// </summary>
        /// <param name="unixTime">这个Unix时间戳</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this long unixTime)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            TimeSpan span = new TimeSpan(long.Parse(unixTime + "0000000"));
            return start.Add(span).Add(new TimeSpan(8, 0, 0));
        }
    }
}
