using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.ServiceConnect.MapEndPoints;

/// <summary>
///     端映射扩展
/// </summary>
public static class MapEndPointsExtensions
{
    /// <summary>
    ///     映射迁移端点
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static void MapMigrationEndpoint<TDbContext>(this WebApplication app, string pattern = "/migrate")
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
    }

    /// <summary>
    ///     映射路由表
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static void MapRouteTable(this WebApplication app, string pattern = "/route-table")
    {
        if (app.Environment.IsDevelopment())
            app.MapGet(pattern,
                (IEnumerable<EndpointDataSource> endpointSources) =>
                {
                    return string.Join("\n", endpointSources.SelectMany(source => source.Endpoints));
                });
    }

    /// <summary>
    ///     映射健康检查详细信息
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    internal static void MapDetailHealthChecks(this WebApplication app, string pattern = "/health-detail")
    {
        app.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        });
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
                    exception = entry.Value.Exception?.Message,
                    tags = entry.Value.Tags
                }
            })
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}