using API.Contracts;
using Application.CQRS.Users.Validate;

namespace API.Endpoints.Validate;

public sealed class ValidateErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        ValidateErrorCodes.UserNotFound => StatusCodes.Status401Unauthorized,
        ValidateErrorCodes.UserEmailNotConfirmed => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        ValidateErrorCodes.UserNotFound => "Unauthorized",
        ValidateErrorCodes.UserEmailNotConfirmed => "Forbidden",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        ValidateErrorCodes.UserNotFound => "User not found or password is incorrect",
        ValidateErrorCodes.UserEmailNotConfirmed => "Email is not confirmed. Please check your inbox",
        _ => "An error occurred"
    };
}