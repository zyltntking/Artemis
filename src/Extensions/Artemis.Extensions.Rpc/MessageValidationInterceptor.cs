﻿using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Rpc;

/// <summary>
/// 消息体验证拦截器
/// </summary>
public class MessageValidationInterceptor : Interceptor
{
    /// <summary>
    ///     侦听器构造
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="logger"></param>
    public MessageValidationInterceptor(IServiceProvider provider, ILogger<MessageValidationInterceptor> logger)
    {
        Provider = provider;
        Logger = logger;
    }

    /// <summary>
    /// 服务提供程序
    /// </summary>
    private IServiceProvider Provider { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// 侦听一元调用
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
        Logger.LogInformation("验证请求消息");

        var result = await base.UnaryServerHandler(request, context, continuation);

        return result;
    }

    /// <summary>
    /// 验证请求消息
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <exception cref="RpcException"></exception>
    private void Validate<TRequest>(TRequest request)
    {
        if (TryGetValidator<TRequest>(out var validator))
        {
            var result = validator!.Validate(request);

            if (!result.IsValid && result.Errors.Any())
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.ToString()));
            }
        }
    }

    /// <summary>
    /// 获取验证器
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="validator"></param>
    /// <returns></returns>
    private bool TryGetValidator<TRequest>(out IValidator<TRequest>? validator)
    {
        return (validator = Provider.GetService<IValidator<TRequest>>()) is not null;
    }
}