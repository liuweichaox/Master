using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Files;
using Virgo.Net;

namespace Virgo.AspNetCore
{
    /// <summary>
    /// 依赖注入<see cref="ServiceCollection"/>容器扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
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

        /// <summary>
        /// 注入HttpContext静态对象，方便在任意地方获取HttpContext，app.UseStaticHttpContext();
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext2.Configure(httpContextAccessor);
            return app;
        }
    }
}
