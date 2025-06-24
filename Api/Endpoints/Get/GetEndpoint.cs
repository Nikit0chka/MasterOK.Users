using API.Endpoints.Base;
using Application.CQRS.Users.Get;
using MediatR;

namespace API.Endpoints.Get;

public class GetEndpoint(IMediator mediator, GetErrorMapper errorMapper):BaseEndpoint<GetRequest, GetResponse>
{
    public override void Configure()
    {
        Get(BaseEndpointsRoute.BaseRoute + "/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserQuery(request.Id), cancellationToken);

        await SendResponseByResult(result, static getUserResult => new(getUserResult.Id, getUserResult.Roles.ToList()), errorMapper, cancellationToken);
    }
}