using Artemis.Extensions.ServiceConnect.Authorization;
using Artemis.Service.Shared.Transfer;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.ServiceConnect.Interceptors;

/// <summary>
///     日志加载项
/// </summary>
internal sealed class AddInsLog : AddInsLog<TokenDocument>
{
    /// <summary>
    ///     侦听器构造
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public AddInsLog(
        ILogger<AddInsLog> logger,
        IOptions<ArtemisAuthorizationOptions> options) : base(logger, options)
    {
    }

    #region Overrides of AddInsLog<TokenDocument>

    /// <summary>
    ///     解析Token文档到字符串
    /// </summary>
    /// <param name="tokenDocument"></param>
    /// <returns></returns>
    protected override string ResolveTokenDocument(TokenDocument? tokenDocument)
    {
        if (tokenDocument == null) return string.Empty;

        var userName = tokenDocument.UserName;

        var userId = tokenDocument.UserId;

        return $"用户标识：{userId}，用户名称：{userName}";
    }

    #endregion
}

/// <summary>
///     日志加载项
/// </summary>
internal abstract class AddInsLog<TTokenDocument> : Interceptor where TTokenDocument : class
{
    /// <summary>
    ///     侦听器构造
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public AddInsLog(
        ILogger logger,
        IOptions<ArtemisAuthorizationOptions> options)
    {
        Logger = logger;
        Options = options.Value;
    }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger Logger { get; }

    private ArtemisAuthorizationOptions Options { get; }

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
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var httpContext = context.GetHttpContext();

        var endpoint = httpContext.GetEndpoint();

        var document = httpContext.FetchTokenDocument<TTokenDocument>(Options.ContextItemTokenKey);

        var documentResolved = ResolveTokenDocument(document);

        var description = "未知操作";

        if (endpoint is RouteEndpoint routeEndpoint)
            description = routeEndpoint.FetchDescription();

        var logInfo = $"标识文档：{documentResolved}，客户端标识：{context.Peer}, 操作: {context.Method}, 描述：{description}";

        Logger.LogInformation(logInfo);

        return continuation(request, context);
    }

    #endregion

    /// <summary>
    ///     解析Token文档到字符串
    /// </summary>
    /// <param name="tokenDocument"></param>
    /// <returns></returns>
    protected abstract string ResolveTokenDocument(TTokenDocument? tokenDocument);
}