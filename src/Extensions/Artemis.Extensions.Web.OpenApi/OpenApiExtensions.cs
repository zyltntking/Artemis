using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi扩展
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    ///     添加OpenApi文档
    /// </summary>
    /// <param name="builder">WebApp Builder</param>
    /// <param name="config">配置</param>
    public static IServiceCollection AddOpenApiDoc(this WebApplicationBuilder builder, DocumentConfig config)
    {
        builder.Services.AddEndpointsApiExplorer();

        config.EnsureValidity();

        builder.Services.AddSwaggerGen();

        return builder.Services;
    }

    /// <summary>
    ///     使用OpenApi文档
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config">配置</param>
    public static WebApplication UseOpenApiDoc(this WebApplication app, DocumentConfig config)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseReDoc();
        }

        return app;
    }
}