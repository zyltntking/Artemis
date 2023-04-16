using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.DocumentFilters;

/// <summary>
/// Adds document level property host and sets its value to ArmHostName.
/// </summary>
public class AddHostFilter : IDocumentFilter
{
    private readonly string? _hostname;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hostname"></param>
    public AddHostFilter(string? hostname)
    {
        _hostname = hostname;
    }

    /// <summary>
    /// Applies filter.
    /// </summary>
    /// <param name="swaggerDoc">OpenApiDocument.</param>
    /// <param name="context">DocumentFilterContext.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (!string.IsNullOrEmpty(_hostname))
        {
            swaggerDoc.Extensions.Add("host", new OpenApiString(_hostname));
        }
    }
}