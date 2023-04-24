using System.Reflection;
using Artemis.App.Swashbuckle.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.App.Swashbuckle.Filters.OperationFilters;

/// <summary>
/// A conditional operation filter to hide an API paramater .
/// </summary>
public class HideParamInDocsFilter : IOperationFilter
{
    private readonly bool _hideParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="HideParamInDocsFilter"/> class.
    /// </summary>
    public HideParamInDocsFilter(bool hideParameters = false)
    {
        _hideParameters = hideParameters;
    }

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (_hideParameters)
        {
            foreach (var parameter in context.MethodInfo.GetParameters())
            {
                var isParamHidden = parameter.GetCustomAttributes<SwaggerHideParameterAttribute>().Any();
                if (isParamHidden)
                {
                    var name = parameter.GetCustomAttributes<FromHeaderAttribute>().FirstOrDefault()?.Name ??
                               parameter.GetCustomAttributes<FromQueryAttribute>().FirstOrDefault()?.Name ??
                               parameter.Name;

                    var p = operation.Parameters.FirstOrDefault(p => p.Name?.ToLower() == name?.ToLower());
                    operation.Parameters.Remove(p);
                }
            }
        }
    }
}