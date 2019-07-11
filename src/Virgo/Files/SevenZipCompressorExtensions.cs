using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Files
{
    /// <summary>
    /// 注入7z压缩<see cref="ServiceCollection"/>容器扩展方法
    /// </summary>
    public static class SevenZipCompressorExtensions
    {
        /// <summary>
        /// 注入7z压缩
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSevenZipCompressor(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.TryAddTransient<ISevenZipCompressor, SevenZipCompressor>();
            return services;
        }
    }
}
