namespace Artemis.Extensions.Web.Exceptions;

/// <summary>
/// Artemis系统日志
/// </summary>
public abstract class ArtemisException : Exception
{
    /// <summary>
    /// 错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    protected ArtemisException(string errorMessage, int errorCode = ExceptionCode.ArtemisException) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// 错误编码
    /// </summary>
    public int ErrorCode { get; }
}