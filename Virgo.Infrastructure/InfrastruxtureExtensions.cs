using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Infrastructure
{
    public static class InfrastruxtureExtensions
    {
        public static IServiceCollection UseInfrastructure(this IServiceCollection services)
        {
            var assembly = typeof(InfrastruxtureExtensions).Assembly;
            services.AddAssembly(assembly);
            return services;
        }
    }
}
