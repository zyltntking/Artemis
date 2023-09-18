namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     凭据无效异常
/// </summary>
public class ClaimInvalidException : ArtemisException
{
    /// <summary>
    ///     构造
    /// </summary>
    public ClaimInvalidException() : base("凭据无效", ExceptionCode.ClaimInvalidException)
    {
    }

    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="actionName">操作名</param>
    public ClaimInvalidException(string actionName) : base($"操作{actionName}没有有效的凭据",
        ExceptionCode.ClaimInvalidException)
    {
    }
}