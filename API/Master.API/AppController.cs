using Microsoft.AspNetCore.Mvc;
using Master.Infrastructure.Api;

namespace Master.API;

public class AppController : ControllerBase
{
    protected IActionResult Success<T>(T data)
    {
        return Ok(ApiResult<T>.SuccessResult(data));
    }

    public IActionResult Success<T>(T data, string message)
    {
        return Ok(ApiResult<T>.SuccessResult(data,message));
    }

    protected IActionResult Error(string message)
    {
        return Ok(ApiResult.ErrorResult(message));
    }

    protected IActionResult Error(string message, ApiResultCode code)
    {
        return Ok(ApiResult.ErrorResult(message, code));
    }
}