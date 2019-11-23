using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Virgo.Cache.Configuration;
using Virgo.DependencyInjection;

namespace Virgo.Cache.Memory
{
    /// <summary>
    /// 实现<see cref ="ICacheManager"/>以使用MemoryCache
    /// </summary>
    public class VirgoMemoryCacheManager : CacheManagerBase
    {

        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public VirgoMemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            Logger = NullLogger.Instance;
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new VirgoMemoryCache(name)
            {
                Logger = Logger
            };
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
