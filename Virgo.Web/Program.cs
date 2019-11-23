using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Virgo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureAppConfiguration((hostingContext, app) =>
            {
                app.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Configs/myjson.json", true, true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        private static void CallBack_Registered(object sender, Autofac.Core.ComponentRegisteredEventArgs e)
        {
            var implType = e.ComponentRegistration.Activator.LimitType;
            System.Diagnostics.Debug.WriteLine(Environment.NewLine + implType.Name + Environment.NewLine);
        }
    }
}
