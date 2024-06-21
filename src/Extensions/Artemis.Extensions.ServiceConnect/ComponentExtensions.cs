using Microsoft.Extensions.Hosting;

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
    public static IHostApplicationBuilder AddRedisComponent(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddRedisClient(connectionName);
        builder.AddRedisOutputCache(connectionName);
        builder.AddRedisDistributedCache(connectionName);
        return builder;
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
}