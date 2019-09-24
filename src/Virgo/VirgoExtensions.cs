using Microsoft.Extensions.DependencyInjection;
using Virgo.DependencyInjection;

namespace Virgo
{
    /// <summary>
    /// Virgo拓展类
    /// </summary>
    public static class VirgoExtensions
    {
        /// <summary>
        /// 注入Virgo
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection UseVirgo(this IServiceCollection services)
        {
            var assembly = typeof(VirgoExtensions).Assembly;
            services.RegisterAssemblyByConvention(assembly);
            return services;
        }
    }
}
