using System.Reflection;
using Autofac.Extras.IocManager;

namespace Virgo.Domain
{
    public static class DomainExtensions
    {
        public static IIocBuilder UseDomainModule(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            return iocBuilder;
        }
    }
}