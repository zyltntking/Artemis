namespace Artemis.Data.Core.Exceptions;

#region MyRegion

/// <summary>
///     异常成员接口
/// </summary>
file interface IArtemisException
{
    /// <summary>
    ///     错误信息
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    ///     错误编码
    /// </summary>
    int ErrorCode { get; }
}

#endregion

/// <summary>
///     Artemis系统日志
/// </summary>
public abstract class ArtemisException : Exception, IArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    protected ArtemisException(string errorMessage, int errorCode = ExceptionCode.ArtemisException) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    /// <summary>
    ///     错误信息
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    ///     错误编码
    /// </summary>
    public int ErrorCode { get; }
}