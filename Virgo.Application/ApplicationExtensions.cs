using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Application
{
    /// <summary>
    /// Application拓展类
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 注入Application容器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationExtensions).Assembly;
            services.AddAssembly(assembly);
            return services;
        }
    }
}
