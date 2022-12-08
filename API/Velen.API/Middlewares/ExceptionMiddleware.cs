using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Velen.Application.Exceptions;
using Velen.Domain.SeedWork;
using Velen.Infrastructure.Api;

namespace Velen.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(new EventId(ex.HResult),
            ex,
            ex.Message);
        context.Response.ContentType = "application/json;charset=utf-8";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null, 
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        switch (ex.InnerException)
        {
            case InvalidCommandException exception:
            {
                var result = ApiResult.ErrorResult(exception.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                break;
            }
            case BusinessRuleValidationException exception:
            {
                var result = ApiResult.ErrorResult(exception.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                break;
            }
            default:
            {
                var result = ApiResult.ErrorResult("服务器内部错误，无法完成请求", ApiResultCode.InternalServerError);
                await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                break;
            }
        }
    }
}