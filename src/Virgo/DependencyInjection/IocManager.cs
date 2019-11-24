using Autofac;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// <see cref="IIocManager"/>管理器实现类
    /// </summary>
    public class IocManager : IIocManager
    {
        static IocManager()
        {
            Instance = new IocManager();
        }
        public static IocManager Instance { get; private set; }
        /// <summary>
        /// Autofac容器
        /// </summary>
        public ILifetimeScope AutofacContainer { get; set; }

        public TService GetInstance<TService>()
        {
            return AutofacContainer.Resolve<TService>();
        }
    }
}
