using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core;

#region interface

/// <summary>
///     数据结果协议模板接口
/// </summary>
file interface IResultBase
{
    /// <summary>
    ///     消息码
    /// </summary>
    int Code { get; set; }

    /// <summary>
    ///     操作是否成功
    /// </summary>
    bool Succeeded { get; }

    /// <summary>
    ///     消息
    /// </summary>
    string Message { get; set; }

    /// <summary>
    ///     异常信息
    /// </summary>
    string? Error { get; set; }

    /// <summary>
    ///     本地时间戳
    /// </summary>
    DateTime DateTime { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    long Timestamp { get; set; }
}

/// <summary>
///     数据结果协议模板接口
/// </summary>
/// <typeparam name="T">模板类型</typeparam>
file interface IResultBase<T> : IResultBase
{
    /// <summary>
    ///     数据
    /// </summary>
    T? Data { get; set; }
}

#endregion

/// <summary>
///     抽象数据结果
/// </summary>
public abstract record AbstractResult : IResultBase
{
    #region Implementation of IResultBase

    /// <summary>
    ///     消息码
    /// </summary>
    [Required]
    public required int Code { get; set; }

    /// <summary>
    ///     是否成功
    /// </summary>
    public bool Succeeded => Code == ResultStatus.Success;

    /// <summary>
    ///     消息
    /// </summary>
    [Required]
    public required string Message { get; set; } = string.Empty;

    /// <summary>
    ///     异常信息
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    ///     本地时间戳
    /// </summary>
    [Required]
    public required DateTime DateTime { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    [Required]
    public required long Timestamp { get; set; }

    #endregion
}

/// <summary>
///     数据结果协议模板接口
/// </summary>
/// <typeparam name="T">模板类型</typeparam>
public record DataResult<T> : AbstractResult, IResultBase<T>
{
    #region Implementation of IResultBase<T>

    /// <summary>
    ///     数据
    /// </summary>
    public virtual T? Data { get; set; }

    #endregion

    /// <summary>
    ///     内部异常
    /// </summary>
    private Exception? InnerException { get; set; }

    /// <summary>
    ///     描述器
    /// </summary>
    private Dictionary<string, Collection<string>> Descriptor { get; } = new();

    /// <summary>
    ///     添加描述器
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddDescriptor(string key, string value)
    {
        if (Descriptor.TryGetValue(key, out var collection))
            collection.Add(value);
        else
            Descriptor.Add(key, new Collection<string> { value });
    }

    /// <summary>
    ///     获取描述器
    /// </summary>
    public Dictionary<string, Collection<string>> GetDescriptor()
    {
        return Descriptor;
    }

    /// <summary>
    ///     设置异常
    /// </summary>
    /// <param name="exception">异常</param>
    public void SetException(Exception exception)
    {
        InnerException = exception;
    }

    /// <summary>
    ///     获取异常
    /// </summary>
    /// <returns></returns>
    public Exception? GetException()
    {
        return InnerException;
    }
}

/// <summary>
///     数据结果生成函数集
/// </summary>
public static class DataResult
{
    #region DataResult

    /// <summary>
    ///     生成成功结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>成功结果</returns>
    public static DataResult<T> Success<T>(T data, string message = "成功")
    {
        return GenerateResult(ResultStatus.Success, message, data);
    }

    /// <summary>
    ///     生成成功结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>成功结果</returns>
    public static EmptyResult EmptySuccess(string message = "成功")
    {
        return GenerateEmptyResult(ResultStatus.Success, message);
    }

    /// <summary>
    ///     生成无数据结果
    /// </summary>
    /// <returns></returns>
    public static NullResult NullSuccess(string message = "成功")
    {
        return GenerateNullResult(ResultStatus.Success, message);
    }

    /// <summary>
    ///     生成失败结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="message">消息</param>
    /// <returns>失败结果</returns>
    public static DataResult<T> Fail<T>(string message = "失败")
    {
        return GenerateResult<T>(ResultStatus.Fail, message);
    }

    /// <summary>
    ///     生成失败结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>失败结果</returns>
    public static EmptyResult EmptyFail(string message = "失败")
    {
        return GenerateEmptyResult(ResultStatus.Fail, message);
    }

    /// <summary>
    ///     生成无数据结果
    /// </summary>
    /// <returns></returns>
    public static NullResult NullFail(string message = "失败")
    {
        return GenerateNullResult(ResultStatus.Fail, message);
    }

    /// <summary>
    ///     认证失败
    /// </summary>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    public static NullResult AuthFail(string message = "认证失败")
    {
        return GenerateNullResult(ResultStatus.AuthFail, message);
    }

    /// <summary>
    ///     校验失败
    /// </summary>
    /// <param name="message">失败消息</param>
    /// <returns></returns>
    public static NullResult ValidateFail(string message = "校验失败")
    {
        return GenerateNullResult(ResultStatus.ValidateFail, message);
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">结果消息</param>
    /// <returns></returns>
    public static DataResult<T> Exception<T>(
        Exception exception,
        int code = ResultStatus.Exception,
        string message = "异常")
    {
        return GenerateResult<T>(code, message, default, exception);
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">结果消息</param>
    /// <returns></returns>
    public static EmptyResult EmptyException(
        Exception exception,
        int code = ResultStatus.Exception,
        string message = "异常")
    {
        return GenerateEmptyResult(code, message, exception);
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">结果消息</param>
    /// <returns></returns>
    public static NullResult NullException(
        Exception exception,
        int code = ResultStatus.Exception,
        string message = "异常")
    {
        return GenerateNullResult(code, message, exception);
    }

    /// <summary>
    ///     生成数据结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="code">结果状态编码</param>
    /// <param name="message">结果消息</param>
    /// <param name="data">结果数据</param>
    /// <param name="exception"></param>
    /// <returns>数据结果</returns>
    private static DataResult<T> GenerateResult<T>(
        int code,
        string message,
        T? data = default,
        Exception? exception = null)
    {
        var result = new DataResult<T>
        {
            Code = code,
            Data = data,
            Message = message,
            Error = exception?.ToString(),
            DateTime = DateTime.Now.ToLocalTime(),
            Timestamp = DateTime.Now.ToUnixTimeStamp()
        };

        if (exception != null)
        {
            result.SetException(exception);

            result.AddDescriptor("Exception", exception.ToString());
        }

        return result;
    }

    /// <summary>
    ///     生成空数据结果
    /// </summary>
    /// <param name="code">结果编码</param>
    /// <param name="message">结果消息</param>
    /// <param name="exception">异常信息</param>
    /// <returns></returns>
    private static EmptyResult GenerateEmptyResult(
        int code,
        string message,
        Exception? exception = null)
    {
        var result = new EmptyResult
        {
            Code = code,
            Data = new EmptyRecord(),
            Message = message,
            Error = exception?.ToString(),
            DateTime = DateTime.Now.ToLocalTime(),
            Timestamp = DateTime.Now.ToUnixTimeStamp()
        };

        if (exception != null)
        {
            result.SetException(exception);

            result.AddDescriptor("Exception", exception.ToString());
        }

        return result;
    }

    /// <summary>
    ///     生成无数据结果
    /// </summary>
    /// <param name="code">结果编码</param>
    /// <param name="message">结果消息</param>
    /// <param name="exception">异常信息</param>
    /// <returns></returns>
    private static NullResult GenerateNullResult(
        int code,
        string message,
        Exception? exception = null)
    {
        var result = new NullResult
        {
            Code = code,
            Message = message,
            Error = exception?.ToString(),
            DateTime = DateTime.Now.ToLocalTime(),
            Timestamp = DateTime.Now.ToUnixTimeStamp()
        };

        return result;
    }

    #endregion
}

/// <summary>
///     结果状态
/// </summary>
public static class ResultStatus
{
    /// <summary>
    ///     成功
    /// </summary>
    public const int Success = 0;

    /// <summary>
    ///     失败
    /// </summary>
    public const int Fail = 1;

    /// <summary>
    ///     认证失败
    /// </summary>
    public const int AuthFail = 4;

    /// <summary>
    ///     校验失败
    /// </summary>
    public const int ValidateFail = 5;

    /// <summary>
    ///     异常
    /// </summary>
    public const int Exception = 9;
}

/// <summary>
///     空记录
/// </summary>
public readonly record struct EmptyRecord;

/// <summary>
///     无结果
/// </summary>
public record NullResult : AbstractResult;

/// <summary>
///     空结果
/// </summary>
public record EmptyResult : DataResult<EmptyRecord>;