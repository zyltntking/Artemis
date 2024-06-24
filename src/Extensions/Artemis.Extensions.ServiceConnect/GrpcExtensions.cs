using Artemis.Extensions.ServiceConnect.Interceptors;
using Artemis.Extensions.ServiceConnect.SwaggerFilters;
using Artemis.Extensions.ServiceConnect.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.ServiceConnect;

/// <summary>
///     Grpc扩展
/// </summary>
public static class GrpcExtensions
{
    /// <summary>
    ///     配置Grpc
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <param name="enableValidator"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureGrpc(
        this IHostApplicationBuilder builder,
        SwaggerConfig config,
        bool enableValidator = true)
    {
        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<MessageValidator>();
            options.Interceptors.Add<AddInsLog>();
            options.Interceptors.Add<FriendlyException>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();

        if (enableValidator)
            builder.Services.AddValidators();

        builder.ConfigureSwagger(config, true);

        return builder;
    }

    /// <summary>
    ///     使用Grpc修饰
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static WebApplication UseGrpcModify(this WebApplication app, SwaggerConfig config)
    {
        app.MapSwagger(config, true);

        return app;
    }
}