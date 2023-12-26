namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     枚举类不匹配异常
/// </summary>
public sealed class EnumerationNotMatchException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public EnumerationNotMatchException(string errorMessage, int errorCode = ExceptionCode.EnumerationNotMatchException)
        : base(errorMessage, errorCode)
    {
    }
}