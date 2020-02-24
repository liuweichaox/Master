using Microsoft.Extensions.DependencyInjection;
using Virgo.Cache;

namespace Virgo.Redis
{
    /// <summary>
    /// <see cref ="ICachingConfiguration"/>的扩展方法
    /// </summary>
    public static class RedisCacheConfigurationExtensions
    {
        /// <summary>
        /// 配置缓存以将Redis用作缓存服务器
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<ICache, RedisCache>();
            services.AddSingleton<IRedisCacheProvider, RedisCacheProvider>();
        }
    }
}
