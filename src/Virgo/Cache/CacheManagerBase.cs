using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Virgo.Cache.Configuration;
using Virgo.DependencyInjection;

namespace Virgo.Cache
{
    /// <summary>
    /// 缓存管理器的基类
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager, ISingletonDependency
    {
        /// <summary>
        /// <see cref="IIocManager"/>实例
        /// </summary>
        protected readonly IIocManager IocManager;

        /// <summary>
        /// 配置中心实例
        /// </summary>
        protected readonly ICachingConfiguration Configuration;

        /// <summary>
        /// 缓存字典
        /// </summary>
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocManager"></param>
        /// <param name="configuration"></param>
        protected CacheManagerBase(IIocManager iocManager, ICachingConfiguration configuration)
        {
            IocManager = iocManager;
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }
        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ICache GetCache(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);

                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }

                return cache;
            });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            DisposeCaches();
            Caches.Clear();
        }
        /// <summary>
        /// 释放缓存
        /// </summary>
        protected virtual void DisposeCaches()
        {
            foreach (var cache in Caches)
            {
                cache.Value?.Dispose();
            }
        }

        /// <summary>
        /// 用于创建实际的缓存实现
        /// </summary>
        /// <param name="name">缓存的名称</param>
        /// <returns>缓存对象</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}
