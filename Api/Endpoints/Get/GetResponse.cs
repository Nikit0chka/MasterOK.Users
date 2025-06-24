using Domain.AggregateModels.UserAggregate.ValueObjects;

namespace API.Endpoints.Get;

public readonly record struct GetResponse(int UserId, List<Role> Roles);