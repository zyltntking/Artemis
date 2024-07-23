using Mapster;

namespace Artemis.Data.Core;

/// <summary>
///     结果适配器
/// </summary>
public static class ResultAdapter
{
    /// <summary>
    ///     成功结果
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <param name="data">载荷数据</param>
    /// <param name="config">映射配置</param>
    /// <returns></returns>
    public static TResult AdaptSuccess<TResult, TData>(TData data, TypeAdapterConfig? config = null)
    {
        if (config is null) return DataResult.Success(data).Adapt<TResult>();

        return DataResult.Success(data).Adapt<TResult>(config);
    }

    /// <summary>
    ///     成功结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult AdaptEmptySuccess<TResult>()
    {
        return DataResult.NullSuccess().Adapt<TResult>();
    }

    /// <summary>
    ///     失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult AdaptFail<TResult, TData>(string message = "失败")
    {
        return DataResult.Fail<TData>(message).Adapt<TResult>();
    }

    /// <summary>
    ///     失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult AdaptEmptyFail<TResult>(string message = "失败")
    {
        return DataResult.NullFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     认证失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult AdaptAuthFail<TResult>(string message = "认证失败")
    {
        return DataResult.AuthFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     校验失败结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static TResult AdaptValidateFail<TResult>(string message = "校验失败")
    {
        return DataResult.ValidateFail(message).Adapt<TResult>();
    }

    /// <summary>
    ///     异常结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static TResult AdaptException<TResult>(Exception exception)
    {
        return DataResult.NullException(exception, message: exception.Message).Adapt<TResult>();
    }
}