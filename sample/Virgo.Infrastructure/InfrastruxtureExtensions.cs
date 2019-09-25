using Microsoft.Extensions.DependencyInjection;
using Virgo.DependencyInjection;

namespace Virgo.Infrastructure.Sample
{
    public static class InfrastruxtureExtensions
    {
        public static IServiceCollection UseInfrastructure(this IServiceCollection builder)
        {
            var assembly = typeof(InfrastruxtureExtensions).Assembly;
            builder.AddAssembly(assembly);
            return builder;
        }
    }
}
