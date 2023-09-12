using Artemis.Data.Core.Exceptions;

namespace Artemis.Services.Identity.Exceptions;

/// <summary>
///     认证系统不支持邮箱异常
/// </summary>
public class NotSupportEmailException : ArtemisException
{
    /// <summary>
    ///     构造
    /// </summary>
    public NotSupportEmailException() : base("认证系统不支持邮箱", ExceptionCode.NotSupportEmailException)
    {
    }

    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public NotSupportEmailException(string errorMessage, int errorCode = ExceptionCode.NotSupportEmailException) : base(
        errorMessage, errorCode)
    {
    }
}