using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Master.API.Extensions
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "示例 CQRS API",
                    Version = "v1",
                    Description = "使用原始 SQL 和 DDD 的示例 .NET Core REST API CQRS 实现。",
                });

                options.AddAuthenticationHeader();
                
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                options.IncludeXmlComments(commentsFile);
            });

            return services;
        }

        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample CQRS API V1");
            });

            return app;
        }
        
        /// <summary>
        /// 为Swagger增加Authentication报文头
        /// </summary>
        /// <param name="c"></param>
        internal static void AddAuthenticationHeader(this SwaggerGenOptions c)
        {
            c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
            {
                Description =  "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Authorization"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }
    }
}