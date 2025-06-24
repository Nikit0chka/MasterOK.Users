using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Register;

internal class RegisterValidator:Validator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(static registerRequest => registerRequest.Email).NotEmpty().WithMessage(AuthorizationValidationMessages.EmailIsRequired)
            .MinimumLength(AuthorizationValidationConstants.EmailMinLength).WithMessage(AuthorizationValidationMessages.EmailMinLength)
            .MaximumLength(AuthorizationValidationConstants.EmailMaxLength).WithMessage(AuthorizationValidationMessages.EmailMaxLength)
            .EmailAddress().WithMessage(AuthorizationValidationMessages.InvalidEmailFormat);

        RuleFor(static registerRequest => registerRequest.Password)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.PasswordIsRequired);
    }
}