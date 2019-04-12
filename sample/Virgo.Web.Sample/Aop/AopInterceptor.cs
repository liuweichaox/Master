using Autofac.Extras.IocManager;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virgo.Web.Sample.Aop
{
    public class AopInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method+"执行之前。。。");
            invocation.Proceed();
            Console.WriteLine(invocation.Method + "执行之后。。。");
        }
    }
}
