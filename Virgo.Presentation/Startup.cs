using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Virgo.Application;
using Virgo.DependencyInjection;
using Virgo.Infrastructure;
using Virgo.Presentation.Interceptors;
using Virgo.Presentation.Middlewares;
using Virgo.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Virgo.AspNetCore;

namespace Virgo.Presentation
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置实例
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 此方法由运行时调用。使用此方法将服务添加到容器。
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);

            services.AddApiVersion();

            services.AddSwagger(typeof(Startup));
        }

        /// <summary>
        /// Autofac容器配置
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomInterceptor>();
            builder.RegisterInterceptorBy<CustomInterceptor>();
            builder.RegisterInfrastructure().RegisterApplication();
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseIocManager();

            app.UseApiVersioning();

            app.UseSwaggerWithUI();

            app.UseWebSockets();
            app.Map("/ws", builder => { app.UseChatWebSocketMiddleware(); });
            app.UseStaticFiles();
        }
    }
}