﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
    /// <param name="path"></param>
    /// <returns></returns>
    internal static void ConfigureSwagger(
        this IHostApplicationBuilder builder,
        string path = "swagger.Setting.json")
    {
        builder.Configuration.AddJsonFile(path, true, true);

        var config = builder.Configuration.GetSection("Swagger").Get<SwaggerConfig>();

        if (config != null)
        {
            if (builder.Environment.IsProduction() && !config.EnableInProduction) return;

            if (config.UserGrpc)
                builder.Services.AddGrpcSwagger();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = config.Application,
                    Version = "v1"
                });

                foreach (var xmlDoc in config.XmlDocs)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlDoc);

                    if (Path.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                        if (config.UserGrpc)
                            options.IncludeGrpcXmlComments(xmlPath, true);
                    }
                }

                if (config.UserGrpc)
                {
                    options.OperationFilter<RemoveDefaultRpcResponse>();
                    options.DocumentFilter<RemoveDefaultRpcSchemas>();
                    options.OperationFilter<AddAuthorizationToken>();
                    //config.OperationFilter<MarkFieldFeature>();
                    options.SchemaFilter<GrpcCommentDescriptor>();
                }
            });
        }
    }

    /// <summary>
    ///     映射Swagger端
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    internal static void MapSwagger(
        this WebApplication app)
    {
        var config = app.Configuration.GetSection("Swagger").Get<SwaggerConfig>();

        if (config != null)
        {
            if (app.Environment.IsProduction() && !config.EnableInProduction)
                return;

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = $"{config.Application} Api Docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{config.Application} v1");
                options.DefaultModelsExpandDepth(-1);
                options.ShowCommonExtensions();
            });
            app.UseReDoc(options =>
            {
                options.RoutePrefix = "api-docs";
                options.SpecUrl("/swagger/v1/swagger.json");
                options.DocumentTitle = config.Application;
            });
            if (config.UserGrpc)
                app.MapGrpcReflectionService();
        }
    }
}

/// <summary>
///     Swagger配置
/// </summary>
internal record SwaggerConfig
{
    /// <summary>
    ///     是否是GrpcSwagger
    /// </summary>
    public required bool UserGrpc { get; init; }

    /// <summary>
    ///     是否允许在生产环境中使用
    /// </summary>
    public required bool EnableInProduction { get; init; }

    /// <summary>
    ///     应用名
    /// </summary>
    public required string Application { get; init; }

    /// <summary>
    ///     xml文档
    /// </summary>
    public required string[] XmlDocs { get; init; }
}