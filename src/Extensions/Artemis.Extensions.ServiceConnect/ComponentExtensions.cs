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
        builder.AddRedisClient("RedisInstance");
        builder.AddRedisOutputCache("RedisInstance");
        builder.AddRedisDistributedCache("RedisInstance");

        return builder;
    }
}