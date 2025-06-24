using API.Endpoints.Base;
using Application.CQRS.Users.ConfirmEmail;
using MediatR;

namespace API.Endpoints.ConfirmEmail;

public class ConfirmEmailEndpoint(IMediator mediator, ConfirmEmailErrorMapper errorMapper):BaseEndpoint<ConfirmEmailRequest>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/confirm-email");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ConfirmEmailCommand(request.Email, request.ConfirmationCode), cancellationToken);

        await SendResponseByResult(result, errorMapper, cancellationToken);
    }
}