﻿using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Cache;
using Virgo.Cache.Configuration;

namespace Virgo.Redis
{
    /// <summary>
    /// 用于创建<see cref ="RedisCache"/>实例
    /// </summary>
    public class RedisCacheManager : CacheManagerBase
    {
        /// <summary>
        /// 初始化<see cref ="RedisCacheManager"/>类的新实例
        /// </summary>
        public RedisCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {

        }

        protected override ICache CreateCacheImplementation(string name)
        {
            var provider = IocManager.Resolve<IRedisCacheProvider>();
            return new RedisCache(name, provider);
        }
    }
}
