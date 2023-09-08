using Artemis.Data.Core.Exceptions;

namespace Artemis.App.Logic.IdentityLogic.Exceptions;

/// <summary>
/// 认证不支持手机号码异常
/// </summary>
public class NotSupportPhoneNumberException : ArtemisException
{

    /// <summary>
    ///     构造
    /// </summary>
    public NotSupportPhoneNumberException() : base("认证系统不支持手机号码", ExceptionCode.NotSupportPhoneNumberException)
    {
    }

    /// <summary>
    /// 错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public NotSupportPhoneNumberException(string errorMessage, int errorCode = ExceptionCode.NotSupportPhoneNumberException) : base(errorMessage, errorCode)
    {
    }
}