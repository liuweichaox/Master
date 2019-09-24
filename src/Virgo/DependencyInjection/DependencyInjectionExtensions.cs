using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// .NET Core 依赖注入拓展
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 注册程序集组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssemblyByConvention(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies?.Any() == false)
            {
                throw new Exception("assemblies cannot be empty.");
            }
            foreach (var assembly in assemblies)
            {
                RegisterDependenciesByAssembly<ISingletonDependency>(services, assembly);
                RegisterDependenciesByAssembly<ITransientDependency>(services, assembly);
                RegisterDependenciesByAssembly<ILifetimeScopeDependency>(services, assembly);
            }
            return services;
        }

        /// <summary>
        ///     Registers the assembly by convention.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void RegisterDependenciesByAssembly<TServiceLifetime>(IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => typeof(TServiceLifetime).GetTypeInfo().IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsSealed).ToList();
            foreach (var type in types)
            {
                var itype = type.GetTypeInfo().GetInterfaces().FirstOrDefault(x=>x.Name.ToUpper().Contains(type.Name.ToUpper()));
                var serviceLifetime = FindServiceLifetime(typeof(TServiceLifetime));
                services.Add(new ServiceDescriptor(itype, type, serviceLifetime));
            }
        }

        private static ServiceLifetime FindServiceLifetime(Type type)
        {
            if (type == typeof(ISingletonDependency))
            {
                return ServiceLifetime.Singleton;
            }
            if (type == typeof(ITransientDependency))
            {
                return ServiceLifetime.Singleton;
            }
            if (type == typeof(ILifetimeScopeDependency))
            {
                return ServiceLifetime.Singleton;
            }

            throw new ArgumentOutOfRangeException($"Provided ServiceLifetime type is invalid. Lifetime:{type.Name}");
        }
    }
}
