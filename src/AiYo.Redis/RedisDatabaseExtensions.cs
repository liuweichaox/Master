using StackExchange.Redis;
using System;

namespace Virgo.Redis
{
    /// <summary>
    /// <see cref ="IDatabase"/>的扩展方法
    /// </summary>
    internal static class RedisDatabaseExtensions
    {
        /// <summary>
        /// 通过keys进行模糊查询后的批量操作
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="prefix">前缀</param>
        public static void KeyDeleteWithPrefix(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            database.ScriptEvaluate(@"
                local keys = redis.call('keys', ARGV[1]) 
                for i=1,#keys,5000 do 
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { prefix });
        }

        /// <summary>
        /// 模糊查询给定前缀的数量
        /// </summary>
        /// <param name="database"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static int KeyCount(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            var retVal = database.ScriptEvaluate("return table.getn(redis.call('keys', ARGV[1]))", values: new RedisValue[] { prefix });

            if (retVal.IsNull)
            {
                return 0;
            }

            return (int)retVal;
        }
    }
}