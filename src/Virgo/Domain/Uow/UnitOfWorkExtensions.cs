using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Virgo.Domain.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection UseUnitOfWorkInterceptor(this IServiceCollection services)
        {
            //services.RegisterServices(r =>
            //{
            //    r.UseBuilder(b =>
            //    {
            //        b.RegisterCallback(x => x.Registered += (sender, e) =>
            //            {
            //                if (e.ComponentRegistration.Activator.LimitType.GetMethods().Any(m => m.IsDefined(typeof(UnitOfWorkAttribute), true)))
            //                {
            //                    e.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
            //                }
            //            });
            //    });
            //});
            return services;
        }
    }
}
