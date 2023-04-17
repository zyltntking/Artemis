using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;
using Artemis.App.Swashbuckle.Attributes;

namespace Artemis.App.Swashbuckle.Filters.SchemaFilters;

/// <summary>
/// 自定义架构属性筛选器
/// </summary>
public class CustomSchemaPropertiesFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var attr = context.Type.GetCustomAttribute<CustomSwaggerSchemaAttribute>();
        if (attr != null && !string.IsNullOrEmpty(attr.DescriptionFormat))
        {
            var humanizedCamelCase = string.Empty;
            if (context.Type.GenericTypeArguments.Any())
            {
                humanizedCamelCase = Regex.Replace(context.Type.GenericTypeArguments.First().Name, "([A-Z])", " $1").Trim();
            }
            schema.Description = attr.DescriptionFormat.Replace("{GenericType}", humanizedCamelCase);
        }
    }
}