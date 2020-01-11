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
using Virgo.Extensions;
using Autofac.Extras.DynamicProxy;

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
            services.AddHostedService<BGMJobService>();
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
    /// BGMJobService
    /// </summary>
    public class BGMJobService : VirgoBackgroundService
    {
        private readonly ILogger<BGMJobService> _logger;
        public BGMJobService(ILogger<BGMJobService> logger)
        {
            Interval = TimeSpan.FromSeconds(3);
            _logger = logger;
        }
        int ExcuteCount = 0;
        string str = @"徘徊着的 在路上的 |你要走吗 Via Via |易碎的 骄傲着 |那也曾是我的模样 |沸腾着的 不安着的 |你要去哪 Via Via |谜一样的 沉默着的 |故事你真的在听吗 |我曾经跨过山和大海 |也穿过人山人海 |我曾经拥有着的一切 |转眼都飘散如烟 |我曾经失落失望失掉所有方向 |直到看见平凡才是唯一的答案 |当你仍然 还在幻想 |你的明天 Via Via |她会好吗 还是更烂 |对我而言是另一天 |我曾经毁了我的一切 |只想永远地离开 |我曾经堕入无边黑暗 |想挣扎无法自拔 |我曾经像你像他像那野草野花 |绝望着 也渴望着 |也哭也笑平凡着 |向前走 就这么走 |就算你被给过什么 |向前走 就这么走 |就算你被夺走什么 |向前走 就这么走 |就算你会错过什么 |向前走 就这么走 |就算你会 |我曾经跨过山和大海 |也穿过人山人海 |我曾经拥有着的一切 |转眼都飘散如烟 |我曾经失落失望失掉所有方向 |直到看见平凡才是唯一的答案 |我曾经毁了我的一切 |只想永远地离开 |我曾经堕入无边黑暗 |想挣扎无法自拔 |我曾经像你像他像那野草野花 |绝望着 也渴望着 |也哭也笑平凡着 |我曾经跨过山和大海 |也穿过人山人海 |我曾经问遍整个世界 |从来没得到答案 |我不过像你像他像那野草野花 |冥冥中这是我 唯一要走的路啊 |时间无言 如此这般 |明天已在 Hia Hia |风吹过的 路依然远 |你的故事讲到了哪";
        public override void DoWork(object state)
        {
            var s = $"\r\nFor {ExcuteCount.ToString()} Excuteing……\r\nSay：{GetStr()}\r\n";
            Debug.WriteLine(s);
            _logger.LogInformation(s);
            ExcuteCount++;
        }
        int i = 0;
        public string GetStr()
        {
            if (i > str.Split('|').Length - 1)
            {
                i = 0;
            }
            var s = str.Split('|')[i];
            i++;
            return s;
        }
    }
}