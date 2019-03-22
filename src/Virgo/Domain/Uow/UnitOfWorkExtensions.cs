using Castle.Core;
using System.Linq;
using System.Reflection;
using Autofac.Extras.IocManager;
using System;
using Autofac.Extras.IocManager.DynamicProxy;

namespace Virgo.Domain.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static IIocBuilder UseUnitOfWorkInterceptor(this IIocBuilder builder)
        {
            builder.RegisterServices(r =>
            {
                r.UseBuilder(b =>
                {
                    b.RegisterCallback(x => x.Registered += (sender, e) =>
                        {
                            if (e.ComponentRegistration.Activator.LimitType.GetMethods().Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
                            {
                                e.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
                            }
                        });
                });
            });
            return builder;
        }
    }
}
