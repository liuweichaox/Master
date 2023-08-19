using Microsoft.AspNetCore.Mvc;
using Velen.Infrastructure.Api;

namespace Velen.API;

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