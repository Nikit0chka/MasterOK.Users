using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.AggregateModels.UserAggregate.Specifications;
using Domain.Utils;
using Kafka.Contracts;
using Kafka.Models;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Users.Register;

internal sealed class RegisterCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasherService passwordHasherService,
    IConfirmationCodeService confirmationCodeService,
    IKafkaProducer<EmailConfirmationCodeMessage> kafkaProducer,
    ILogger<RegisterCommandHandler> logger):ICommandHandler<RegisterCommand, OperationResult>
{
    public async Task<OperationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with Email: {Email}", nameof(RegisterCommand), request.Email);

        try
        {
            var userWithSameEmail = await userRepository.SingleOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

            if (userWithSameEmail is not null)
                return await HandleUserAlreadyExists(userWithSameEmail, request.Email, request.Password, cancellationToken);

            return await HandleUserNotExist(request.Email, request.Password, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with Email: {Email}", nameof(RegisterCommand), request.Email);
            return OperationResult.Error();
        }
    }

    private async Task<OperationResult> HandleUserAlreadyExists(User user, string email, string password, CancellationToken cancellationToken)
    {
        logger.LogWarning("User with Email: {Email} already exists", email);

        if (user.IsEmailConfirmed)
            return OperationResult.Error(RegisterErrorCodes.EmailAlreadyRegistered);

        if (user.EmailConfirmationCode is null || user.EmailConfirmationCode.IsExpired())
        {
            logger.LogWarning("User with Email: {Email} has not confirmed email. Confirmation code is expired, deleting user", email);
            await userRepository.DeleteAsync(user, cancellationToken);
            return await HandleUserNotExist(email, password, cancellationToken);
        }

        logger.LogWarning("User with Email: {Email} has not confirmed email. Confirmation code is not expired", email);
        return OperationResult.Error(RegisterErrorCodes.EmailRegisteredNotConfirmed);
    }

    private async Task<OperationResult> HandleUserNotExist(string email, string password, CancellationToken cancellationToken)
    {
        var confirmationCode = confirmationCodeService.GenerateConfirmationCode();

        var user = await AddUser(password, email, confirmationCode, cancellationToken);

        await ProduceEmailConfirmationCode(email, confirmationCode, cancellationToken);
        logger.LogInformation("User confirmation code has been send. Email: {UserEmail}", email);

        logger.LogInformation("{Command} handled successful. User created. UserId: {UserId}, Email: {Email}", nameof(RegisterCommand), user.Id, user.Email);
        return OperationResult.Success();
    }

    private Task ProduceEmailConfirmationCode(string email, string confirmationCode, CancellationToken cancellationToken)
    {
        var authorizationCodeMessage = new EmailConfirmationCodeMessage(email, confirmationCode);

        return kafkaProducer.ProduceAsync(authorizationCodeMessage, cancellationToken);
    }

    private Task<User> AddUser(string password, string email, string confirmationCode, CancellationToken cancellationToken)
    {
        var hashedPassword = passwordHasherService.HashPassword(password);
        var user = new User(email, hashedPassword, confirmationCode);

        return userRepository.AddAsync(user, cancellationToken);
    }
}