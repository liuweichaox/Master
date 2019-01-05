using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Cache;
using Virgo.Cache.Configuration;

namespace Virgo.Redis
{
    /// <summary>
    /// 用于创建<see cref =“RedisCache”/>实例
    /// </summary>
    public class RedisCacheManager : CacheManagerBase, ISingletonDependency
    {
        /// <summary>
        /// 初始化<see cref =“AbpRedisCacheManager”/>类的新实例
        /// </summary>
        public RedisCacheManager(ICachingConfiguration configuration)
            : base(configuration)
        {

        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Instance.Resolve<RedisCache>(new { name });
        }
    }
}
