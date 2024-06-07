using Artemis.Data.Core.Fundamental;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Rpc;

/// <summary>
/// 标记字段特性
/// </summary>
public class MarkFieldFeature : IOperationFilter
{
    #region Implementation of IOperationFilter

    /// <summary>
    /// 实现
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schemas = context.SchemaRepository.Schemas;

        foreach (var (key, value) in schemas)
        {
            if (value?.Description != null && Matcher.ContentMatcher().IsMatch(value.Description))
            {
                var featuresDes = Matcher.ContentMatcher().Match(value.Description);
                var features = featuresDes.Groups[1].Value.Split(',');
                foreach (var feature in features)
                {
                    if (feature == "required")
                    {
                        value.Nullable = false;
                    }
                }
            }
        }
    }

    #endregion
}