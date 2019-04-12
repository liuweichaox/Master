using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.IocManager;
using Autofac.Extras.IocManager.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Virgo.AspNetCore.Controllers;
using Virgo.AspNetCore.Models;
using Virgo.Domain.Uow;
using Virgo.Infrastructure;
using Virgo.Infrastructure.Domain.Uow;

namespace Virgo.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment ev)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用。 使用此方法将服务添加到容器。
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // 此lambda确定对于给定请求是否需要用户同意非必要cookie
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #region Autofac接管Ioc
            var builder = IocBuilder.New.UseAutofacContainerBuilder().RegisterIocManager();

            //注入的先后顺序很重要
            //builder.UseVirgo().UseInfrastructure().UseUnitOfWorkInterceptor();

            builder.RegisterServices(r =>
            {
                r.BeforeRegistrationCompleted += ((sender, args) =>
                {
                    args.ContainerBuilder.Populate(services);
                });

                //r.UseBuilder(b =>
                //{
                //    b.RegisterCallback(x =>
                //    {
                //        x.Registered += (sender, e) =>
                //         {
                //             Type implType = e.ComponentRegistration.Activator.LimitType;
                //             if (typeof(ITransientDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ITransientDependency或者间接实现了ITransientDependency
                //             {
                //                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                //             };
                //             if (typeof(ILifetimeScopeDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ILifetimeScopeDependency或者间接实现了ILifetimeScopeDependency
                //             {
                //                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                //             };
                //             if (typeof(ISingletonDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ISingletonDependency或者间接实现了ISingletonDependency
                //             {
                //                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                //             };
                //         };
                //    });
                //});

                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });



            var resolver = builder.CreateResolver().UseIocManager();
            return new AutofacServiceProvider(resolver.Container);
            #endregion
        }

        // 此方法由运行时调用。 使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            };

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
