namespace Domain.AggregateModels.UserAggregate;

internal static class UserValidationMessages
{
    public const string EmailIsRequired = "Email is required";
    public const string PasswordHashIsRequired = "Password is required";
    public const string InvalidEmailFormat = "Email is not a valid email address.";
    public readonly static string EmailIsOutOfRange = $"Email must be between {UserConstants.EmailMinLength} and {UserConstants.EmailMaxLength} characters.";
    public readonly static string PasswordHashIsOutOfRange = $"Password hash must be between {UserConstants.PasswordHashMinLength} and {UserConstants.PasswordHashMaxLength} characters.";
}