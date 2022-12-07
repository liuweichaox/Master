using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Velen.API.SeedWork;
using Velen.Application.Configuration.Validation;

namespace Velen.API.Configuration
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json;charset=utf-8";
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = null,//解决后端传到前端全大写
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)//解决后端返回数据中文被编码
                };
                if (ex.InnerException is InvalidCommandException)
                {
                    var result = new InvalidCommandProblemDetails((InvalidCommandException)ex.InnerException);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                }
                else
                {
                    var result = new
                    {
                        Code = 500,
                        Message = "服务器错错误"
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                }
            }

        }
    }
}
