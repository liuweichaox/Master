using Master.Infrastructure.API;
using Microsoft.AspNetCore.Mvc;

namespace Master.API;

/// <summary>
///     Base controller for all controllers in the application.
/// </summary>
public class AppController : ControllerBase
{
    /// <summary>
    ///     Returns a success response with data.
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Success<T>(T data)
    {
        return Ok(APIResult<T>.SuccessResult(data));
    }

    /// <summary>
    ///     Returns a success response with data and message.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Success<T>(T data, string message)
    {
        return Ok(APIResult<T>.SuccessResult(data, message));
    }

    /// <summary>
    ///     Returns a error response with message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected IActionResult Error(string message)
    {
        return Ok(APIResult.ErrorResult(message));
    }

    /// <summary>
    ///     Returns a error response with message and code.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    protected IActionResult Error(string message, APIResultCode code)
    {
        return Ok(APIResult.ErrorResult(message, code));
    }
}