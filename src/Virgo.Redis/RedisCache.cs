using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgo.Cache;

namespace Virgo.Redis
{
    /// <summary>
    /// 用于在Redis服务器中存储缓存
    /// </summary>
    public class RedisCache : CacheBase
    {
        /// <summary>
        /// key前缀
        /// </summary>
        private readonly string _prefix;

        private readonly IDatabase _database;
        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisCache(IRedisCacheProvider redisCacheProvider)
        {
            _database = redisCacheProvider.GetDatabase();
        }

        public override void Clear()
        {
            _database.KeyDeleteWithPrefix(GetLocalizedRedisKey("*"));
        }

        public override TValue GetOrDefault<TValue>(string key)
        {
            var obj = _database.StringGet(key);
            if (obj.IsNullOrEmpty) return default(TValue);
            return JsonConvert.DeserializeObject<TValue>(obj);
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public override void Set<TValue>(string key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("无法将空值插入缓存！");
            }
            _database.StringSet(key, JsonConvert.SerializeObject(value));
        }
        protected virtual RedisKey GetLocalizedRedisKey(string key)
        {
            return GetLocalizedKey(key);
        }
        protected virtual string GetLocalizedKey(string key)
        {
            return _prefix + ":" + key;
        }
    }
}
