using Application.Contracts;
using Application.CQRS.Users.Register;
using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.AggregateModels.UserAggregate.Specifications;
using Domain.Utils;
using Kafka.Contracts;
using Kafka.Models;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Users.ResendEmailConfirmationCode;

internal sealed class ResendEmailConfirmationCodeHandler(
    IRepository<User> userRepository,
    IConfirmationCodeService confirmationCodeService,
    IKafkaProducer<EmailConfirmationCodeMessage> kafkaProducer,
    ILogger<RegisterCommandHandler> logger):ICommandHandler<ResendEmailConfirmationCodeCommand, OperationResult>
{
    public async Task<OperationResult> Handle(ResendEmailConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with Email: {Email}", nameof(ResendEmailConfirmationCodeCommand), request.Email);

        try
        {
            var user = await userRepository.SingleOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with email {Email} not found", request.Email);
                return OperationResult.Error(ResendEmailConfirmationCodeErrorCodes.UserNotFound);
            }

            if (user.IsEmailConfirmed)
            {
                logger.LogWarning("An attempt to resend email confirmation code to confirmed user, UserId: {UserId}", user.Id);
                return OperationResult.Error(ResendEmailConfirmationCodeErrorCodes.EmailAlreadyConfirmed);
            }

            var confirmationCode = confirmationCodeService.GenerateConfirmationCode();

            var result = user.SetNewConfirmationCode(confirmationCode);

            if (!result.IsSuccess)
                return MapDomainError(result.ErrorCode);

            await userRepository.UpdateAsync(user, cancellationToken);

            logger.LogInformation("Updated confirmation code for user with Id: {UserId}", user.Id);

            var authorizationCodeMessage = new EmailConfirmationCodeMessage(request.Email, confirmationCode);

            await kafkaProducer.ProduceAsync(authorizationCodeMessage, cancellationToken);

            logger.LogInformation("User confirmation code has been send User email: {UserEmail}", request.Email);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with Email: {Email}", nameof(ResendEmailConfirmationCodeCommand), request.Email);
            return OperationResult.Error();
        }
    }

    private static OperationResult MapDomainError(string? domainError) => domainError switch
    {
        UserErrorCodes.EmailAlreadyConfirmed =>
            OperationResult.Error(ResendEmailConfirmationCodeErrorCodes.EmailAlreadyConfirmed),
        _ => OperationResult.Error()
    };
}