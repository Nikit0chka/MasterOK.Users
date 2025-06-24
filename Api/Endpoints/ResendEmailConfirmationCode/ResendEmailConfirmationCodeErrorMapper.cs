using API.Contracts;
using Application.CQRS.Users.ResendEmailConfirmationCode;

namespace API.Endpoints.ResendEmailConfirmationCode;

public sealed class ResendEmailConfirmationCodeErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        ResendEmailConfirmationCodeErrorCodes.UserNotFound => StatusCodes.Status404NotFound,
        ResendEmailConfirmationCodeErrorCodes.EmailAlreadyConfirmed => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        ResendEmailConfirmationCodeErrorCodes.UserNotFound => "User not found",
        ResendEmailConfirmationCodeErrorCodes.EmailAlreadyConfirmed => "Email already confirmed",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        ResendEmailConfirmationCodeErrorCodes.UserNotFound => "User by provided email not found",
        ResendEmailConfirmationCodeErrorCodes.EmailAlreadyConfirmed => "Provided email already confirmed",
        _ => "An error occurred"
    };
}