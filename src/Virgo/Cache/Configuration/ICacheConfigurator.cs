using System;

namespace Virgo.Cache.Configuration
{
    /// <summary>
    /// 已注册的缓存配置程序。
    /// </summary>
    public interface ICacheConfigurator
    {
        /// <summary>
        /// 缓存的名称。
        /// 如果此配置程序配置所有缓存，则它将为null。
        /// </summary>
        string CacheName { get; }

        /// <summary>
        /// 配置动作。 在创建缓存之后调用。
        /// </summary>
        Action<ICache> InitAction { get; }
    }
}
