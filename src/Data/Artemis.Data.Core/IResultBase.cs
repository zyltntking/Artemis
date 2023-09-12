namespace Artemis.Data.Core;

/// <summary>
///     数据结果协议模板接口
/// </summary>
public interface IResultBase
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
internal interface IResultBase<T> : IResultBase
{
    /// <summary>
    ///     数据
    /// </summary>
    T? Data { get; set; }
}