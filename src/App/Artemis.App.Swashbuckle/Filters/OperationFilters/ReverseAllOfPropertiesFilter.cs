using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.OperationFilters;

/// <summary>
///     Solve known issues with Swashbuckle 5
///     For more details:
///     https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1488 ,
///     https://github.com/Azure/autorest/issues/3417
/// </summary>
public class ReverseAllOfPropertiesFilter : ISchemaFilter
{
    /// <summary>
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        (string key, OpenApiSchema typeSchema)[] allOfProperties = schema.Properties
            .Where(x => x.Value.AllOf is { Count: 1 })
            .Select(x => (x.Key, x.Value.AllOf.First()))
            .ToArray();

        foreach (var (key, typeSchema) in allOfProperties) schema.Properties[key] = typeSchema;
    }
}