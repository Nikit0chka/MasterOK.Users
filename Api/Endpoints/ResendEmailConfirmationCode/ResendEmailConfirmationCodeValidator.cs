using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.ResendEmailConfirmationCode;

internal class ResendEmailConfirmationCodeValidator:Validator<ResendEmailConfirmationCodeRequest>
{
    public ResendEmailConfirmationCodeValidator()
    {
        RuleFor(static confirmEmailRequest => confirmEmailRequest.Email)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.EmailIsRequired)
            .MinimumLength(AuthorizationValidationConstants.EmailMinLength).WithMessage(AuthorizationValidationMessages.EmailMinLength)
            .MaximumLength(AuthorizationValidationConstants.EmailMaxLength).WithMessage(AuthorizationValidationMessages.EmailMaxLength)
            .EmailAddress().WithMessage(AuthorizationValidationMessages.InvalidEmailFormat);
    }
}