using Autofac;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Virgo.Application;
using Virgo.AspNetCore;
using Virgo.DependencyInjection;
using Virgo.Infrastructure;
using Virgo.Web.Filters;
using Virgo.Web.Interceptors;
using Virgo.Web.Middlewares;

namespace Virgo.Web
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
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddControllersWithViews(options => { options.Filters.Add<AuditActionFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddOptions();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInterceptorBy<CustomInterceptor>();
            builder.RegisterType<CustomInterceptor>().As<IInterceptor>();
            //builder.RegisterInfrastructure().RegisterApplication();
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
            }

            app.UseStaticHttpContext();

            app.UseWebSockets();
            app.Map("/ws", builder => { app.UseChatWebSocketMiddleware(); });
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}