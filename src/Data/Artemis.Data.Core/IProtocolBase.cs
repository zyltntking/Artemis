namespace Artemis.Data.Core;

/// <summary>
///     数据结果协议模板接口
/// </summary>
internal interface IDataResult
{
    /// <summary>
    ///     消息码
    /// </summary>
    int Code { get; set; }

    /// <summary>
    ///     消息
    /// </summary>
    string Message { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    long Timestamp { get; set; }
}

/// <summary>
///     数据结果协议模板接口
/// </summary>
/// <typeparam name="T">模板类型</typeparam>
internal interface IDataResult<T> : IDataResult
{
    /// <summary>
    ///     数据
    /// </summary>
    T? Data { get; set; }

    /// <summary>
    ///     异常信息
    /// </summary>
    string? Error { get; set; }
}