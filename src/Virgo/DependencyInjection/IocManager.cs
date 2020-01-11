using Autofac;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// <see cref="IIocManager"/>管理器实现类
    /// </summary>
    public class IocManager : IIocManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static IocManager()
        {
            Instance = new IocManager();
        }
        /// <summary>
        /// <see cref="IIocManager"/>管理器实例
        /// </summary>
        public static IocManager Instance { get; private set; }
        /// <summary>
        /// Autofac容器
        /// </summary>
        public ILifetimeScope AutofacContainer { get; set; }
        /// <summary>
        /// 获取指定类型实例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService GetInstance<TService>()
        {
            return AutofacContainer.Resolve<TService>();
        }
    }
}
