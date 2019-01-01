using System.Reflection;
using System.Reflection.Emit;
using Autofac.Extras.IocManager;

namespace Virgo.Domain
{
    public class DomainModule : IModule
    {
        public void Register(IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
        }
    }
}