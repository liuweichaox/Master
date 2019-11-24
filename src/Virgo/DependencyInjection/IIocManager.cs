using Autofac;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// 管理器接口
    /// </summary>
    public interface IIocManager
    {
        ILifetimeScope AutofacContainer { get; set; }
        TService GetInstance<TService>();
    }
}
