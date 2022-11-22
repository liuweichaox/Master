using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
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
        /// <param name="builder"></param>
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
        /// <param name="builder"></param>
        /// <param name="assembly">The assembly.</param>
        private static void RegisterDependenciesByAssembly<TServiceLifetime>(ContainerBuilder builder, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => typeof(TServiceLifetime).GetTypeInfo().IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsSealed).ToList();
            foreach (var type in types)
            {
                var itype = type.GetTypeInfo().GetInterfaces().FirstOrDefault(x => x.Name.ToUpper().Contains(type.Name.ToUpper()));
                if (itype.IsNull()) continue;
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

        /// <summary>
        /// 注册IocManager
        /// </summary>
        /// <param name="app"></param>
        public static void UseIocManager(this IApplicationBuilder app)
        {
            IocManager.Instance.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
        }
    }
}
