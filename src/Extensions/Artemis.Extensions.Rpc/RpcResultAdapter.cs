using Artemis.Data.Core;
using Mapster;

namespace Artemis.Extensions.Rpc;

/// <summary>
///     Rpc结果适配器
/// </summary>
public static class RpcResultAdapter
{
    /// <summary>
    ///     成功结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult Success<TResult>()
    {
        return DataResult.AdapterSuccess().Adapt<TResult>();
    }

    /// <summary>
    ///     成功结果
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <param name="data">载荷数据</param>
    /// <returns></returns>
    public static TResult Success<TResult, TData>(TData data)
    {
        return DataResult.AdapterSuccess(data).Adapt<TResult>();
    }

    /// <summary>
    ///     失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult Fail<TResult>(string message = "失败")
    {
        return DataResult.AdapterFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult Fail<TResult, TData>(string message = "失败")
    {
        return DataResult.AdapterFail<TData>(message).Adapt<TResult>();
    }

    /// <summary>
    ///     认证失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult AuthFail<TResult>(string message = "认证失败")
    {
        return DataResult.AdapterAuthFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     校验失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult ValidateFail<TResult>(string message = "校验失败")
    {
        return DataResult.AdapterValidateFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     异常结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static TResult Exception<TResult>(Exception exception)
    {
        return DataResult.AdapterException(exception, message: exception.Message).Adapt<TResult>();
    }
}