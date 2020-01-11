using System;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref="DateTime"/>的扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// DateTime时间格式转换为时间戳格式[13位时间戳]
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static long ConvertDateTimeToLong(this DateTime datetime)
        {
            DateTime UnixTimestampLocalZero = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            return (long)(datetime - UnixTimestampLocalZero).TotalMilliseconds;
        }
        /// <summary>
        /// 将本地时间戳转为C#格式时间[13位时间戳]   
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertLocalFromTimestamp(this long timestamp)
        {
            DateTime UnixTimestampLocalZero = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            return UnixTimestampLocalZero.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 将日期时间转换为Unix时间戳[10位时间戳]
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
        /// 将中的Unix时间戳转换为日期时间[10位时间戳]
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
