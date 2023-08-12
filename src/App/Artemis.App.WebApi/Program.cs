using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;

namespace Artemis.App.WebApi;

/// <summary>
///     Program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var generateInternalSwagger = Environment.GetCommandLineArgs().Contains("--internal-swagger"); // 生成开发环境Swagger

        var generateExternalSwagger = !generateInternalSwagger; // 生成生产环境Swagger

        var documentConfig = new DocumentConfig
        {
            Title = "Artemis",
            Description = "Artemis",
            ClientName = "Artemis",
            SupportedApiVersions = new[] { "2021-09-01-preview", "2022-01-01-preview", "2021-10-01" },
            TypeSchemaMapping = new Dictionary<Type, OpenApiSchema>
            {
                { typeof(ODataQueryOptions<>), new OpenApiSchema() }
            },
            GenerateExternalSwagger = generateExternalSwagger
        };

        LogHost.CreateWebApp(args,
            builder =>
            {
                // Add services to the container.
                builder.Services.AddControllers();

                // OpenApi
                builder.AddOpenApiDoc(documentConfig);
            },
            app =>
            {
                // Configure the HTTP request pipeline.
                app.UseOpenApiDoc(documentConfig);

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            });
    }
}