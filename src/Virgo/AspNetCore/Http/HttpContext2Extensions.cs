using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Virgo.AspNetCore.Http
{
    /// <summary>
    /// 依赖注入<see cref="ServiceCollection"/>容器扩展方法
    /// </summary>
    public static class HttpContext2Extensions
    {
        /// <summary>
        /// 注入HttpContext静态对象，方便在任意地方获取HttpContext，app.UseStaticHttpContext();
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext2.Configure(httpContextAccessor);
            return app;
        }
    }
}
