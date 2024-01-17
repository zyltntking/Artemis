using Grpc.Core;
using Grpc.Core.Interceptors;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Rpc;

/// <summary>
///     友好异常侦听器
/// </summary>
public class FriendlyExceptionInterceptor : Interceptor
{
    /// <summary>
    ///     侦听器构造
    /// </summary>
    /// <param name="logger"></param>
    public FriendlyExceptionInterceptor(ILogger<FriendlyExceptionInterceptor> logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    ///     侦听一元调用异常
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            Logger.LogInformation("格式化异常信息");

            var result = RpcResultAdapter.Exception<TResponse>(exception);

            var response = Activator.CreateInstance<TResponse>();

            result.Adapt(response);

            return response;
        }
    }
}