using Microsoft.AspNetCore.Mvc;

namespace Virgo.AspNetCore.ApiResults
{
    /// <summary>
    /// 全局控制器
    /// </summary>
    public class ApplicationController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationController()
        {

        }
        /// <summary>
        /// <see cref="ApiResult{T}(T, ApiStatus, string)"/>返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult<T> ApiResult<T>(T data, ApiStatus status, string message="")
        {
            return new ApiResult<T>(data, status, message);
        }
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResults.ApiResult Success(string message = "")
        {
            return new ApiResults.ApiResult("", ApiStatus.SUCCESS, message);
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult<T> Success<T>(T data, string message = "")
        {
            return new ApiResult<T>(data, ApiStatus.SUCCESS, message);
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult<T> Failed<T>(T data, string message = "")
        {
            return new ApiResult<T>(data, ApiStatus.FAIL, message);
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResults.ApiResult Failed(string message = "")
        {
            return new ApiResults.ApiResult("", ApiStatus.FAIL, message);
        }
    }
}
