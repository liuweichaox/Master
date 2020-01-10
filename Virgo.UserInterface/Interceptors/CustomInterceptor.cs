using Castle.DynamicProxy;
using Virgo.DependencyInjection;

namespace Virgo.UserInterface.Interceptors
{
    public class CustomInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            System.Diagnostics.Debug.WriteLine(invocation.TargetType.FullName + " " + invocation.Method + " Proceed Befor");
            invocation.Proceed();
            System.Diagnostics.Debug.WriteLine(invocation.TargetType.FullName + " " + invocation.Method + " Proceed After");
        }
    }
}
