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

        builder.Services.AddSwaggerGen(options =>
        {

            options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

            var xmlCommentFiles = Directory.GetFiles(
                    AppContext.BaseDirectory,
                    "Artemis.*.xml",
                    SearchOption.TopDirectoryOnly)
                .Where(path =>
                    Path.GetFileName(path).Contains("Artemis.Data") ||
                    Path.GetFileName(path).Contains("Artemis.Shared") ||
                    Path.GetFileName(path).Contains("Artemis.App"))
                .ToList();

            foreach (var xmlCommentFile in xmlCommentFiles) options.IncludeXmlComments(xmlCommentFile, true);
        });

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