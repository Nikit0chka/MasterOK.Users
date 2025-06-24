namespace Domain.AggregateModels.UserAggregate;

/// <summary>
///     User entity constants <see cref="User" />
/// </summary>
public static class UserConstants
{
    public const int EmailMinLength = 3;
    public const int EmailMaxLength = 64;
    public const int PasswordHashMaxLength = 64;
    public const int ConfirmationCodeMaxLength = 64;
    internal const int PasswordHashMinLength = 6;
    internal const int ConfirmationCodeMinLength = 6;
    internal const int ConfirmationCodeExpirationTimeInMinutes = 3;
}