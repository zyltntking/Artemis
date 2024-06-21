using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Artemis.Extensions.ServiceConnect.Authorization;

/// <summary>
/// Rpc授权中间件结果处理器
/// </summary>
public class RpcAuthorizationMiddlewareResultHandler : AuthorizationMiddlewareResultHandler
{
    #region Overrides of AuthorizationResultTransformer

    /// <summary>
    ///     失败处理器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override Task FailHandler(HttpContext context)
    {
        if (context.Request.ContentType == "application/grpc")
            throw new RpcException(new Status(StatusCode.PermissionDenied, AuthorizationMessage!), AuthorizationMessage!);
        //var result = RpcResultAdapter.AuthFail<EmptyResponse>(AuthMessage!);
        //var bytes = result.ToByteArray();
        //context.Response.ContentType = "application/grpc";
        //context.Response.BodyWriter.WriteAsync(bytes);
        //return Task.CompletedTask;
        return base.FailHandler(context);
    }

    #endregion
}