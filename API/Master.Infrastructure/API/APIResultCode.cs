namespace Master.Infrastructure.API;

public enum APIResultCode
{
    Success = 200,
    ClientError = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}