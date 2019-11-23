using Autofac;
using Virgo.DependencyInjection;

namespace Virgo.Application
{
    /// <summary>
    /// Application拓展类
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 注入Application容器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterApplication(this ContainerBuilder builder)
        {
            var assembly = typeof(ApplicationExtensions).Assembly;
            builder.RegisterAssembly(assembly);
            return builder;
        }
    }
}
