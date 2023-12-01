using Microsoft.AspNetCore.Mvc;
using Master.Infrastructure.Api;

namespace Master.API;

/// <summary>
///   Base controller for all controllers in the application.
/// </summary>
public class AppController : ControllerBase
{
    /// <summary>
    ///  Returns a success response with data.
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Success<T>(T data)
    {
        return Ok(ApiResult<T>.SuccessResult(data));
    }

    /// <summary>
    /// Returns a success response with data and message.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Success<T>(T data, string message)
    {
        return Ok(ApiResult<T>.SuccessResult(data,message));
    }

    /// <summary>
    /// Returns a error response with message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected IActionResult Error(string message)
    {
        return Ok(ApiResult.ErrorResult(message));
    }

    /// <summary>
    /// Returns a error response with message and code.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    protected IActionResult Error(string message, ApiResultCode code)
    {
        return Ok(ApiResult.ErrorResult(message, code));
    }
}