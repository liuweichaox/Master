using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Virgo.Presentation.Interceptors
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute 
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("Before service call");
                await next(context);
            }
            catch (Exception)
            {
                Console.WriteLine("Service threw an exception!");
                throw;
            }
            finally
            {
                Console.WriteLine("After service call");
            }
        }
    }
}