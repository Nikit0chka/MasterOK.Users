using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate.ValueObjects;
using Domain.Extensions;
using Domain.Utils;
using JetBrains.Annotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Domain.AggregateModels.UserAggregate;

/// <inheritdoc cref="Ardalis.SharedKernel.EntityBase" />
/// <summary>
///     User entity
/// </summary>
public sealed class User:EntityBase, IAggregateRoot
{
    public ICollection<Role> Roles { get; init; }
    public bool IsEmailConfirmed { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public EmailConfirmationCode? EmailConfirmationCode { get; private set; }

    /// <summary>
    ///     Ef constructor
    /// </summary>
    [UsedImplicitly]
    private User() { }

    public User(string email, string passwordHash, string emailConfirmationCode)
    {
        Guard.Against.NullOrEmpty(email, nameof(email), UserValidationMessages.EmailIsRequired);
        GuardAgainstExtensions.StringLengthOutOfRange(email, UserConstants.EmailMinLength, UserConstants.EmailMaxLength, nameof(email), UserValidationMessages.EmailIsOutOfRange);

        GuardAgainstExtensions.InvalidEmail(email, nameof(email), UserValidationMessages.InvalidEmailFormat);

        Guard.Against.NullOrEmpty(passwordHash, nameof(passwordHash), UserValidationMessages.PasswordHashIsRequired);

        GuardAgainstExtensions.StringLengthOutOfRange(passwordHash,
                                                      UserConstants.PasswordHashMinLength,
                                                      UserConstants.PasswordHashMaxLength,
                                                      nameof(passwordHash),
                                                      UserValidationMessages.PasswordHashIsOutOfRange);


        //TODO: разобраться с ролями
        Roles = new List<Role>
                {
                    Role.Customer,
                    Role.Master
                };

        EmailConfirmationCode = new(emailConfirmationCode, DateTime.UtcNow.AddMinutes(UserConstants.ConfirmationCodeExpirationTimeInMinutes));
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        IsEmailConfirmed = false;
    }

    public OperationResult SetNewConfirmationCode(string emailConfirmationCode)
    {
        if (IsEmailConfirmed)
            return OperationResult.Error(UserErrorCodes.EmailAlreadyConfirmed);

        if (string.IsNullOrWhiteSpace(emailConfirmationCode))
            return OperationResult.Error(UserErrorCodes.InvalidConfirmationCode);

        try
        {
            UpdateConfirmationCode(emailConfirmationCode);
            return OperationResult.Success();
        }
        catch (ArgumentException)
        {
            return OperationResult.Error(UserErrorCodes.ConfirmationCodeCreationFailed);
        }
    }

    public OperationResult ConfirmEmail(string confirmationCode)
    {
        if (IsEmailConfirmed)
            return OperationResult.Error(UserErrorCodes.EmailAlreadyConfirmed);

        if (EmailConfirmationCode is null)
            return OperationResult.Error(UserErrorCodes.ConfirmationCodeNotProvided);

        if (!EmailConfirmationCode.Matches(confirmationCode))
            return OperationResult.Error(UserErrorCodes.InvalidConfirmationCode);

        if (EmailConfirmationCode.IsExpired())
            return OperationResult.Error(UserErrorCodes.ConfirmationCodeHasExpired);

        IsEmailConfirmed = true;
        EmailConfirmationCode = null;
        return OperationResult.Success();
    }

    public OperationResult AddRole(Role role)
    {
        if (Roles.Contains(role))
            return OperationResult.Error(UserErrorCodes.RoleAlreadyExists);

        Roles.Add(role);
        return OperationResult.Success();
    }

    private void UpdateConfirmationCode(string code) { EmailConfirmationCode = new(code, DateTime.UtcNow.AddMinutes(UserConstants.ConfirmationCodeExpirationTimeInMinutes)); }
}