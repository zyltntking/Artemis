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
    ///     实体已存在异常
    /// </summary>
    [Description("实体已存在异常")] public const int EntityHasBeenSetException = 90003;

    /// <summary>
    ///     实例未设置异常
    /// </summary>
    [Description("实例未设置异常")] public const int InstanceNotImplementException = 90004;

    /// <summary>
    ///     映射目标空异常
    /// </summary>
    [Description("映射目标空异常")] public const int MapTargetNullException = 90005;

    /// <summary>
    ///     枚举类不匹配异常
    /// </summary>
    [Description("枚举类不匹配异常")] public const int EnumerationNotMatchException = 90006;

    /// <summary>
    ///     存储已被释放异常
    /// </summary>
    [Description("存储已被释放异常")] public const int StoreDisposedException = 90007;

    /// <summary>
    ///     存储参数空异常
    /// </summary>
    [Description("存储参数空异常")] public const int StoreParameterNullException = 90008;

    /// <summary>
    ///     管理器已被释放异常
    /// </summary>
    [Description("管理器已被释放异常")] public const int ManagerDisposedException = 90009;

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