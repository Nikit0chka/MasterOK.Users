using API.Contracts;
using Application.CQRS.Users.ConfirmEmail;

namespace API.Endpoints.ConfirmEmail;

public sealed class ConfirmEmailErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        ConfirmEmailErrorCodes.UserNotFound => StatusCodes.Status404NotFound,
        ConfirmEmailErrorCodes.InvalidConfirmationCode => StatusCodes.Status400BadRequest,
        ConfirmEmailErrorCodes.EmailAlreadyConfirmed => StatusCodes.Status409Conflict,
        ConfirmEmailErrorCodes.ConfirmationCodeHasExpired => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        ConfirmEmailErrorCodes.UserNotFound => "Not Found",
        ConfirmEmailErrorCodes.InvalidConfirmationCode => "Invalid Confirmation Code",
        ConfirmEmailErrorCodes.EmailAlreadyConfirmed => "Email Already Confirmed",
        ConfirmEmailErrorCodes.ConfirmationCodeHasExpired => "Expired Confirmation Code",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        ConfirmEmailErrorCodes.UserNotFound => "User for provided email was not found",
        ConfirmEmailErrorCodes.InvalidConfirmationCode => "Provided confirmation code is invalid",
        ConfirmEmailErrorCodes.EmailAlreadyConfirmed => "User by provided email already confirmed",
        ConfirmEmailErrorCodes.ConfirmationCodeHasExpired => "Provided confirmation code has expired",
        _ => "An error occurred"
    };
}