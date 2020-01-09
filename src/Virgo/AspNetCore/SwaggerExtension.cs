using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Virgo.AspNetCore
{

    /// <summary>
    /// Swagger拓展类
    /// </summary>
    /// <remarks>Swashbuckle.AspNetCore</remarks>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 添加Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        public static void AddSwagger(this IServiceCollection services, Type type)
        {
            // 配置 Swagger 文档信息
            services.AddSwaggerGen(options =>
            {
                // 根据 API 版本信息生成 API 文档
                //
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                    {
                        Title = $"Virgo v{description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "我来我见我征服",
                        Contact = new OpenApiContact()
                        {
                            Name = "Virgo",
                            Email = "893703963@qq.com"
                        }
                    }
                   );
                }

                // 在 Swagger 文档显示的 API 地址中将版本信息参数替换为实际的版本号
                options.DocInclusionPredicate((version, apiDescription) =>
                {
                    if (!version.Equals(apiDescription.GroupName))
                        return false;

                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", apiDescription.GroupName)); apiDescription.RelativePath = string.Join("/", values);
                    return true;
                });

                // 参数使用驼峰命名方式
                options.DescribeAllParametersInCamelCase();

                // 取消 API 文档需要输入版本信息
                options.OperationFilter<RemoveVersionFromParameter>();

                // 获取接口文档描述信息
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = type.GetTypeInfo().Assembly.GetName().Name + ".xml";
                var xmlPath = Path.Combine(basePath, fileName);
                options.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// 添加Swagger中间件和SwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerWithUI(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwagger().UseSwaggerUI((o) =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
