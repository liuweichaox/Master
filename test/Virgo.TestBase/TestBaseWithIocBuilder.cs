using Autofac.Extras.IocManager;
using System;
using Virgo.DependencyInjection;
using IIocManager = Autofac.Extras.IocManager.IIocManager;
using IocManager = Autofac.Extras.IocManager.IocManager;

namespace Virgo.TestBase
{
    /// <summary>
    /// ʹ��<see cref="Autofac.Extras.IocManager.IocBuilder"/>���Ի���
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
