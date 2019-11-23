using System;

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
        public IServiceProvider ServiceProvider { get; set; }

        public TService GetInstance<TService>()
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }
    }
}
