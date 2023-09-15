using System.ComponentModel;

namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     异常编码
/// </summary>
public static class ExceptionCode
{
    /// <summary>
    ///     Artemis异常
    /// </summary>
    [Description("Artemis异常")] public const int ArtemisException = ResultStatus.Exception;

    /// <summary>
    ///     创建实例异常
    /// </summary>
    [Description("创建实例异常")] public const int CreateInstanceException = 90001;

    /// <summary>
    ///     实体查询失败异常
    /// </summary>
    [Description("实体查询失败异常")] public const int EntityNotFoundException = 90002;

    /// <summary>
    ///     凭据无效异常
    /// </summary>
    [Description("凭据无效异常")] public const int ClaimInvalidException = 91001;

    /// <summary>
    ///     认证不支持邮箱异常
    /// </summary>
    [Description("认证不支持邮箱异常")] public const int NotSupportEmailException = 91002;

    /// <summary>
    ///     认证不支持手机号码异常
    /// </summary>
    [Description("认证不支持手机号码异常")] public const int NotSupportPhoneNumberException = 91003;
}