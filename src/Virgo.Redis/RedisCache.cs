using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virgo.Cache;

namespace Virgo.Redis
{
    /// <summary>
    /// 用于在Redis服务器中存储缓存
    /// </summary>
    public class RedisCache : CacheBase
    {
        private readonly IDatabase _database;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisCache(
            string name,
            IRedisCacheProvider redisCacheProvider)
            : base(name)
        {
            _database = redisCacheProvider.GetDatabase();
        }

        public override object GetOrDefault(string key)
        {
            var objbyte = _database.StringGet(GetLocalizedRedisKey(key));
            return objbyte.HasValue ? Deserialize(objbyte) : null;
        }

        public override object[] GetOrDefault(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey);
            var redisValues = _database.StringGet(redisKeys.ToArray());
            var objbytes = redisValues.Select(obj => obj.HasValue ? Deserialize(obj) : null);
            return objbytes.ToArray();
        }

        public override async Task<object> GetOrDefaultAsync(string key)
        {
            var objbyte = await _database.StringGetAsync(GetLocalizedRedisKey(key));
            return objbyte.HasValue ? Deserialize(objbyte) : null;
        }

        public override async Task<object[]> GetOrDefaultAsync(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey);
            var redisValues = await _database.StringGetAsync(redisKeys.ToArray());
            var objbytes = redisValues.Select(obj => obj.HasValue ? Deserialize(obj) : null);
            return objbytes.ToArray();
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("无法将空值插入缓存！");
            }

            _database.StringSet(
                GetLocalizedRedisKey(key),Serialize(value),
                absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime
                );
        }

        public override async Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("无法将空值插入缓存！");
            }

            await _database.StringSetAsync(GetLocalizedRedisKey(key),Serialize(value),absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override void Set(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (pairs.Any(p => p.Value == null))
            {
                throw new Exception("无法将空值插入缓存！");
            }

            var redisPairs = pairs.Select(p => new KeyValuePair<RedisKey, RedisValue>
                                          (GetLocalizedRedisKey(p.Key), Serialize(p.Value))
                                         );

            if (slidingExpireTime.HasValue || absoluteExpireTime.HasValue)
            {
                Logger.LogWarning("Redis批量插入键值对不支持{0} / {1}", nameof(slidingExpireTime), nameof(absoluteExpireTime));
            }
            _database.StringSet(redisPairs.ToArray());
        }

        public override async Task SetAsync(KeyValuePair<string, object>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (pairs.Any(p => p.Value == null))
            {
                throw new Exception("无法将空值插入缓存！");
            }

            var redisPairs = pairs.Select(p => new KeyValuePair<RedisKey, RedisValue>(GetLocalizedRedisKey(p.Key), Serialize(p.Value)));
            if (slidingExpireTime.HasValue || absoluteExpireTime.HasValue)
            {
                Logger.LogWarning("Redis批量插入键值对不支持{0} / {1}", nameof(slidingExpireTime), nameof(absoluteExpireTime));
            }
            await _database.StringSetAsync(redisPairs.ToArray());
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(GetLocalizedRedisKey(key));
        }

        public override async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(GetLocalizedRedisKey(key));
        }

        public override void Remove(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey);
            _database.KeyDelete(redisKeys.ToArray());
        }

        public override async Task RemoveAsync(string[] keys)
        {
            var redisKeys = keys.Select(GetLocalizedRedisKey);
            await _database.KeyDeleteAsync(redisKeys.ToArray());
        }

        public override void Clear()
        {
            _database.KeyDeleteWithPrefix(GetLocalizedRedisKey("*"));
        }

        protected virtual Type GetSerializableType(object value)
        {
            var type = value.GetType();
            return type;
        }

        protected virtual string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        protected virtual object Deserialize(RedisValue objbyte)
        {
            return JsonConvert.DeserializeObject(objbyte);
        }

        protected virtual RedisKey GetLocalizedRedisKey(string key)
        {
            return GetLocalizedKey(key);
        }

        protected virtual string GetLocalizedKey(string key)
        {
            return "n:" + Name + ",k:" + key;
        }
    }
}
