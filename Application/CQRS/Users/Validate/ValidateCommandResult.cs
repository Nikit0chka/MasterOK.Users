using Domain.AggregateModels.UserAggregate.ValueObjects;

namespace Application.CQRS.Users.Validate;

public readonly record struct ValidateCommandResult(int UserId, List<Role> Roles);