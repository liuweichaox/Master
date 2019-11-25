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
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddControllers();

            services.AddOptions();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInterceptorBy<CustomInterceptor>();
            builder.RegisterType<CustomInterceptor>();

            builder.RegisterInfrastructure().RegisterApplication();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseWebSockets();
            app.Map("/ws", builder => { app.UseChatWebSocketMiddleware(); });
            app.UseStaticFiles();
        }
    }
}
