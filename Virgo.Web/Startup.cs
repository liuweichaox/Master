using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Virgo.Application.IService;
using Virgo.Application.Service;
using Virgo.AspNetCore;
using Virgo.DependencyInjection;
using Virgo.Web.Filters;
using Virgo.Web.Middlewares;
using Virgo.Web.Tests;

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


            //services.UseVirgo().UseInfrastructure().UseApplication();

            //services.AddAssembly(Assembly.GetExecutingAssembly());

            services.AddIocManager();

            services.AddOptions();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterCallback(x => x.Registered += X_Registered);

            //builder.RegisterUnitOfWorkInterceptor();

            //ע����������ʱ�����õĻص���

            //builder.RegisterInterceptorBy<CustomInterceptor>(typeof(ITransientDependency));
        }

        private void X_Registered(object sender, Autofac.Core.ComponentRegisteredEventArgs e)
        {
            //��ȡ���ע��.��ȡ���ڴ���ʵ���ļ������� ��ȡ��֪���ʵ����ת�������������
            var implType = e.ComponentRegistration.Activator.LimitType;
            System.Diagnostics.Debug.WriteLine(Environment.NewLine + implType.Name + Environment.NewLine);
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