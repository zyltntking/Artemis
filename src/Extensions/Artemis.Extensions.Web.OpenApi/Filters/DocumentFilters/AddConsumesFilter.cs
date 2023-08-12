using System.Net.Mime;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;

/// <summary>
///     添加文档级别的Consumes属性(consumes)
/// </summary>
public sealed class AddConsumesFilter : IDocumentFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="openApiDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        var consumes = new OpenApiArray
        {
            new OpenApiString(MediaTypeNames.Application.Json)
        };
        openApiDoc.Extensions.Add("consumes", consumes);
    }
}