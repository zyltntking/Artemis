using Artemis.Data.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
/// 认证中间件结果处理程序
/// </summary>
public class AuthorizationResultTransformer : IAuthorizationMiddlewareResultHandler
{
    /// <summary>
    /// 默认处理程序
    /// </summary>
    private IAuthorizationMiddlewareResultHandler DefaultHandler { get; }

    /// <summary>
    /// 构造
    /// </summary>
    public AuthorizationResultTransformer()
    {
        DefaultHandler = new AuthorizationMiddlewareResultHandler();
    }

    #region Implementation of IAuthorizationMiddlewareResultHandler

    /// <summary>
    /// Evaluates the authorization requirement and processes the authorization result.
    /// </summary>
    /// <param name="next">
    /// The next middleware in the application pipeline. Implementations may not invoke this if the authorization did not succeed.
    /// </param>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" />.</param>
    /// <param name="policy">The <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> for the resource.</param>
    /// <param name="authorizeResult">The result of authorization.</param>
    public Task HandleAsync(
        RequestDelegate next, 
        HttpContext context, 
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded)
        {
            return FailHandler(context);
        }

        return next(context);
    }

    #endregion

    /// <summary>
    /// 失败处理器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task FailHandler(HttpContext context)
    {
        var authMessage = context.Items[SharedKey.AuthMessage] as string;

        var result = DataResult.AdapterAuthFail(authMessage!);

        return context.Response.WriteAsJsonAsync(result);
    }
}