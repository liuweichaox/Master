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
        protected readonly IIocManager IocManager;

        protected readonly ICachingConfiguration Configuration;

        protected readonly ConcurrentDictionary<string, ICache> Caches;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        protected CacheManagerBase(IIocManager iocManager, ICachingConfiguration configuration)
        {
            IocManager = iocManager;
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

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

        public virtual void Dispose()
        {
            DisposeCaches();
            Caches.Clear();
        }

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
