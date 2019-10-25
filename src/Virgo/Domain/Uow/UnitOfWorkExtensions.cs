using Autofac;
using System.Linq;
using System.Reflection;
using Virgo.DependencyInjection;
using System;
using Castle.DynamicProxy;

namespace Virgo.Domain.Uow
{

    /// <summary>
    /// 注册工作单元拦截器拓展
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// 注册工作单元拦截器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterUnitOfWorkInterceptor(this ContainerBuilder builder)
        {
            builder.RegisterCallback(x => x.Registered += (sender, e) =>
            {
                if (e.ComponentRegistration.Activator.LimitType.GetMethods().Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
                {
                    e.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
                }
            });
            return builder;
        }
    }
}
