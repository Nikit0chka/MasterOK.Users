using API.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Get;

internal class GetValidator:Validator<GetRequest>
{
    public GetValidator()
    {
        RuleFor(static loginRequest => loginRequest.Id)
            .NotEmpty().WithMessage(AuthorizationValidationMessages.IdIsRequired);
    }
}