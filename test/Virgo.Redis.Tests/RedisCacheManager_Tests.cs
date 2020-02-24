using Autofac;
using Shouldly;
using System;
using System.Collections.Generic;
using Virgo.Cache;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Redis.Tests
{
    public class RedisCacheManager_Tests : TestBaseWithIocBuilder
    {
        private readonly ICache _cache;
        public RedisCacheManager_Tests()
        {
            Building(builder =>
            {
                builder.RegisterType<RedisCache>().As<ICache>().SingleInstance();
                builder.RegisterType<RedisCacheProvider>().As<IRedisCacheProvider>().SingleInstance();
            });
            _cache = The<ICache>();
        }

        [Fact]
        public void Simple_Get_Set_Test()
        {
            //获取空key
            _cache.GetOrDefault<MyCacheItem>("A").ShouldBe(null);
            //获取空key,执行委托
            _cache.Set("A", new MyCacheItem { Value = 42 });
            //再次获取key，不为空
            _cache.GetOrDefault<MyCacheItem>("A").ShouldNotBe(null);
            _cache.GetOrDefault<MyCacheItem>("A").Value.ShouldBe(42);

            //第一次获取空key，会执行第二个委托参数
            _cache.Get("B", key => { return new MyCacheItem(43); }).Value.ShouldBe(43);
            //已经有值后，不会再执行委托
            _cache.Get("B", key => new MyCacheItem { Value = 44 }).Value.ShouldBe(43); //不调用工厂，所以值不变

        }
    }
    [Serializable]
    public class MyCacheItem
    {
        public string Name { get; set; }
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
