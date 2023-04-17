using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Artemis.App.Swashbuckle.Attributes;

namespace Artemis.App.Swashbuckle.Filters.SchemaFilters;

/// <summary>
/// Adds "readOnly": true to the property marked by <see cref="ReadOnlyPropertyAttribute"/> in the generated swagger.
/// </summary>
public class AddReadOnlyPropertyFilter : ISchemaFilter
{
    /// <summary>
    /// Applies AddReadOnlyPropertyFilter.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        var readOnlyAttr = context.Type.GetTypeInfo().GetCustomAttribute<ReadOnlyPropertyAttribute>();
        if (readOnlyAttr is { IsReadOnlyProperty: true })
        {
            schema.ReadOnly = true;
        }

        foreach (var schemaProperty in schema.Properties)
        {
            PropertyInfo? property;
            try
            {
                property = context.Type.GetProperty(schemaProperty.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            catch (AmbiguousMatchException)
            {
                // we do this to support overrides on inhrited (properties with new keyword)
                property = context.Type.GetProperty(schemaProperty.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            if (property != null)
            {
                var attr = property.GetCustomAttributes(typeof(ReadOnlyPropertyAttribute), false).SingleOrDefault() as ReadOnlyPropertyAttribute;
                if (attr is { IsReadOnlyProperty: true })
                {
                    schemaProperty.Value.ReadOnly = true;
                }
            }
        }
    }
}