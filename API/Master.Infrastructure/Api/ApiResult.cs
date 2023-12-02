namespace Master.Infrastructure.Api;

public class ApiResult<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public int Code { get; set; }

    public ApiResult(T data, bool success, string message, ApiResultCode code)
    {
        Data = data;
        Success = success;
        Message = message;
        Code = (int)code;
    }

    public static ApiResult<T> SuccessResult(T data)
    {
        return new ApiResult<T>(data, true, string.Empty, ApiResultCode.Success);
    }

    public static ApiResult<T> SuccessResult(T data, string message)
    {
        return new ApiResult<T>(data, true, message, ApiResultCode.Success);
    }

    public static ApiResult<T> ErrorResult(string message)
    {
        return new ApiResult<T>(default!, false, message, ApiResultCode.ClientError);
    }

    public static ApiResult<T> ErrorResult(string message, ApiResultCode code)
    {
        return new ApiResult<T>(default(T)!, false, message, code);
    }
}

public class ApiResult : ApiResult<object>
{
    public ApiResult(object data, bool success, string message, ApiResultCode code) : base(data, success, message, code)
    {
    }
}