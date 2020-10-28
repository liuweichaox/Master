using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;
using Virgo.AspNetCore;
using Virgo.Redis;
using Virgo.UserInterface.Filters;

namespace Virgo.UserInterface.Extensions
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class NativeInjectorBootStrapper
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddOptions();

            services.AddApiVersion();

            services.AddRedis();

            var xmlPaths = new List<string>()
            {
                "Virgo.UserInterface.xml",
                "Virgo.Application.xml"
            };
          
            services.AddSwaggerStep(xmlPaths);

            services.AddControllers(options=> 
            {
                options.Filters.Add<AuditActionFilter>();
            }).AddNewtonsoftJson(options =>
            {
               options.SerializerSettings.ContractResolver = new DefaultContractResolver();             
            });
        }
    }
}
