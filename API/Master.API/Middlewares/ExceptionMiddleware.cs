using Master.Application.Exceptions;
using Master.Infrastructure.API;

namespace Master.API.Middlewares;

/// <summary>
///     ExceptionMiddleware
/// </summary>
public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    ///     ExceptionMiddleware
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    ///     InvokeAsync
    /// </summary>
    /// <param name="context"></param>
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
            case BusinessException exception:
                {
                    var result = APIResult.ErrorResult(exception.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                    break;
                }
            case BusinessRuleValidationException exception:
                {
                    var result = APIResult.ErrorResult(exception.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                    break;
                }
            default:
                {
                    var environment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
                    var message = environment.IsDevelopment() ? ex.Message : "服务器内部错误，无法完成请求";
                    var result = APIResult.ErrorResult(message, APIResultCode.InternalServerError);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
                    break;
                }
        }
    }
}