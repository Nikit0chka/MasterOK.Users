using Domain.AggregateModels.UserAggregate;
using Infrastructure.Data.Configs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <inheritdoc />
/// <summary>
///     Data base context
/// </summary>
internal sealed class Context:DbContext
{
    public DbSet<User> Users { get; init; }

    public Context(DbContextOptions<Context> options):base(options) { Database.EnsureCreated(); }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}