using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Virgo.AspNetCore;
using Virgo.AspNetCore.ApiVersion;
using Virgo.AspNetCore.Swagger;
using Virgo.Redis;
using Virgo.UserInterface.Filters;

namespace Virgo.UserInterface.Extensions
{
    /// <summary>
    /// 启动器
    /// </summary>
    public static class NativeInjectorBootStrapper
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
                "AiYoCore.UserInterface.xml",
                "AiYoCore.Application.xml"
            };
          
            services.AddSwaggerStep(xmlPaths);

            services.AddControllers(options=> 
            {
                options.Filters.Add<AuditActionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;//解决后端传到前端全大写
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//解决后端返回数据中文被编码
            });
        }
    }
}
