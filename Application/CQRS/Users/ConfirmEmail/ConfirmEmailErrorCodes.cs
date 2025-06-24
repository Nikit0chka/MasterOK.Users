namespace Application.CQRS.Users.ConfirmEmail;

public abstract class ConfirmEmailErrorCodes
{
    public const string UserNotFound = "CONFIRM_EMAIL_USER_NOT_FOUND";
    public const string ConfirmationCodeHasExpired = "CONFIRM_EMAIL_CONFIRMATION_CODE_EXPIRED";
    public const string EmailAlreadyConfirmed = "CONFIRM_EMAIL_USER_EMAIL_ALREADY_CONFIRMED";
    public const string InvalidConfirmationCode = "CONFIRM_EMAIL_INVALID_CODE";
}