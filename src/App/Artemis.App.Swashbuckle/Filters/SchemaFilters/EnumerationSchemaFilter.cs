using Artemis.Data.Core.Fundamental;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.SchemaFilters;

/// <summary>
///     枚举类过滤器
/// </summary>
public class EnumerationSchemaFilter : ISchemaFilter
{
    #region Implementation of ISchemaFilter

    /// <summary>
    ///     过滤器执行逻辑
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        if (type.BaseType == typeof(Enumeration))
        {
            schema.Type = type.Name;

            foreach (var item in Enumeration.GetAllName(type)) schema.Enum.Add(new OpenApiString(item));
        }
    }

    #endregion
}