using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     Swagger扩展
/// </summary>
internal static class SwaggerExtensions
{
    /// <summary>
    ///     配置Grpc
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <param name="grpcSwagger"></param>
    /// <returns></returns>
    internal static IHostApplicationBuilder ConfigureSwagger(
        this IHostApplicationBuilder builder,
        SwaggerConfig config,
        bool grpcSwagger = false)
    {
        if (grpcSwagger) builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = config.AppName,
                Version = "v1"
            });

            foreach (var xmlDoc in config.XmlDocs)
                if (Path.Exists(xmlDoc))
                {
                    options.IncludeXmlComments(xmlDoc);
                    if (grpcSwagger) options.IncludeGrpcXmlComments(xmlDoc, true);
                }

            if (grpcSwagger)
            {
                options.OperationFilter<RemoveDefaultRpcResponse>();
                options.DocumentFilter<RemoveDefaultRpcSchemas>();
                options.OperationFilter<AddAuthorizationToken>();
                //config.OperationFilter<MarkFieldFeature>();
            }
        });

        return builder;
    }

    /// <summary>
    ///     映射Swagger端
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config"></param>
    /// <param name="grpcSwagger"></param>
    /// <returns></returns>
    internal static WebApplication MapSwagger(
        this WebApplication app,
        SwaggerConfig config,
        bool grpcSwagger = false)
    {
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", config.AppName); });
            if (grpcSwagger) app.MapGrpcReflectionService();
        }
        app.UseReDoc(options =>
        {
            options.RoutePrefix = "api-docs";
            options.SpecUrl("/swagger/v1/swagger.json");
            options.DocumentTitle = config.AppName;
        });

        return app;
    }
}

/// <summary>
///     Swagger配置
/// </summary>
public record SwaggerConfig
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