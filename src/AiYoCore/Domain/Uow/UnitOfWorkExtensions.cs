using System;
using Autofac;
using System.Linq;
using Autofac.Core.Resolving.Pipeline;
using Virgo.DependencyInjection;

namespace Virgo.Domain.Uow
{

    /// <summary>
    /// 注册工作单元拦截器拓展
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// 注册工作单元拦截器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterUnitOfWorkInterceptor(this ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
