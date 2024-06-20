using Artemis.Extensions.ServiceConnect.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureGrpc(this IHostApplicationBuilder builder)
    {
        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<MessageValidator>();
            options.Interceptors.Add<FriendlyException>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
        });

        return builder;
    }

    /// <summary>
    ///     使用Grpc
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseGrpc(this WebApplication app)
    {
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            app.MapGrpcReflectionService();
        }

        return app;
    }
}