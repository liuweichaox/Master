using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Virgo.Infrastructure.Sample
{
    public static class InfrastruxtureExtensions
    {
        public static IIocBuilder UseInfrastructure(this IIocBuilder builder)
        {
            var assembly = typeof(InfrastruxtureExtensions).Assembly;
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(assembly));
            return builder;
        }
    }
}
