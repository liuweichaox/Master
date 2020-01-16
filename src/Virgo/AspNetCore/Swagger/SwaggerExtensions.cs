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

namespace Virgo.AspNetCore
{

    /// <summary>
    /// Swagger拓展类
    /// </summary>
    /// <remarks>Swashbuckle.AspNetCore</remarks>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 添加Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="xmlPaths"></param>
        public static void AddSwaggerStep(this IServiceCollection services, List<string> xmlPaths)
        {
            // 配置 Swagger 文档信息
            services.AddSwaggerGen(options =>
            {
                // 根据 API 版本信息生成 API 文档
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
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
                //options.DescribeAllParametersInCamelCase();

                // 取消 API 文档需要输入版本信息
                options.OperationFilter<RemoveVersionFromParameter>();

                // 获取接口文档XML描述信息
                foreach (var xmlFile in xmlPaths)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }
        /// <summary>
        /// 创建Api版本信息
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"Virgo v{description.ApiVersion} 接口文档",
                Version = description.ApiVersion.ToString(),
                Description = "多版本管理（点右上角版本切换）<br/>",
                Contact = new OpenApiContact()
                {
                    Name = "Virgo",
                    Email = "893703963@qq.com"
                }
            };
            if (description.IsDeprecated)
            {
                info.Description += "<br/><b>TopBrids</b>";
            }

            return info;
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
