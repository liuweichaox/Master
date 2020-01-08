using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.TestBase
{
    /// <summary>
    /// 使用<see cref="IServiceCollection"/>的测试基类
    /// </summary>
    public class TestBaseWithServiceCollection
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        protected IServiceCollection Services;

        /// <summary>
        /// 服务提供程序
        /// </summary>
        protected IServiceProvider ServiceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TestBaseWithServiceCollection()
        {
            Services = new ServiceCollection();
        }

        /// <summary>
        /// 构建服务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IServiceProvider Building(Action<IServiceCollection> action)
        {
            action(Services);
            ServiceProvider = Services.BuildServiceProvider();
            return ServiceProvider;
        }

        /// <summary>
        /// 获取指定类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T The<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
