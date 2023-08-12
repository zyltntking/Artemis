using System.Net.Mime;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;

/// <summary>
///     添加文档级别的Produce属性('produce')
/// </summary>
/// <remarks>默认设置'application/json'</remarks>
public sealed class AddProducesFilter : IDocumentFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="openApiDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        var produces = new OpenApiArray
        {
            new OpenApiString(MediaTypeNames.Application.Json)
        };
        openApiDoc.Extensions.Add("produces", produces);
    }
}