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
    /// <param name="enableValidator"></param>
    /// <returns></returns>
    public static void ConfigureGrpc(
        this IHostApplicationBuilder builder,
        bool enableValidator = true)
    {
        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            if (enableValidator) options.Interceptors.Add<MessageValidator>();
            options.Interceptors.Add<AddInsLog>();
            options.Interceptors.Add<FriendlyException>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();

        if (enableValidator)
            builder.Services.AddValidators();

        builder.ConfigureSwagger();
    }

    /// <summary>
    ///     使用Grpc修饰
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static void UseGrpcSwagger(this WebApplication app)
    {
        app.MapSwagger();
    }
}