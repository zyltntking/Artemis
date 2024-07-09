using Artemis.Data.Core;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
///     授权中间件结果处理器
/// </summary>
internal class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    #region Implementation of IAuthorizationMiddlewareResultHandler

    /// <summary>
    ///     Evaluates the authorization requirement and processes the authorization result.
    /// </summary>
    /// <param name="next">
    ///     The next middleware in the application pipeline. Implementations may not invoke this if the authorization did not
    ///     succeed.
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
        AuthorizationMessage = context.Items[SharedKey.AuthorizationMessage] as string;

        if (!authorizeResult.Succeeded)
            return FailHandler(context);

        return next(context);
    }

    #endregion

    /// <summary>
    ///     认证消息
    /// </summary>
    protected string? AuthorizationMessage { get; set; }

    /// <summary>
    ///     失败处理器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task FailHandler(HttpContext context)
    {
        var result = DataResult.AuthFail(AuthorizationMessage ?? string.Empty);

        if (context.Request.ContentType == "application/grpc")
            throw new RpcException(new Status(StatusCode.PermissionDenied, AuthorizationMessage!),
                AuthorizationMessage!);

        return context.Response.WriteAsJsonAsync(result);
    }
}