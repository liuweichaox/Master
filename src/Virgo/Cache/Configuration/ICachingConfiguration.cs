using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Cache.Configuration
{
    /// <summary>
    /// 用于配置缓存系统
    /// </summary>
    public interface ICachingConfiguration
    {

        /// <summary>
        /// 所有已注册配置程序的列表。
        /// </summary>
        IReadOnlyList<ICacheConfigurator> Configurators { get; }

        /// <summary>
        /// 用于配置所有缓存。
        /// </summary>
        /// <param name="initAction">
        /// 配置缓存的操作
        /// 创建后，每个缓存都会调用此操作。
        /// </param>
        void ConfigureAll(Action<ICache> initAction);

        /// <summary>
        /// 用于配置特定缓存。 
        /// </summary>
        /// <param name="cacheName">Cache name</param>
        /// <param name="initAction">
        /// 配置缓存的操作
        /// 在创建缓存后立即调用此操作
        /// </param>
        void Configure(string cacheName, Action<ICache> initAction);
    }
}
