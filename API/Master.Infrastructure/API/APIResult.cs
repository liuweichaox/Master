namespace Master.Infrastructure.API;

public class APIResult<T>
{
    public APIResult(T data, bool success, string message, APIResultCode code)
    {
        Data = data;
        Success = success;
        Message = message;
        Code = (int)code;
    }

    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public int Code { get; set; }

    public static APIResult<T> SuccessResult(T data)
    {
        return new APIResult<T>(data, true, string.Empty, APIResultCode.Success);
    }

    public static APIResult<T> SuccessResult(T data, string message)
    {
        return new APIResult<T>(data, true, message, APIResultCode.Success);
    }

    public static APIResult<T> ErrorResult(string message)
    {
        return new APIResult<T>(default!, false, message, APIResultCode.ClientError);
    }

    public static APIResult<T> ErrorResult(string message, APIResultCode code)
    {
        return new APIResult<T>(default!, false, message, code);
    }
}

public class APIResult : APIResult<object>
{
    public APIResult(object data, bool success, string message, APIResultCode code) : base(data, success, message, code)
    {
    }
}