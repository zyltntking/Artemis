using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.DocumentFilters;

/// <summary>
///     添加文档级别的Host属性('host')
/// </summary>
public sealed class AddHostFilter : IDocumentFilter
{
    private readonly DocumentConfig _config;

    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="config"></param>
    public AddHostFilter(DocumentConfig config)
    {
        _config = config;
    }

    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="openApiDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
    {
        if (!string.IsNullOrEmpty(_config.DefaultHostName))
            openApiDoc.Extensions.Add("host", new OpenApiString(_config.DefaultHostName));
    }
}