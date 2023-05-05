using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.DocumentFilters;

/// <summary>
///     Adds document level property 'schemes' and sets its value to https.
/// </summary>
public class AddSchemesFilter : IDocumentFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="swaggerDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var schemes = new OpenApiArray
        {
            new OpenApiString(Uri.UriSchemeHttps)
        };
        swaggerDoc.Extensions.Add("schemes", schemes);
    }
}