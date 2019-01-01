using System.Reflection;
using Autofac.Extras.IocManager;

namespace Virgo.Presentation
{
    public static class PresentationExtensions
    {
        public static IIocBuilder UsePresentationModule(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            return iocBuilder;
        }
    }
}