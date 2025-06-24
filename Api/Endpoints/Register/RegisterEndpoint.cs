using API.Endpoints.Base;
using Application.CQRS.Users.Register;
using MediatR;

namespace API.Endpoints.Register;

public class RegisterEndpoint(IMediator mediator, RegisterErrorMapper errorMapper):BaseEndpoint<RegisterRequest>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);

        await SendResponseByResult(result, errorMapper, cancellationToken);
    }
}