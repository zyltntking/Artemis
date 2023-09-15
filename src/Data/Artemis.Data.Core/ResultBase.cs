using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Artemis.Data.Core;

/// <summary>
///     数据结果协议模板接口
/// </summary>
/// <typeparam name="T">模板类型</typeparam>
[DataContract]
public class DataResult<T> : IResultBase<T>
{
    /// <summary>
    ///     空构造
    /// </summary>
    public DataResult()
    {
    }

    /// <summary>
    ///     异常构造
    /// </summary>
    /// <param name="code">消息码</param>
    /// <param name="message">消息</param>
    /// <param name="exception">异常</param>
    public DataResult(int code, string message, Exception exception)
    {
        Code = code;
        Message = message;
        Error = exception.ToString();
        Data = default;
    }

    /// <summary>
    ///     成功构造
    /// </summary>
    /// <param name="code">消息码</param>
    /// <param name="message">消息</param>
    /// <param name="data">数据</param>
    public DataResult(int code, string message, T? data = default)
    {
        Code = code;
        Message = message;
        Data = data;
        Error = null;
    }

    /// <summary>
    ///     内部异常
    /// </summary>
    private Exception? InnerException { get; set; }

    /// <summary>
    ///     描述器
    /// </summary>
    public Dictionary<string, Collection<string>>? Descriptor { get; set; }

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

    #region Implementation of IResultBase

    /// <summary>
    ///     消息码
    /// </summary>
    [DataMember(Order = 1)]
    public int Code { get; set; }

    /// <summary>
    ///     是否成功
    /// </summary>
    [DataMember(Order = 2)]
    public bool Succeeded
    {
        get => Code == ResultStatus.Success;
        set
        {
            //ignore
        }
    }

    /// <summary>
    ///     消息
    /// </summary>
    [DataMember(Order = 3, IsRequired = true)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     异常信息
    /// </summary>
    [DataMember(Order = 4)]
    public string? Error { get; set; }

    /// <summary>
    ///     本地时间戳
    /// </summary>
    [DataMember(Order = 5)]
    public DateTime DateTime { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    [DataMember(Order = 6)]
    public long Timestamp { get; set; }

    /// <summary>
    ///     数据
    /// </summary>
    [DataMember(Order = 7)]
    public T? Data { get; set; }

    #endregion
}

/// <summary>
///     数据结果生成函数集
/// </summary>
public static class DataResult
{
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
    public static DataResult<object> Success(string message = "成功")
    {
        return GenerateResult(ResultStatus.Success, message, new object());
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
    public static DataResult<object> Fail(string message = "失败")
    {
        return GenerateResult<object>(ResultStatus.Fail, message);
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">消息</param>
    /// <returns>异常结果</returns>
    public static DataResult<T> Exception<T>(Exception exception, int code = ResultStatus.Exception,
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
    public static DataResult<object> Exception(Exception exception, int code = ResultStatus.Exception,
        string message = "异常")
    {
        return GenerateResult<object>(code, message, default, exception);
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
    private static DataResult<T> GenerateResult<T>(int code, string message, T? data = default,
        Exception? exception = null)
    {
        var result = new DataResult<T>
        {
            Code = code,
            Data = data,
            Message = message,
            Error = exception?.ToString()
        };

        if (exception != null)
        {
            result.SetException(exception);

            result.Descriptor = new Dictionary<string, Collection<string>>
            {
                { "Exception", new Collection<string> { exception.ToString() } }
            };
        }

        return result;
    }
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
    ///     异常
    /// </summary>
    public const int Exception = 9;
}