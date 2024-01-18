using Artemis.Extensions.Web.Identity;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Artemis.Extensions.Rpc;

/// <summary>
///     Rpc认证结果转换
/// </summary>
public class RpcAuthorizationResultTransformer : AuthorizationResultTransformer
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
            throw new RpcException(new Status(StatusCode.PermissionDenied, AuthMessage!), AuthMessage!);
        //var result = RpcResultAdapter.AuthFail<EmptyResponse>(AuthMessage!);
        //var bytes = result.ToByteArray();
        //context.Response.ContentType = "application/grpc";
        //context.Response.BodyWriter.WriteAsync(bytes);
        //return Task.CompletedTask;
        return base.FailHandler(context);
    }

    #endregion
}