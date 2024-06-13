﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artemis.Extensions.Rpc;

/// <summary>
///     移除默认响应
/// </summary>
public class RemoveDefaultResponse : IOperationFilter
{
    #region Implementation of IOperationFilter

    /// <summary>
    ///     实现
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Remove("default");
    }

    #endregion
}