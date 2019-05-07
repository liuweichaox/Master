using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.IocManager;
using Autofac.Extras.IocManager.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Virgo.Domain.Uow;
using Virgo.Infrastructure.Sample;
using Virgo.Web.Sample.Aop;
using Virgo.Web.Sample.Middlewares;

namespace Virgo.Web.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // 此lambda确定对于给定请求是否需要用户同意非必要cookie
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #region Autofac接管Ioc
            var builder = IocBuilder.New.UseAutofacContainerBuilder().RegisterIocManager();

            //注入的先后顺序很重要
            builder.UseVirgo().UseInfrastructure().UseUnitOfWorkInterceptor();

            builder.RegisterServices(r =>
            {
                r.BeforeRegistrationCompleted += ((sender, args) =>
                {
                    args.ContainerBuilder.Populate(services);
                });

                r.UseBuilder(b =>
                {
                    b.RegisterCallback(x =>
                    {
                        x.Registered += (sender, e) =>
                         {
                             Type implType = e.ComponentRegistration.Activator.LimitType;
                             if (typeof(ITransientDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ITransientDependency或者间接实现了ITransientDependency
                             {
                                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                             };
                             if (typeof(ILifetimeScopeDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ILifetimeScopeDependency或者间接实现了ILifetimeScopeDependency
                             {
                                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                             };
                             if (typeof(ISingletonDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ISingletonDependency或者间接实现了ISingletonDependency
                             {
                                 e.ComponentRegistration.InterceptedBy<AopInterceptor>();
                             };
                         };
                    });
                });

                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });



            var resolver = builder.CreateResolver().UseIocManager();
            return new AutofacServiceProvider(resolver.Container);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseWebSockets();
            app.Map("/ws", builder =>
            {
                app.UseChatWebSocketMiddleware();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
