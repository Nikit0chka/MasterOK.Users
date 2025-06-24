namespace Domain.AggregateModels.UserAggregate;

public static class UserErrorCodes
{
    public const string EmailAlreadyConfirmed = "DOMAIN_USER_EMAIL_ALREADY_CONFIRMED";
    internal const string ConfirmationCodeNotProvided = "DOMAIN_USER_CONFIRMATION_CODE_NOT_PROVIDED";
    public const string InvalidConfirmationCode = "DOMAIN_USER_INVALID_CONFIRMATION_CODE";
    public const string ConfirmationCodeHasExpired = "DOMAIN_USER_CONFIRMATION_CODE_HAS_EXPIRED";
    internal const string ConfirmationCodeCreationFailed = "DOMAIN_USER_CONFIRMATION_CODE_FAILED";
    public const string RoleAlreadyExists = "DOMAIN_USER_ROLE_ALREADY_EXISTS";
}