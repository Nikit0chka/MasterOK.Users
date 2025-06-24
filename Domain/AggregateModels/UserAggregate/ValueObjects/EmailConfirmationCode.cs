using Ardalis.SharedKernel;
using Domain.Extensions;

namespace Domain.AggregateModels.UserAggregate.ValueObjects;

public class EmailConfirmationCode:ValueObject
{

    /// <summary>
    ///     Ef constructor
    /// </summary>
    protected EmailConfirmationCode() { }

    public EmailConfirmationCode(string code, DateTime expirationDate)
    {
        Validate(code, expirationDate);
        Code = code;
        ExpirationDate = expirationDate;
    }

    public DateTime ExpirationDate { get; }
    public string Code { get; } = null!;

    public bool IsExpired() => ExpirationDate <= DateTime.UtcNow;

    internal bool Matches(string code) => Code == code;

    private static void Validate(string code, DateTime expirationDate)
    {
        GuardAgainstExtensions.StringLengthOutOfRange(code,
                                                      UserConstants.ConfirmationCodeMinLength,
                                                      UserConstants.ConfirmationCodeMaxLength,
                                                      nameof(code),
                                                      $"Confirmation code must be between {UserConstants.ConfirmationCodeMinLength} and {UserConstants.ConfirmationCodeMaxLength} characters.");

        if (expirationDate < DateTime.UtcNow)
            throw new ArgumentException("Expiration date cannot be in the past.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return ExpirationDate;
    }
}