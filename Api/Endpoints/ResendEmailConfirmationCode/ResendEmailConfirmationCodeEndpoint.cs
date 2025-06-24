using API.Endpoints.Base;
using Application.CQRS.Users.ResendEmailConfirmationCode;
using MediatR;

namespace API.Endpoints.ResendEmailConfirmationCode;

public class ResendEmailConfirmationCodeEndpoint(IMediator mediator, ResendEmailConfirmationCodeErrorMapper errorMapper):BaseEndpoint<ResendEmailConfirmationCodeRequest>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/resend-email-confirmation-code");
        AllowAnonymous();
    }


    public override async Task HandleAsync(ResendEmailConfirmationCodeRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ResendEmailConfirmationCodeCommand(request.Email), cancellationToken);

        await SendResponseByResult(result, errorMapper, cancellationToken);
    }
}