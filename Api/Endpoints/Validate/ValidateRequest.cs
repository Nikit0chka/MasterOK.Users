using Domain.AggregateModels.UserAggregate.ValueObjects;

namespace API.Endpoints.Validate;

public readonly record struct ValidateRequest(string Email, string Password, Role Role);