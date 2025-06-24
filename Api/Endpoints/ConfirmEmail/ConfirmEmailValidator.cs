using API.Validation;
using Domain.AggregateModels.UserAggregate;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.ConfirmEmail;

internal class ConfirmEmailValidator:Validator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(static confirmEmailRequest => confirmEmailRequest.Email)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.EmailIsRequired)
            .MinimumLength(UserConstants.EmailMinLength).WithMessage(AuthorizationValidationMessages.EmailMinLength)
            .MaximumLength(UserConstants.EmailMaxLength).WithMessage(AuthorizationValidationMessages.EmailMaxLength)
            .EmailAddress().WithMessage(AuthorizationValidationMessages.InvalidEmailFormat);

        RuleFor(static confirmEmailRequest => confirmEmailRequest.ConfirmationCode)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.ConfirmationCodeIsRequired);
    }
}