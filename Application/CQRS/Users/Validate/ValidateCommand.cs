using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate.ValueObjects;
using Domain.Utils;

namespace Application.CQRS.Users.Validate;

public readonly record struct ValidateCommand(string Email, string Password, Role Role):ICommand<OperationResult<ValidateCommandResult>>;