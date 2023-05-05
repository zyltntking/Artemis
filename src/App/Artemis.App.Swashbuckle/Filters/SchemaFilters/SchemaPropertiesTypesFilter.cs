using System.Reflection;
using Artemis.App.Swashbuckle.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.SchemaFilters;

/// <summary>
///     架构属性类型过滤器
/// </summary>
public class SchemaPropertiesTypesFilter : ISchemaFilter
{
    /// <summary>
    ///     应用
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var props = context.Type.GetProperties();
        foreach (var prop in props)
        {
            var typeAtt = prop.GetCustomAttribute<SwaggerTypeAttribute>();
            if (typeAtt != null)
            {
                var schemaProp = schema.Properties.FirstOrDefault(kvp => kvp.Key.ToLower() == prop.Name.ToLower());
                if (schemaProp.Value != null) schemaProp.Value.Type = typeAtt.TypeName;
            }
        }
    }
}