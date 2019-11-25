using Castle.DynamicProxy;
using Virgo.DependencyInjection;

namespace Virgo.Presentation.Interceptors
{
    public class CustomInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            System.Diagnostics.Debug.WriteLine(invocation.Method + " Proceed Befor");
            invocation.Proceed();
            System.Diagnostics.Debug.WriteLine(invocation.Method + " Proceed After");
        }
    }
}
