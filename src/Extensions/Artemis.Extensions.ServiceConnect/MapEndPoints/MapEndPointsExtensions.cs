using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Extensions.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        app.MapGet(pattern,
            (IEnumerable<EndpointDataSource> endpointSources) =>
            {
                var endpointDataSources = endpointSources.ToList();

                var endpoints = endpointDataSources
                    .SelectMany(source => source.Endpoints)
                    .Select(endPoint =>
                    {
                        if (endPoint is RouteEndpoint routeEndpoint)
                        {
                            var path = routeEndpoint.FetchRoutePath() ?? string.Empty;

                            var routeType = path.Contains("/api") ? RouteType.Restful : RouteType.gRpc;

                            return new RouteInfo
                            {
                                RouteType = routeType.Name,
                                Path = path,
                                Description = routeEndpoint.FetchDescription()
                            };
                        }

                        return null;

                    })
                    .Where(route => route != null)
                    .Where(route => route != null && !route.Path.Contains("unimplemented", StringComparison.OrdinalIgnoreCase))
                    .Where(route => route != null && !route.Path.Contains("reflection", StringComparison.OrdinalIgnoreCase))
                    .Where(route => (route != null && route.Path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)) || 
                                    (route != null && route.Path.StartsWith("/Artemis", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                return endpoints.Serialize();
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

/// <summary>
/// 路由信息
/// </summary>
public record RouteInfo
{
    /// <summary>
    /// 路由类型
    /// </summary>
    public required string RouteType { get; init; }

    /// <summary>
    /// 路由路径
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// 路由描述
    /// </summary>
    public string? Description { get; init; }
}