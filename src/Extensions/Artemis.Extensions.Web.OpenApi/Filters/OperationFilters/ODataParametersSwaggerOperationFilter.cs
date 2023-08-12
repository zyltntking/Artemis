using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.OperationFilters;

/// <summary>
///     为OData参数添加文档标识
/// </summary>
public sealed class ODataParametersSwaggerOperationFilter : IOperationFilter
{
    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="operation">OpenApiOperation</param>
    /// <param name="context">OperationFilterContext</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var odataParam = context.MethodInfo.GetParameters().FirstOrDefault(p =>
            p.ParameterType.BaseType!.FullName == typeof(ODataQueryOptions<>).FullName);
        if (odataParam != null)
        {
            var p = operation.Parameters.FirstOrDefault(p => p.Name.ToLower() == odataParam.Name!.ToLower());
            operation.Parameters.Remove(p);
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$filter",
                Description = "Filter the results using OData syntax.",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$orderby",
                Description = "Order the results using OData syntax.",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$skipToken",
                Description = "The continuation token for scorlling the items",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$top",
                Description = "The number of results to return.",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "integer" }
            });
            // $skip - 不再支持
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "$skip",
            //    Description = "The number of results to skip.",
            //    Required = false,
            //    In = ParameterLocation.Query,
            //    Schema = new OpenApiSchema { Type = "integer" }
            //});
            // $count - 不再支持
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "$count",
            //    Description = "Return the total count.",
            //    Required = false,
            //    In = ParameterLocation.Query,
            //    Schema = new OpenApiSchema { Type = "boolean" }
            //});
        }
    }
}