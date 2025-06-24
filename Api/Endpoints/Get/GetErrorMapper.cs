using API.Contracts;
using Application.CQRS.Users.Get;

namespace API.Endpoints.Get;

public sealed class GetErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        GetUserErrorCodes.UserNotFound => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        GetUserErrorCodes.UserNotFound => "Not Found",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        GetUserErrorCodes.UserNotFound => "User not found",
        _ => "An error occurred"
    };
}