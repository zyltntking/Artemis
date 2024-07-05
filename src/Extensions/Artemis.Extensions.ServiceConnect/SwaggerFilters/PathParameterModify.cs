using Artemis.Extensions.ServiceConnect.SwaggerFilters.Descriptor;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     参数描述修饰
/// </summary>
public class ParameterDescriptionModify : IOperationFilter
{
    #region Implementation of IOperationFilter

    /// <summary>
    ///     应用
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = operation.Parameters
            .Where(item => item.In == ParameterLocation.Path)
            .ToList();

        if (parameters.Any())
            foreach (var parameter in parameters)
            {
                var description = parameter.Description;

                if (!string.IsNullOrWhiteSpace(description))
                {
                    var comments = description.Split("\r\n");

                    var schemaInfo = CommentDescriptor.Describe(comments);

                    parameter.Description = schemaInfo.Description ?? string.Empty;
                }
            }
    }

    #endregion
}