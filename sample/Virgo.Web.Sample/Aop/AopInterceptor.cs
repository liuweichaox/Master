using Castle.DynamicProxy;
using System;
using Virgo.DependencyInjection;

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
