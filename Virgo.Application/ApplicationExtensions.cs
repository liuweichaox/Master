using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Infrastructure
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection UseApplication(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationExtensions).Assembly;
            services.AddAssembly(assembly);
            return services;
        }
    }
}
