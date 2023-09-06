using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.Web.Builder;

/// <summary>
///     路由表应用创建扩展
/// </summary>
public static class ApiRoutesTableBuilderExtensions
{
    /// <summary>
    ///     映射路由表
    /// </summary>
    /// <param name="app">Web应用</param>
    /// <param name="pattern">访问路径</param>
    /// <returns></returns>
    public static WebApplication MapApiRouteTable(this WebApplication app, string pattern = "route-table")
    {
        if (app.Environment.IsDevelopment())
            app.MapGet(pattern, (IEnumerable<EndpointDataSource> endpointSources, HttpContext context) =>
            {
                var domain = context.Items[SharedKeys.DomainKey] as string;

                var endpoints = endpointSources
                    .SelectMany(es => es.Endpoints);

                var routeItemList = new List<RouteItem>();

                foreach (var endpoint in endpoints)
                {
                    if (endpoint is RouteEndpoint routeEndpoint)
                    {

                        var apiControllerMetadata = routeEndpoint
                            .Metadata
                            .OfType<ApiControllerAttribute>()
                            .FirstOrDefault();

                        if (apiControllerMetadata != null)
                        {
                            var httpMethodsMetadata = routeEndpoint
                                .Metadata
                                .OfType<HttpMethodMetadata>()
                                .FirstOrDefault();

                            var methods = httpMethodsMetadata?.HttpMethods;

                            var displayMethods = methods == null ? string.Empty : string.Join("|", methods);

                            var displayPath = routeEndpoint.RoutePattern.RawText ?? string.Empty;

                            displayPath = $"{domain}/{displayPath}";

                            var routePatternDefaults = routeEndpoint.RoutePattern.Defaults;

                            string? controller, action;
                            controller = routePatternDefaults[nameof(controller)] as string;
                            action = routePatternDefaults[nameof(action)] as string;

                            var displayAction = $"{domain}.{controller}.{action}";

                            var routeItem = new RouteItem
                            {
                                Action = ActionType.API,
                                Methods = displayMethods,
                                Path = displayPath,
                                DomainAction = displayAction
                            };

                            routeItemList.Add(routeItem);
                        }
                    }
                }

                return routeItemList;
            });

        return app;
    }
}