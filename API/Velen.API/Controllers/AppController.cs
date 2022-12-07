using Microsoft.AspNetCore.Mvc;
using Velen.Infrastructure.Api;

namespace Velen.API.Controllers;

public class AppController:ControllerBase
{
    protected IActionResult Success<T>(T data)
    {
        return Ok(ApiResult<T>.SuccessResult(data));
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