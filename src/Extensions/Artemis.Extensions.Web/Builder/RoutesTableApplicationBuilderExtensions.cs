using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.Web.Builder;

/// <summary>
///     路由表应用创建扩展
/// </summary>
public static class RoutesTableApplicationBuilderExtensions
{
    /// <summary>
    ///     映射路由表
    /// </summary>
    /// <param name="app">Web应用</param>
    /// <param name="pattern">访问路径</param>
    /// <returns></returns>
    public static WebApplication MapRouteTable(this WebApplication app, string pattern = "route-table")
    {
        if (app.Environment.IsDevelopment())
            app.MapGet(pattern, (IEnumerable<EndpointDataSource> endpointSources) =>
            {
                var builder = new StringBuilder();
                var endpoints = endpointSources.SelectMany(es => es.Endpoints);
                var routeInfoList = new List<Tuple<string, string, string>>();
                foreach (var endpoint in endpoints)
                    if (endpoint is RouteEndpoint routeEndpoint)
                    {
                        var actions = new List<string>();

                        var apiControllerMetadata = routeEndpoint.Metadata
                            .OfType<ApiControllerAttribute>().FirstOrDefault();

                        if (apiControllerMetadata != null) actions.Add("API");

                        var pageMetadata = routeEndpoint.Metadata.OfType<PageModelAttribute>()
                            .FirstOrDefault();

                        if (pageMetadata != null) actions.Add("PAGE");

                        var displayActions = string.Join(",", actions);

                        var httpMethodsMetadata = routeEndpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();

                        var methods = httpMethodsMetadata?.HttpMethods;

                        var displayMethods = methods == null ? string.Empty : string.Join("|", methods);

                        var displayPath = routeEndpoint.RoutePattern.RawText ?? string.Empty;

                        var tuple = new Tuple<string, string, string>(displayActions, displayMethods, displayPath);

                        routeInfoList.Add(tuple);
                    }

                builder.AppendLine("ACTION\t\tMETHODS\t\tPATH");
                foreach (var (action, methods, path) in routeInfoList)
                    builder.AppendLine($"{action}\t\t{methods}\t\t/{path}");

                return builder.ToString();
            });

        return app;
    }
}