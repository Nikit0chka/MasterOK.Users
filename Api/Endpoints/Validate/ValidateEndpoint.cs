using API.Endpoints.Base;
using Application.CQRS.Users.Validate;
using MediatR;

namespace API.Endpoints.Validate;

public class ValidateEndpoint(IMediator mediator, ValidateErrorMapper errorMapper):BaseEndpoint<ValidateRequest, ValidateResponse>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/validate");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ValidateRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ValidateCommand(request.Email, request.Password, request.Role), cancellationToken);

        await SendResponseByResult(result, static validateCommandResult => new(validateCommandResult.UserId, validateCommandResult.Roles), errorMapper, cancellationToken);
    }
}