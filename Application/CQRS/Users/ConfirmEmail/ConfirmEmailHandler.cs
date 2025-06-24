using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.AggregateModels.UserAggregate.Specifications;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Users.ConfirmEmail;

internal class ConfirmEmailHandler(ILogger<ConfirmEmailHandler> logger, IRepository<User> userRepository):ICommandHandler<ConfirmEmailCommand, OperationResult>
{
    public async Task<OperationResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with email: {Email}", nameof(ConfirmEmailCommand), request.Email);

        try
        {
            var user = await userRepository.SingleOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with email {Email} is not exists", request.Email);
                return OperationResult.Error(ConfirmEmailErrorCodes.UserNotFound);
            }

            var emailConfirmationResult = user.ConfirmEmail(request.ConfirmationCode);

            if (!emailConfirmationResult.IsSuccess)
            {
                logger.LogWarning("User with email {Email} was not confirmed, UserId: {UserId}", request.Email, user.Id);

                return MapDomainError(emailConfirmationResult.ErrorCode);
            }

            await userRepository.UpdateAsync(user, cancellationToken);

            logger.LogInformation("{Command} handled successful. Email has been confirmed and updated UserId: {UserId}, Email: {Email}", nameof(ConfirmEmailCommand), user.Id, user.Email);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with Email: {Email}", nameof(ConfirmEmailCommand), request.Email);
            return OperationResult.Error();
        }
    }

    private static OperationResult MapDomainError(string? domainError) => domainError switch
    {
        UserErrorCodes.InvalidConfirmationCode =>
            OperationResult.Error(ConfirmEmailErrorCodes.InvalidConfirmationCode),
        UserErrorCodes.ConfirmationCodeHasExpired =>
            OperationResult.Error(ConfirmEmailErrorCodes.ConfirmationCodeHasExpired),
        UserErrorCodes.EmailAlreadyConfirmed =>
            OperationResult.Error(ConfirmEmailErrorCodes.EmailAlreadyConfirmed),
        _ => OperationResult.Error()
    };
}