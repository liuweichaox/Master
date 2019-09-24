using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Virgo.AspNetCore;
using Virgo.Infrastructure.Sample;
using Virgo.Web.Sample.Filters;
using Virgo.Web.Sample.Middlewares;
using Virgo.DependencyInjection;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // 此lambda确定对于给定请求是否需要用户同意非必要cookie
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AuditActionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            services.UseVirgo().UseInfrastructure();

            services.AddSingleton<IIocManager, IocManager>(provide =>
            {
                IocManager.Instance.ServiceProvider = provide;
                return IocManager.Instance;
            });
            //#region Autofac接管Ioc
            //var builder = IocBuilder.New.UseAutofacContainerBuilder().RegisterIocManager();

            ////注入的先后顺序很重要
            //builder.UseVirgo().UseInfrastructure().UseUnitOfWorkInterceptor(); 
            //builder.RegisterServices(r =>
            //{
            //    r.BeforeRegistrationCompleted += ((sender, args) =>
            //    {
            //        args.ContainerBuilder.Populate(services);
            //    });

            //    r.UseBuilder(b =>
            //    { 
            //        b.RegisterCallback(x =>
            //        {
            //            x.Registered += (sender, e) =>
            //             {
            //                 Type implType = e.ComponentRegistration.Activator.LimitType;
            //                 if (typeof(ITransientDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ITransientDependency或者间接实现了ITransientDependency
            //                 {
            //                     e.ComponentRegistration.InterceptedBy<AopInterceptor>();
            //                 };
            //                 if (typeof(ILifetimeScopeDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ILifetimeScopeDependency或者间接实现了ILifetimeScopeDependency
            //                 {
            //                     e.ComponentRegistration.InterceptedBy<AopInterceptor>();
            //                 };
            //                 if (typeof(ISingletonDependency).IsAssignableFrom(implType) && implType != typeof(AopInterceptor))//如果继承了ISingletonDependency或者间接实现了ISingletonDependency
            //                 {
            //                     e.ComponentRegistration.InterceptedBy<AopInterceptor>();
            //                 };
            //             };
            //        });
            //    });

            //    r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //});
            //#endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseStaticHttpContext();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseWebSockets();
            app.Map("/ws", builder =>
            {
                app.UseChatWebSocketMiddleware();
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
