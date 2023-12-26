namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     实例未设置异常
/// </summary>
public sealed class InstanceNotImplementException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    public InstanceNotImplementException() : base("未设置实例", ExceptionCode.InstanceNotImplementException)
    {
    }

    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="instanceName">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public InstanceNotImplementException(string instanceName,
        int errorCode = ExceptionCode.InstanceNotImplementException) : base($"未设置{instanceName}实例", errorCode)
    {
    }
}