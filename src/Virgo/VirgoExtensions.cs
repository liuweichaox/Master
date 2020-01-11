using Autofac;
using Virgo.DependencyInjection;

namespace Virgo
{
    /// <summary>
    /// Virgo拓展类
    /// </summary>
    public static class VirgoExtensions
    {
        /// <summary>
        /// 注入Virgo
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder UseVirgo(this ContainerBuilder builder)
        {
            var assembly = typeof(VirgoExtensions).Assembly;
            builder.RegisterAssembly(assembly);
            return builder;
        }
    }
}
