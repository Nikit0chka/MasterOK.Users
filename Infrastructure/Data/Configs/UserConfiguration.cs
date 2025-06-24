using Domain.AggregateModels.UserAggregate;
using Domain.AggregateModels.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs;

/// <inheritdoc />
/// <summary>
///     User entity ef core configuration <see cref="User" />
/// </summary>
internal class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(static user => user.PasswordHash)
            .HasMaxLength(UserConstants.PasswordHashMaxLength)
            .IsRequired();

        builder.Property(static user => user.Email)
            .HasMaxLength(UserConstants.EmailMaxLength)
            .IsRequired();

        builder.Property(static user => user.Roles)
            .HasConversion(
                           static roles => string.Join(",", roles.Select(static role => role.Name)),
                           static rolesString => rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(static roleName => Role.FromName(roleName, false))
                               .ToList(),
                           new ValueComparer<ICollection<Role>>(static (c1, c2) =>
                                                                    c1 != null && c2 != null && c1.SequenceEqual(c2),
                                                                static c => c.Aggregate(0, static (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                                                static c => c.ToList()
                                                               ));

        builder.OwnsOne(static user => user.EmailConfirmationCode).Property(static emailConfirmationCode => emailConfirmationCode.Code).HasMaxLength(UserConstants.ConfirmationCodeMaxLength);
        builder.OwnsOne(static user => user.EmailConfirmationCode).Property(static emailConfirmationCode => emailConfirmationCode.ExpirationDate);
    }
}