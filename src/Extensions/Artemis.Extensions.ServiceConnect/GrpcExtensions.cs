using Artemis.Extensions.ServiceConnect.Interceptors;
using Artemis.Extensions.ServiceConnect.SwaggerFilters;
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
    /// <param name="config"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureGrpc(this IHostApplicationBuilder builder, GrpcSwaggerConfig config)
    {
        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<MessageValidator>();
            options.Interceptors.Add<FriendlyException>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = config.AppName,
                    Version = "v1"
                });

            foreach (var xmlDoc in config.XmlDocs)
                if (Path.Exists(xmlDoc))
                {
                    options.IncludeXmlComments(xmlDoc);
                    options.IncludeGrpcXmlComments(xmlDoc, true);
                }

            options.OperationFilter<RemoveDefaultResponse>();
            options.DocumentFilter<RemoveDefaultSchemas>();
            //options.OperationFilter<AddIdentityToken>();
        });

        return builder;
    }

    /// <summary>
    ///     使用Grpc修饰
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseGrpcModify(this WebApplication app)
    {
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            app.UseReDoc(options =>
            {
                options.RoutePrefix = "api-docs";
                options.SpecUrl("/swagger/v1/swagger.json");
                options.DocumentTitle = "Artemis Identity API";
            });
            app.MapGrpcReflectionService();
        }

        return app;
    }
}

/// <summary>
///     Grpc Swagger配置
/// </summary>
public record GrpcSwaggerConfig
{
    /// <summary>
    ///     应用名
    /// </summary>
    public required string AppName { get; init; }

    /// <summary>
    ///     xml文档
    /// </summary>
    public required string[] XmlDocs { get; init; }
}