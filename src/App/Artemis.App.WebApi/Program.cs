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
        var documentConfig = new DocumentConfig();

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