using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Infrastructure
{
    /// <summary>
    /// 基础设施拓展
    /// </summary>
    public static class InfrastruxtureExtensions
    {
        /// <summary>
        /// 引用基础设施服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseInfrastructure(this IServiceCollection services)
        {
            var assembly = typeof(InfrastruxtureExtensions).Assembly;
            services.AddAssembly(assembly);
            return services;
        }
    }
}
