using Ardalis.SharedKernel;
using Domain.AggregateModels.UserAggregate;
using Domain.Utils;

namespace Application.CQRS.Users.Get;

public readonly record struct GetUserQuery(int Id):IQuery<OperationResult<User>>;