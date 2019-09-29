using Microsoft.Extensions.DependencyInjection;
using Virgo.DependencyInjection;

namespace Virgo.Infrastructure.Sample
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
