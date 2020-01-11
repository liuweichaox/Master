using System;
using System.Diagnostics;

namespace Virgo.Extensions
{
    /// <summary>
    /// 随机数拓展
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// 生成真正的随机数
        /// </summary>
        /// <param name="r"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static int StrictNext(this Random r, int seed = Int32.MaxValue)
        {
            return new Random((int)Stopwatch.GetTimestamp()).Next(seed);
        }
    }
}
