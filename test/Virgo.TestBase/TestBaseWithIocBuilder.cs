using Autofac;
using System;
using Virgo.DependencyInjection;

namespace Virgo.TestBase
{
    /// <summary>
    /// 使用<see cref="IIocManager"/>的测试基类
    /// </summary>
    public class TestBaseWithIocBuilder
    {
        /// <summary>
        /// Autofac容器
        /// </summary>
        protected ContainerBuilder IocBuilder;
        /// <summary>
        /// <see cref="IIocManager"/>实例
        /// </summary>
        protected IIocManager LocalIocManager;

        /// <summary>
        /// 测试基类构造器
        /// </summary>
        protected TestBaseWithIocBuilder()
        {
            LocalIocManager = new IocManager();
            IocBuilder = new ContainerBuilder();
        }

        /// <summary>
        /// 创建<see cref="ContainerBuilder"/>实例
        /// </summary>
        /// <param name="builderAction"></param>
        /// <returns></returns>
        protected IContainer Building(Action<ContainerBuilder> builderAction)
        {
            IocBuilder.RegisterType<IocManager>().As<IIocManager>().SingleInstance();
            builderAction(IocBuilder);
            var container = IocBuilder.Build();
            LocalIocManager.AutofacContainer = container;
            return container;
        }

        /// <summary>
        /// 获取指定类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T The<T>()
        {
            return LocalIocManager.AutofacContainer.Resolve<T>();
        }
    }
}
