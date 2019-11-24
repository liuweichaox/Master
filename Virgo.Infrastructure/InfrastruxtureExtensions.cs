using Autofac;
using Virgo.DependencyInjection;
using Virgo.Strings;
using Virgo.Text;

namespace Virgo.Infrastructure
{
    /// <summary>
    /// 基础设施拓展
    /// </summary>
    public static class InfrastruxtureExtensions
    {
        /// <summary>
        /// 引用基础设施服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterInfrastructure(this ContainerBuilder builder)
        {
            var assembly = typeof(InfrastruxtureExtensions).Assembly;
            builder.RegisterAssembly(assembly);
            return builder;
        }
    }
}
