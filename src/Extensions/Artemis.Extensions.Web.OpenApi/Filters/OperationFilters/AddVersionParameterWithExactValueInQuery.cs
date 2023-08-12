using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.OperationFilters;

/// <summary>
///     在查询中添加具有确切值的版本参数
/// </summary>
public sealed class AddVersionParameterWithExactValueInQuery : IOperationFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="operation">OpenApiOperation</param>
    /// <param name="context">OperationFilterContext</param>
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