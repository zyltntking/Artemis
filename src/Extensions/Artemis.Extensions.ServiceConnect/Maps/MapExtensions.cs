using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Artemis.Extensions.ServiceConnect.Maps;

/// <summary>
///     端映射扩展
/// </summary>
public static class MapExtensions
{
    /// <summary>
    ///     映射迁移端点
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static WebApplication MapMigrationEndpoint<TDbContext>(this WebApplication app, string pattern = "/migrate")
        where TDbContext : DbContext
    {
        app.MapGet(pattern, (TDbContext context) =>
        {
            try
            {
                context.Database.Migrate();
            }
            catch
            {
                return "Failed!";
            }

            return "Success!";
        });

        return app;
    }

    /// <summary>
    ///     映射健康检查详细信息
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static WebApplication MapDetailHealthChecks(this WebApplication app, string pattern = "/health-detail")
    {
        app.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        });

        return app;
    }

    /// <summary>
    ///     写入响应
    /// </summary>
    /// <param name="context"></param>
    /// <param name="healthReport"></param>
    /// <returns></returns>
    private static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = healthReport.Status.ToString(),
            errors = healthReport.Entries.Select(entry => new
            {
                key = entry.Key,
                value = new
                {
                    status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                    description = entry.Value.Description,
                    //data = entry.Value.Data,
                    exception = entry.Value.Exception?.Message,
                    tags = entry.Value.Tags
                    //duration = entry.Value.Duration
                }
            })
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}