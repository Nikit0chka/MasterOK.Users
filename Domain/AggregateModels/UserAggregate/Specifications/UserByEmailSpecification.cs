using Ardalis.Specification;

namespace Domain.AggregateModels.UserAggregate.Specifications;

/// <inheritdoc cref="Ardalis.Specification.Specification{T}" />
/// <summary>
///     User specification by email
/// </summary>
public sealed class UserByEmailSpecification:Specification<User>, ISingleResultSpecification<User>
{
    public UserByEmailSpecification(string email) { Query.Where(user => user.Email == email); }
}