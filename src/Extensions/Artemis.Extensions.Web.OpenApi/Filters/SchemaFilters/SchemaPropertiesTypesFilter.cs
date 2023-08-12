using System.Reflection;
using Artemis.Extensions.Web.OpenApi.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.SchemaFilters;

/// <summary>
///     模式属性类型筛选
/// </summary>
public sealed class SchemaPropertiesTypesFilter : ISchemaFilter
{
    #region Implementation of ISchemaFilter

    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="schema">OpenApiSchema</param>
    /// <param name="context">SchemaFilterContext</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var props = context.Type.GetProperties();

        foreach (var prop in props)
        {
            var typeAtt = prop.GetCustomAttribute<OpenApiTypeAttribute>();
            if (typeAtt != null)
            {
                var schemaProp = schema.Properties.FirstOrDefault(kvp =>
                    string.Equals(kvp.Key, prop.Name, StringComparison.CurrentCultureIgnoreCase));
                if (schemaProp.Value != null) schemaProp.Value.Type = typeAtt.TypeName;
            }
        }
    }

    #endregion
}