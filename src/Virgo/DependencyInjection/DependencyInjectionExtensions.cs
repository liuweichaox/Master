using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Virgo.Extensions;

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
        public static ContainerBuilder RegisterAssembly(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
            {
                throw new Exception("assemblies cannot be empty.");
            }
            foreach (var assembly in assemblies)
            {
                RegisterDependenciesByAssembly<ISingletonDependency>(builder, assembly);
                RegisterDependenciesByAssembly<ITransientDependency>(builder, assembly);
                RegisterDependenciesByAssembly<ILifetimeScopeDependency>(builder, assembly);
            }
            return builder;
        }

        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void RegisterDependenciesByAssembly<TServiceLifetime>(ContainerBuilder builder, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => typeof(TServiceLifetime).GetTypeInfo().IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsSealed).ToList();
            foreach (var type in types)
            {
                var itype = type.GetTypeInfo().GetInterfaces().FirstOrDefault(x => x.Name.ToUpper().Contains(type.Name.ToUpper()));
                if (!itype.IsNull())
                {
                    if (typeof(TServiceLifetime) == typeof(ISingletonDependency))
                    {
                        builder.RegisterType(type).As(itype).SingleInstance();
                    }
                    if (typeof(TServiceLifetime) == typeof(ITransientDependency))
                    {
                        builder.RegisterType(type).As(itype).InstancePerDependency();
                    }
                    if (typeof(TServiceLifetime) == typeof(ILifetimeScopeDependency))
                    {
                        builder.RegisterType(type).As(itype).InstancePerLifetimeScope();
                    }
                }
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

        /// <summary>
        /// 注册IocManager
        /// 在ConfigureServices方法最后一行使用
        /// </summary>
        /// <param name="services"></param>
        public static void AddIocManager(this IServiceCollection services)
        {
            services.AddSingleton<IIocManager, IocManager>(provide =>
            {
                IocManager.Instance.ServiceProvider = provide;
                return IocManager.Instance;
            });
        }
    }
}
