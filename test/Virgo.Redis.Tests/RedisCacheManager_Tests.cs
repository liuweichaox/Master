using Autofac;
using Shouldly;
using System;
using System.Collections.Generic;
using Virgo.Cache;
using Virgo.Cache.Configuration;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Redis.Tests
{
    public class RedisCacheManager_Tests : TestBaseWithIocBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ITypedCache<string, MyCacheItem> _cache;
        public RedisCacheManager_Tests()
        {
            Building(builder =>
            {
                builder.RegisterType<CachingConfiguration>().As<ICachingConfiguration>().SingleInstance();
                var instance = new RedisCaCheConfiguration()
                {
                    DatabaseId = 0,
                    HostAndPort = "localhost:6379",
                    ConnectionString = "localhost:6379,Password=123456,ConnectTimeout=1000,ConnectRetry=1,SyncTimeout=10000"
                };
                builder.RegisterInstance<IRedisCaCheConfiguration>(instance);
                builder.RegisterType<RedisCacheProvider>().As<IRedisCacheProvider>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().SingleInstance();
            });
            _cacheManager = The<ICacheManager>();
            The<ICachingConfiguration>().ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(24);
            });
            The<ICachingConfiguration>().Configure("MyCacheItems", cache =>
             {
                 cache.DefaultSlidingExpireTime = TimeSpan.FromHours(1);
             });
            _cache = _cacheManager.GetCache<string, MyCacheItem>("MyCacheItems");
        }

        [Fact]
        public void Simple_Get_Set_Test()
        {
            _cache.GetOrDefault("A").ShouldBe(null);
            _cache.Set("A", new MyCacheItem { Value = 42 });
            _cache.GetOrDefault("A").ShouldNotBe(null);
            _cache.GetOrDefault("A").Value.ShouldBe(42);

            _cache.Get("B", () => new MyCacheItem { Value = 43 }).Value.ShouldBe(43);
            _cache.Get("B", () => new MyCacheItem { Value = 44 }).Value.ShouldBe(43); //不调用工厂，所以值不变

            var items1 = _cache.GetOrDefault(new string[] { "B", "C" });
            items1[0].Value.ShouldBe(43);
            items1[1].ShouldBeNull();

            var items2 = _cache.GetOrDefault(new string[] { "C", "D" });
            items2[0].ShouldBeNull();
            items2[1].ShouldBeNull();

            _cache.Set(new KeyValuePair<string, MyCacheItem>[] {
                new KeyValuePair<string, MyCacheItem>("C", new MyCacheItem{ Value = 44}),
                new KeyValuePair<string, MyCacheItem>("D", new MyCacheItem{ Value = 45})
            });

            var items3 = _cache.GetOrDefault(new string[] { "C", "D" });
            items3[0].Value.ShouldBe(44);
            items3[1].Value.ShouldBe(45);

            var items4 = _cache.Get(new string[] { "D", "E" }, (key) => new MyCacheItem { Value = key == "D" ? 46 : 47 });
            items4[0].Value.ShouldBe(45); //不调用工厂，所以值不变
            items4[1].Value.ShouldBe(47);
        }
    }
    [Serializable]
    public class MyCacheItem
    {
        public int Value { get; set; }

        public MyCacheItem()
        {

        }

        public MyCacheItem(int value)
        {
            Value = value;
        }
    }
}
