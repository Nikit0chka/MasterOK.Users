using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.AggregateModels.UserAggregate.Specifications;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Users.Validate;

internal sealed class ValidateCommandHandler(
    IReadRepository<User> userReadRepository,
    ILogger<ValidateCommandHandler> logger,
    IPasswordHasherService passwordHasherService):ICommandHandler<ValidateCommand, OperationResult<ValidateCommandResult>>
{
    public async Task<OperationResult<ValidateCommandResult>> Handle(ValidateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Command} for user with email: {Email}", nameof(ValidateCommand), request.Email);

        try
        {
            var user = await userReadRepository.SingleOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with Email: {Email} not found", request.Email);
                return OperationResult<ValidateCommandResult>.Error(ValidateErrorCodes.UserNotFound);
            }

            if (!user.Roles.Contains(request.Role))
            {
                logger.LogWarning("User with Email: {Email} found, but not contains Role: {Role} not found", request.Email, request.Role);
                return OperationResult<ValidateCommandResult>.Error(ValidateErrorCodes.UserNotFound);
            }

            if (!passwordHasherService.VerifyPassword(request.Password, user.PasswordHash))
            {
                logger.LogWarning("Invalid password for user with Email: {Email}. UserId: {UserId}", request.Email, user.Id);
                return OperationResult<ValidateCommandResult>.Error(ValidateErrorCodes.UserNotFound);
            }

            if (!user.IsEmailConfirmed)
            {
                logger.LogWarning("An attempt login with unconfirmed Email: {Email}", request.Email);
                return OperationResult<ValidateCommandResult>.Error(ValidateErrorCodes.UserEmailNotConfirmed);
            }

            logger.LogInformation("{Command} handled successfully.  Email: {Email}. UserId: {UserId}", nameof(ValidateCommand), request.Email, user.Id);
            return OperationResult<ValidateCommandResult>.Success(new(user.Id, user.Roles.ToList()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling {Command} for user with Email: {Email}", nameof(ValidateCommand), request.Email);
            return OperationResult<ValidateCommandResult>.Error();
        }
    }
}