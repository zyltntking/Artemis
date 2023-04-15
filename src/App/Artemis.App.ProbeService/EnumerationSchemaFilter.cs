using Artemis.Data.Core.Fundamental;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.ProbeService;

public class EnumerationSchemaFilter : ISchemaFilter
{
    #region Implementation of ISchemaFilter

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        if (type.BaseType == typeof(Enumeration))
        {
            schema.Type = type.Name;

            foreach (var item in Enumeration.GetAll(type))
            {
                schema.Enum.Add(new OpenApiString(item));
            }

        }

    }

    #endregion
}