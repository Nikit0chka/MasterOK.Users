using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Users.Get;

public class GetUserHandler(IReadRepository<User> userRepository, ILogger<GetUserHandler> logger):IQueryHandler<GetUserQuery, OperationResult<User>>
{
    public async Task<OperationResult<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Query} for user with Id: {UserId}", nameof(GetUserQuery), request.Id);

        try
        {
            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with Id {Id} not found", request.Id);
                return OperationResult<User>.Error(GetUserErrorCodes.UserNotFound);
            }

            logger.LogInformation("{Query} handled successful", nameof(GetUserQuery));

            return OperationResult<User>.Success(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Query}, for user with Id: {UserId}", nameof(GetUserQuery), request.Id);
            return OperationResult<User>.Error();
        }
    }
}