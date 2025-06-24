namespace Application.CQRS.Users.ResendEmailConfirmationCode;

public static class ResendEmailConfirmationCodeErrorCodes
{
    public const string UserNotFound = "RESEND_CONFIRMATION_CODE_USER_NOT_FOUND";
    public const string EmailAlreadyConfirmed = "RESEND_CONFIRMATION_CODE_USER_EMAIL_ALREADY_CONFIRMED";
}