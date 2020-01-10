using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Virgo.Application;
using Virgo.DependencyInjection;
using Virgo.Infrastructure;
using Virgo.UserInterface.Interceptors;
using Virgo.UserInterface.Middlewares;
using Virgo.UserInterface.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Virgo.AspNetCore;
using Virgo.AspNetCore.Job;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace Virgo.UserInterface
{
    /// <summary>
    /// Startup启动类
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
            services.AddHostedService<JobService>();
        }

        /// <summary>
        /// Autofac 容器配置
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

    /// <summary>
    /// Job
    /// </summary>
    public class JobService : VirgoBackgroundService
    {
        private readonly ILogger<JobService> _logger;
        public JobService(ILogger<JobService> logger)
        {
            Interval = TimeSpan.FromSeconds(1);
            _logger = logger;
        }


        int ExcuteCount = 0;
        string str = "遇到什么困难也不要怕|微笑着面对他|消除恐惧最好的办法就是战胜恐惧|坚持，才是胜利!|加油，奥里给!";
        public override void DoWork(object state)
        {
            Debug.WriteLine($" for {ExcuteCount.ToString()} excuteing,{DateTime.Now} \r\n {GetStr()}");
            _logger.LogInformation($" for {ExcuteCount.ToString()} excuteing,{DateTime.Now} \r\n {GetStr()}");
            ExcuteCount++;
        }
        public string GetStr()
        {
            return str.Split('|')[new Random().Next(str.Length - 1)];
        }
    }
}