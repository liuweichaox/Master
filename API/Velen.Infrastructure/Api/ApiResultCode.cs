namespace Velen.Infrastructure.Api;

public enum ApiResultCode
{
    Success = 200,
    ClientError = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}