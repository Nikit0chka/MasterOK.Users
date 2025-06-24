using API.Contracts;
using Application.CQRS.Users.Register;

namespace API.Endpoints.Register;

public sealed class RegisterErrorMapper:IErrorMapper
{
    public int GetStatusCode(string? errorCode) => errorCode switch
    {
        RegisterErrorCodes.EmailAlreadyRegistered => StatusCodes.Status409Conflict,
        RegisterErrorCodes.EmailRegisteredNotConfirmed => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    public string GetTitle(string? errorCode) => errorCode switch
    {
        RegisterErrorCodes.EmailAlreadyRegistered => "Email already registered",
        RegisterErrorCodes.EmailRegisteredNotConfirmed => "Email registered, not confirmed",
        _ => "Internal Server Error"
    };

    public string GetDetail(string? errorCode) => errorCode switch
    {
        RegisterErrorCodes.EmailAlreadyRegistered => "Provided email already registered",
        RegisterErrorCodes.EmailRegisteredNotConfirmed => "Proved email already registered, but not confirmed",
        _ => "An error occurred"
    };
}