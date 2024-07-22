using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Artemis.Extensions.ServiceConnect;

/// <summary>
///     Aspire组件扩展
/// </summary>
public static class ComponentExtensions
{

    /// <summary>
    ///     添加Redis组件
    /// </summary>
    /// <param name="builder">appBuilder</param>
    /// <param name="connectionName">连接名</param>
    /// <returns></returns>
    public static void AddRedisComponent(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddRedisClient(connectionName);
        builder.AddRedisOutputCache(connectionName);
        builder.AddRedisDistributedCache(connectionName);
    }

    /// <summary>
    ///     添加MongoDB组件
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMongoDbComponent(this IHostApplicationBuilder builder,
        string connectionName)
    {
        builder.AddMongoDBClient(connectionName);
        return builder;
    }

    /// <summary>
    ///     添加RabbitMQ组件
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddRabbitMqComponent(this IHostApplicationBuilder builder,
        string connectionName)
    {
        builder.AddRabbitMQClient(connectionName);
        return builder;
    }

    /// <summary>
    ///     添加Postgresql组件
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="connectionName"></param>
    /// <param name="migrateAction"></param>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public static IServiceCollection AddPostgreSqlComponent<TDbContext>(this IHostApplicationBuilder builder,
        string connectionName,
        Action<NpgsqlDbContextOptionsBuilder>? migrateAction = null,
        Action<string>? logger = null,
        LogLevel logLevel = LogLevel.Debug) where TDbContext : DbContext
    {
        logger ??= Console.WriteLine;

        builder.AddNpgsqlDbContext<TDbContext>(connectionName, configureDbContextOptions: config =>
        {
            if (migrateAction != null) config.UseNpgsql(migrateAction);

            config.EnableServiceProviderCaching()
                .EnableDetailedErrors(builder.Environment.IsDevelopment())
                .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                .LogTo(logger, logLevel);
        });

        if (builder.Environment.IsDevelopment())
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        return builder.Services;
    }
}