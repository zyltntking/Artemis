using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.OperationFilters;

/// <summary>
///     添加版本参数
/// </summary>
public class AddVersionParameterWithExactValueInQuery : IOperationFilter
{
    /// <summary>
    ///     应用
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var existingApiVersion = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
        if (existingApiVersion == null)
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Query,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    MinLength = 1
                }
            });
    }
}