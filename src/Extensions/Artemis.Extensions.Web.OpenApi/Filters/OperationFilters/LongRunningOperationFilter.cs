using System.Text.RegularExpressions;
using Artemis.Extensions.Web.OpenApi.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Web.OpenApi.Filters.OperationFilters;

/// <summary>
///     添加<see cref="LongRunningOperationAttribute" />标识的x-ms-long-running-operation扩展
/// </summary>
public sealed class LongRunningOperationFilter : IOperationFilter
{
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

        //添加x-ms-long-running-operation扩展的POST操作状态码只能时200、201、204.
        //响应202的操作不能标记`x-ms-long-running-operation`.
        var longRunningOperationAttributes =
            context.ApiDescription.CustomAttributes().OfType<LongRunningOperationAttribute>().ToList();
        if (!longRunningOperationAttributes.Any()) return;

        // 设置操作为长操作
        operation.Extensions.Add("x-ms-long-running-operation", new OpenApiBoolean(true));

        // 查找最终状态标识
        var finalStateVia = PascalCaseToKebabCase(longRunningOperationAttributes.First().FinalStateVia.ToString());

        // 添加选项
        operation.Extensions.Add("x-ms-long-running-operation-options", new OpenApiObject
        {
            { "final-state-via", new OpenApiString(finalStateVia) }
        });
    }

    #endregion

    private static string PascalCaseToKebabCase(string pascalCaseString)
    {
        return Regex.Replace(pascalCaseString, "(?<=[a-z])([A-Z])", "-$1", RegexOptions.Compiled).ToLowerInvariant();
    }
}