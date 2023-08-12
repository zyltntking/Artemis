using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.OperationFilters;

/// <summary>
///     删除所有被重复添加的api-version参数
/// </summary>
public sealed class RemoveDuplicateApiVersionParameter : IOperationFilter
{
    private const string HttpHeaderNamesApiVersion = "api-version";

    #region Implementation of IOperationFilter

    /// <summary>
    ///     Applies filter.
    /// </summary>
    /// <param name="operation">OpenApiOperation</param>
    /// <param name="context">OperationFilterContext</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));

        if (context is null) throw new ArgumentNullException(nameof(context));

        var apiDescription = context.ApiDescription;

        if (operation.Parameters == null) return;

        var isApiVersionParameterDuplicated = operation.Parameters.Count(p => p.Name == HttpHeaderNamesApiVersion) > 1;
        if (!isApiVersionParameterDuplicated) return;

        var description = apiDescription.ParameterDescriptions.FirstOrDefault(p =>
            p is { Name: HttpHeaderNamesApiVersion, DefaultValue: not null });

        // 创建副本以避免错误"Collection was modified; enumeration operation may not execute"
        var parameterAddedSoFar = false;
        foreach (var parameter in operation.Parameters.OfType<OpenApiParameter>()
                     .Where(p => p.Name == HttpHeaderNamesApiVersion).ToList())
        {
            // 删除重复的api-version查询参数
            if (parameterAddedSoFar)
                operation.Parameters.Remove(parameter);
            // 不添加默认参数
            else if (parameter.Schema.Default == null)
                parameter.Schema.Default = new OpenApiString(description!.DefaultValue!.ToString());

            parameterAddedSoFar = true;
        }
    }

    #endregion
}