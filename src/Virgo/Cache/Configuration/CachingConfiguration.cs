using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Virgo.DependencyInjection;

namespace Virgo.Cache.Configuration
{
    /// <summary>
    /// <see cref="ICachingConfiguration"/>实现类
    /// </summary>
    public class CachingConfiguration : ICachingConfiguration, ISingletonDependency
    {
        /// <summary>
        /// 缓存配置
        /// </summary>
        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }
        /// <summary>
        /// 缓存配置
        /// </summary>
        private readonly List<ICacheConfigurator> _configurators;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CachingConfiguration()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        /// <summary>
        /// 配置全局
        /// </summary>
        /// <param name="initAction"></param>
        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }
        /// <summary>
        /// 配置某个缓存
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="initAction"></param>
        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}
