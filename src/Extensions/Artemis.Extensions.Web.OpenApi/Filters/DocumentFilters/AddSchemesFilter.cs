using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;

/// <summary>
///     添加文档级别的Schemes属性('schemes')
/// </summary>
/// <remarks>默认设置Http和Https</remarks>
public sealed class AddSchemesFilter : IDocumentFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="openApiDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        var schemes = new OpenApiArray
        {
            new OpenApiString(Uri.UriSchemeHttps),
            new OpenApiString(Uri.UriSchemeHttp)
        };
        openApiDoc.Extensions.Add("schemes", schemes);
    }
}