using Autofac.Extras.IocManager;
using System;
using Xunit;

namespace Virgo.TestBase
{
    /// <summary>
    /// 使用<see cref="Autofac.Extras.IocManager.IocBuilder"/>测试基类
    /// </summary>
    public class TestBaseWithIocBuilder
    {
        protected IIocBuilder IocBuilder;
        protected IIocManager LocalIocManager;

        protected TestBaseWithIocBuilder()
        {
            LocalIocManager = new IocManager();
            IocBuilder = Autofac.Extras.IocManager.IocBuilder.New
                               .UseAutofacContainerBuilder()
                               .RegisterIocManager(LocalIocManager);
        }

        protected IResolver Building(Action<IIocBuilder> builderAction)
        {
            builderAction(IocBuilder);
            return IocBuilder.CreateResolver().UseIocManager(LocalIocManager);
        }

        protected T The<T>()
        {
            return LocalIocManager.Resolve<T>();
        }
    }
}
