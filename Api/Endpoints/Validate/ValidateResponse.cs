using Domain.AggregateModels.UserAggregate.ValueObjects;

namespace API.Endpoints.Validate;

public readonly record struct ValidateResponse(int UserId, List<Role> Roles);