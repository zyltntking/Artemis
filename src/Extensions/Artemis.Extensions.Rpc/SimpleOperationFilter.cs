using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Rpc;

/// <summary>
/// 操作过滤器
/// </summary>
public class SimpleOperationFilter : IOperationFilter
{
    #region Implementation of IOperationFilter

    /// <summary>
    ///  实现
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var path = context.ApiDescription.RelativePath;

        if (path == "Account/SignIn")
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Client",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                },
                Description = "客户端类型"
            });
        }
    }

    #endregion
}