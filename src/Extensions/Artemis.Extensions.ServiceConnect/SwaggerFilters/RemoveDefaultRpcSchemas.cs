using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters;

/// <summary>
///     移除默认生成的模式
/// </summary>
public class RemoveDefaultRpcSchemas : IDocumentFilter
{
    #region Implementation of IDocumentFilter

    /// <summary>
    ///     执行
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Components.Schemas.Remove("Any");
        swaggerDoc.Components.Schemas.Remove("Status");
    }

    #endregion
}