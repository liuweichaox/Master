namespace Virgo.AspNetCore.ApiResults
{
    /// <summary>
    /// Api返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResult(T data, ApiStatus status, string message)
        {
            Data = data;
            Code = status;
            Message = message;
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public ApiStatus Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// <see cref="ApiResult{T}"/>默认实现方式
    /// </summary>
    public class ApiResult : ApiResult<object>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResult(object data, ApiStatus status, string message) : base(data, status, message)
        {
        }
    }
}
