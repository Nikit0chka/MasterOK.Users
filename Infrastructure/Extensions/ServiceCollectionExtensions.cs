﻿using Application.Contracts;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Infrastructure.Data;
using Infrastructure.Services;
using Kafka.Extensions;
using Kafka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

/// <summary>
///     Service collection extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Add infrastructure services to service collections
    /// </summary>
    /// <param name="serviceCollection"> Service collection </param>
    /// <param name="configurationManager"> Configuration manager </param>
    /// <param name="logger"> Logger </param>
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, ILogger logger)
    {
        logger.LogInformation("Adding infrastructure services...");

        logger.LogInformation("Adding database...");
        var dbConnectionString = configurationManager.GetConnectionString("DefaultConnection");

        serviceCollection.AddDbContext<DbContext, Context>
            (options => options.UseSqlServer(Guard.Against.NullOrEmpty(dbConnectionString,
                                                                       nameof(dbConnectionString),
                                                                       "Database connection string was null or empty.")));

        serviceCollection.AddKafka(configurationManager, logger);

        logger.LogInformation("Adding repositories...");
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        serviceCollection.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        serviceCollection.AddScoped<IPasswordHasherService, PasswordHasherService>();
        serviceCollection.AddScoped<IConfirmationCodeService, ConfirmationCodeService>();

        logger.LogInformation("Infrastructure services added");
    }

    private static void AddKafka(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, ILogger logger)
    {
        logger.LogInformation("Adding kafka producers...");
        serviceCollection.AddKafkaProducer<EmailConfirmationCodeMessage>(configurationManager.GetSection("Kafka:Producer:SendAuthorizationCode"), logger);
        logger.LogInformation("Kafka producers added...");
    }
}