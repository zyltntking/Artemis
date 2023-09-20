using System.Net;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Middleware;

/// <summary>
///     异常结果中间件
/// </summary>
public class ExceptionResultMiddleware : IMiddleware
{
    /// <summary>
    ///     中间件构造
    /// </summary>
    /// <param name="logger">日志依赖</param>
    public ExceptionResultMiddleware(ILogger<ExceptionResultMiddleware> logger)
    {
        Logger = logger;
    }

    #region Implementation of IMiddleware

    /// <summary>Request handling method.</summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the execution of this middleware.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            Logger.LogInformation("格式化异常信息...");
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            var result = DataResult.Exception(exception);
            if (exception is ArtemisException artemisException)
            {
                result.Code = artemisException.ErrorCode;
                result.Error = artemisException.ErrorMessage;
            }

            await context.Response.WriteAsJsonAsync(result);
        }
    }

    #endregion

    /// <summary>
    ///     日志
    /// </summary>
    private ILogger<ExceptionResultMiddleware> Logger { get; }
}