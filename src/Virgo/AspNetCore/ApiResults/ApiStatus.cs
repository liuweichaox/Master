namespace Virgo.AspNetCore.ApiResults
{
    /// <summary>
    /// API状态
    /// </summary>
    public enum ApiStatus
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        SUCCESS = 200,
        /// <summary>
        /// 参数错误
        /// </summary>
        PARAM_ERROR = 400,
        /// <summary>
        /// 操作失败
        /// </summary>
        FAIL = 500
    }
}
