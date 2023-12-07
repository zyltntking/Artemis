using Artemis.Data.Core;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Artemis.App.Identity.Interceptors;

/// <summary>
///     Token侦听器
/// </summary>
public class TokenInterceptor : Interceptor
{

    /// <summary>
    /// 日志依赖
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// 侦听器构造
    /// </summary>
    /// <param name="logger"></param>
    public TokenInterceptor(ILogger<TokenInterceptor> logger)
    {
        Logger = logger;
    }


    #region Overrides of Interceptor

    /// <summary>
    ///     Server-side handler for intercepting and incoming unary call.
    /// </summary>
    /// <typeparam name="TRequest">Request message type for this method.</typeparam>
    /// <typeparam name="TResponse">Response message type for this method.</typeparam>
    /// <param name="request">The request value of the incoming invocation.</param>
    /// <param name="context">
    ///     An instance of <see cref="T:Grpc.Core.ServerCallContext" /> representing
    ///     the context of the invocation.
    /// </param>
    /// <param name="continuation">
    ///     A delegate that asynchronously proceeds with the invocation, calling
    ///     the next interceptor in the chain, or the service request handler,
    ///     in case of the last interceptor and return the response value of
    ///     the RPC. The interceptor can choose to call it zero or more times
    ///     at its discretion.
    /// </param>
    /// <returns>
    ///     A future representing the response value of the RPC. The interceptor
    ///     can simply return the return value from the continuation intact,
    ///     or an arbitrary response value as it sees fit.
    /// </returns>
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Logger.LogInformation($"客户端标识：{context.Peer}");

        var requestJson = request.Serialize();

        Logger.LogInformation($"请求参数：{requestJson}");

        var response = continuation(request, context);

        var responseJson = response.Serialize();

        Logger.LogInformation($"响应结果：{responseJson}");

        return response;
    }

    #endregion
}