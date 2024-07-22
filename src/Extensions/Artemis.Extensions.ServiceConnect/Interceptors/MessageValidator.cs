using Artemis.Data.Core;
using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.ServiceConnect.Interceptors;

/// <summary>
///     消息校验侦听器
/// </summary>
internal sealed class MessageValidator : Interceptor
{
    /// <summary>
    ///     侦听器构造
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="logger"></param>
    public MessageValidator(IServiceProvider provider, ILogger<MessageValidator> logger)
    {
        Provider = provider;
        Logger = logger;
    }

    /// <summary>
    ///     服务提供程序
    /// </summary>
    private IServiceProvider Provider { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    ///     侦听一元调用
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Logger.LogInformation("验证请求消息");

        if (TryGetValidator<TRequest>(out var validator))
        {
            var validateResult = validator!.Validate(request);

            if (!validateResult.IsValid && validateResult.Errors.Any())
            {
                var dictionary = new Dictionary<string, List<string>>();

                foreach (var error in validateResult.Errors)
                    if (dictionary.ContainsKey(error.PropertyName))
                        dictionary[error.PropertyName].Add(error.ErrorMessage);
                    else
                        dictionary.Add(error.PropertyName, [error.ErrorMessage]);

                var response = ResultAdapter.AdaptValidateFail<TResponse>(dictionary.Serialize());

                return Task.FromResult(response);
            }
        }

        return continuation(request, context);
    }

    /// <summary>
    ///     获取验证器
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="validator"></param>
    /// <returns></returns>
    private bool TryGetValidator<TRequest>(out IValidator<TRequest>? validator)
    {
        return (validator = Provider.GetService<IValidator<TRequest>>()) is not null;
    }
}