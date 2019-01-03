using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.IocManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Virgo.Presentation.Models;

namespace Virgo.Presentation
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
                //修改为false状态
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region ConfigureServices 修改
            services.AddCors();
            //services.AddCors(_=>_.AddPolicy("Any",optins=>optins.AllowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddControllersAsServices();

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = DateTime.Now - origin;
            var s= Math.Floor(diff.TotalSeconds);

            //ITransientDependency
            //ISingletonDependency
            //ILifetimeScopeDependency 
            IRootResolver resolver = IocBuilder.New
                .UseAutofacContainerBuilder()
                //.RegisterModule<PresentationModule>()//可以用模块注入
                .UsePresentationModule()//也可以用拓展方法注入
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                    r.BeforeRegistrationCompleted += ((sender, args) =>
                    {
                        args.ContainerBuilder.Populate(services);
                        args.ContainerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
                    });
                })
                .RegisterIocManager()
                .CreateResolver()
                .UseIocManager();
            return new AutofacServiceProvider(resolver.Container);
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
            }

            #region Configure修改
            app.UseCors(otions => otions.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            //app.UseCors("Any");
            app.UseApiVersioning();
            #endregion 

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
