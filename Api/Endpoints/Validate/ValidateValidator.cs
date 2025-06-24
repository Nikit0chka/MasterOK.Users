using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Validate;

internal class ValidateValidator:Validator<ValidateRequest>
{
    public ValidateValidator()
    {
        RuleFor(static loginRequest => loginRequest.Email)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.EmailIsRequired)
            .MinimumLength(AuthorizationValidationConstants.EmailMinLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMinLength)
            .MaximumLength(AuthorizationValidationConstants.EmailMaxLength).WithMessage(AuthorizationValidationMessages.RefreshTokenMaxLength)
            .EmailAddress().WithMessage(AuthorizationValidationMessages.InvalidEmailFormat);

        RuleFor(static loginRequest => loginRequest.Password)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.PasswordIsRequired);

        RuleFor(static loginRequest => loginRequest.Role).NotEmpty().WithMessage(AuthorizationValidationMessages.RoleIsRequired);
    }
}