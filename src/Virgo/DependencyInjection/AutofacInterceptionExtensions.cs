using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// Autofac拦截器拓展
    /// </summary>
    public static class AutofacInterceptionExtensions
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> EnableInterception<TService, TInterceptor>(
            this IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
            where TInterceptor : IInterceptor
        {
            return EnableInterception(registration, new Type[] { typeof(TService) }, new Type[] { typeof(TInterceptor) });
        }

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> EnableInterception<TService>(
            this IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration,
            Type[] interfacesToIntercept,
            Type[] interceptors
        )
        {
            return registration.EnableClassInterceptors(ProxyGenerationOptions.Default, interfacesToIntercept).InterceptedBy(interceptors);
        }

        public static void InterceptedBy<TInterceptor>(this IComponentRegistration registration) where TInterceptor : IInterceptor
        {
            InterceptedBy<TInterceptor>(registration, false);
        }

        public static void InterceptedBy<TInterceptor>(this IComponentRegistration registration, bool interceptAdditionalInterfaces) where TInterceptor : IInterceptor
        {
            InterceptedBy(registration, interceptAdditionalInterfaces, typeof(TInterceptor));
        }

        public static void InterceptedBy(this IComponentRegistration registration, params Type[] interceptorTypes)
        {
            InterceptedBy(registration, false, interceptorTypes);
        }

        public static void InterceptedBy(this IComponentRegistration registration, bool interceptAddtionalInterfaces, params Type[] interceptorTypes)
        {
            registration.Activating += (sender, e) => { ApplyInterception(interceptorTypes, e, interceptAddtionalInterfaces); };
        }

        private static void ApplyInterception(Type[] interceptorTypes, IActivatingEventArgs<object> e, bool interceptAdditionalInterfaces)
        {
            Type type = e.Instance.GetType();

            if (e.Component.Services.OfType<IServiceWithType>().Any(swt => !swt.ServiceType.GetTypeInfo().IsVisible) || type.Namespace == "Castle.Proxies")
            {
                return;
            }

            Type[] proxiedInterfaces = type.GetInterfaces().Where(i => i.GetTypeInfo().IsVisible).ToArray();
            if (!proxiedInterfaces.Any())
            {
                return;
            }

            Type theInterface = proxiedInterfaces.First();
            Type[] interfaces = proxiedInterfaces.Skip(1).ToArray();

            IList<IInterceptor> interceptorInstances = new List<IInterceptor>();
            foreach (Type interceptorType in interceptorTypes)
            {
                interceptorInstances.Add((IInterceptor)e.Context.Resolve(interceptorType));
            }

            if (interceptorInstances.Count > 0)
            {
                IInterceptor[] interceptors = interceptorInstances.ToArray();

                object interceptedInstance = interceptAdditionalInterfaces
                    ? generator.CreateInterfaceProxyWithTargetInterface(theInterface, interfaces, e.Instance, interceptors)
                    : generator.CreateInterfaceProxyWithTargetInterface(theInterface, e.Instance, interceptors);

                e.ReplaceInstance(interceptedInstance);
            }
        }

        /// <summary>
        /// 注册拦截器
        /// </summary>
        /// <typeparam name="TInterceptor"></typeparam>
        /// <param name="builder"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterInterceptorBy<TInterceptor>(this ContainerBuilder builder) where TInterceptor : IInterceptor
        {
            builder.RegisterCallback(x => x.Registered += (sender, e) =>
            {

                var implType = e.ComponentRegistration.Activator.LimitType;
                System.Diagnostics.Debug.WriteLine("Register   implType " + implType.Name);
            if (typeof(ILifetime).IsAssignableFrom(implType) && !typeof(IInterceptor).IsAssignableFrom(implType))
                {
                    System.Diagnostics.Debug.WriteLine("RegisterInterceptorBy Register  " + implType.Name);
                    e.ComponentRegistration.InterceptedBy<TInterceptor>();
                };
            });
            return builder;
        }
    }
}
