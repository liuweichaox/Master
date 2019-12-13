using System;
using System.Reflection;
using AspectCore.Configuration;
using Autofac;
using Castle.DynamicProxy;
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
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Autofac.Extensions.DependencyInjection;
using Virgo.Application.Interfaces;
using Virgo.Application.Services;
using Virgo.Domain.Interfaces;
using Virgo.Infrastructure.Repository;
using Virgo.Presentation.Extensions;

namespace Virgo.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用。使用此方法将服务添加到容器。
        public void ConfigureServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }

        //Autofac容器配置
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomInterceptor>();
            builder.RegisterInterceptorBy<CustomInterceptor>();
            builder.RegisterInfrastructure().RegisterApplication();
        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseIocManager();

            app.UseWebSockets();
            app.Map("/ws", builder => { app.UseChatWebSocketMiddleware(); });
            app.UseStaticFiles();
        }
    }
}