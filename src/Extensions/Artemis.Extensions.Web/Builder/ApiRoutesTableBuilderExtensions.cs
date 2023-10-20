using System.ComponentModel;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
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
    public static WebApplication MapApiRouteTable(this WebApplication app, string pattern = "api-route-table")
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
                        var pageModel = routeEndpoint
                            .Metadata
                            .OfType<PageModelAttribute>()
                            .Any();

                        var apiAction = routeEndpoint
                            .Metadata
                            .OfType<ApiControllerAttribute>()
                            .Any();

                        var grpcAction = routeEndpoint
                            .Metadata
                            .OfType<GrpcMethodMetadata>()
                            .Any();

                        if (pageModel)
                        {
                            var displayPath = routeEndpoint.RoutePattern.RawText ?? string.Empty;

                            displayPath = $"{domain}:{displayPath}";

                            var actionParts = routeEndpoint.RoutePattern
                                .PathSegments
                                .SelectMany(item => item.Parts)
                                .Select(item =>
                                {
                                    return item switch
                                    {
                                        RoutePatternLiteralPart literalPart => literalPart.Content,
                                        RoutePatternParameterPart parameterPart => parameterPart.Name,
                                        RoutePatternSeparatorPart separatorPart => separatorPart.Content,
                                        _ => string.Empty
                                    };
                                });

                            var displayAction = $"{domain}:{string.Join(".", actionParts)}";

                            var descriptionMetadata = routeEndpoint
                                .Metadata
                                .OfType<DescriptionAttribute>()
                                .FirstOrDefault();

                            var routeItem = new RouteItem
                            {
                                Action = ActionType.PAGE,
                                Methods = ActionType.PAGE,
                                Path = displayPath,
                                DomainAction = displayAction,
                                Description = descriptionMetadata?.Description
                            };

                            routeItemList.Add(routeItem);
                        }

                        if (apiAction)
                        {
                            var httpMethodsMetadata = routeEndpoint
                                .Metadata
                                .OfType<HttpMethodMetadata>()
                                .FirstOrDefault();

                            var methods = httpMethodsMetadata?.HttpMethods;

                            var displayMethods = methods == null ? string.Empty : string.Join("|", methods);

                            var displayPath = routeEndpoint.RoutePattern.RawText ?? string.Empty;

                            displayPath = $"{domain}:{displayPath}";

                            var actionParts = routeEndpoint
                                .RoutePattern
                                .Defaults
                                .Select(item => item.Value as string);

                            var displayAction = $"{domain}:{string.Join(".", actionParts)}";

                            var descriptionMetadata = routeEndpoint
                                .Metadata
                                .OfType<DescriptionAttribute>()
                                .FirstOrDefault();

                            var routeItem = new RouteItem
                            {
                                Action = ActionType.API,
                                Methods = displayMethods,
                                Path = displayPath,
                                DomainAction = displayAction,
                                Description = descriptionMetadata?.Description
                            };

                            routeItemList.Add(routeItem);
                        }

                        if (grpcAction)
                        {
                            var displayPath = routeEndpoint.RoutePattern.RawText ?? string.Empty;

                            displayPath = $"{domain}:{displayPath}";

                            var actionParts = routeEndpoint.RoutePattern
                                .PathSegments
                                .SelectMany(item => item.Parts)
                                .Select(item =>
                                {
                                    return item switch
                                    {
                                        RoutePatternLiteralPart literalPart => literalPart.Content,
                                        RoutePatternParameterPart parameterPart => parameterPart.Name,
                                        RoutePatternSeparatorPart separatorPart => separatorPart.Content,
                                        _ => string.Empty
                                    };
                                });

                            var displayAction = $"{domain}:{string.Join(".", actionParts)}";

                            var descriptionMetadata = routeEndpoint
                                .Metadata
                                .OfType<DescriptionAttribute>()
                                .FirstOrDefault();

                            var routeItem = new RouteItem
                            {
                                Action = ActionType.GRPC,
                                Methods = ActionType.GRPC,
                                Path = displayPath,
                                DomainAction = displayAction,
                                Description = descriptionMetadata?.Description
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