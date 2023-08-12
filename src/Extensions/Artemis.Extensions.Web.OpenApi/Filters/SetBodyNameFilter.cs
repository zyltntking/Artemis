using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters;

/// <summary>
///     设置Body名称
/// </summary>
public sealed class SetBodyNameFilter : IRequestBodyFilter
{
    #region Implementation of IRequestBodyFilter

    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="requestBody">OpenApiRequestBody</param>
    /// <param name="context">RequestBodyFilterContext</param>
    public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
    {
        var parameterInfo = context?.BodyParameterDescription?.ParameterInfo();

        if (parameterInfo != null) requestBody?.Extensions.Add("x-bodyName", new OpenApiString(parameterInfo.Name));
    }

    #endregion
}