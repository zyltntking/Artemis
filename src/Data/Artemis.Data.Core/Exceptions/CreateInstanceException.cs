namespace Artemis.Data.Core.Exceptions;

/// <summary>
/// 创建实例异常
/// </summary>
public class CreateInstanceException : ArtemisException
{
    /// <summary>
    ///     构造
    /// </summary>
    public CreateInstanceException() : base("创建实例异常", ExceptionCode.CreateInstanceException)
    {
    }

    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="instanceName">实例名</param>
    public CreateInstanceException(string instanceName) : base($"创建实例:{instanceName}异常", ExceptionCode.CreateInstanceException)
    {
    }
}