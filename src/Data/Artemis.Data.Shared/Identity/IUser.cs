namespace Artemis.Data.Shared.Identity;

/// <summary>
///     用户接口
/// </summary>
public interface IUser : IUserInfo
{
    /// <summary>
    ///     标准化用户名
    /// </summary>
    string NormalizedUserName { get; set; }

    /// <summary>
    ///     标准化电子邮件
    /// </summary>
    string? NormalizedEmail { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    ///     密码哈希
    /// </summary>
    string PasswordHash { get; set; }

    /// <summary>
    ///     密码锁
    /// </summary>
    string? SecurityStamp { get; set; }

    /// <summary>
    ///     是否启用双因子认证
    /// </summary>
    bool TwoFactorEnabled { get; set; }

    /// <summary>
    ///     用户锁定到期时间标记
    /// </summary>
    DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    ///     是否启用锁定
    /// </summary>
    bool LockoutEnabled { get; set; }

    /// <summary>
    ///     失败尝试次数
    /// </summary>
    int AccessFailedCount { get; set; }
}

/// <summary>
///     基本用户信息接口
/// </summary>
public interface IUserInfo
{
    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    string? PhoneNumber { get; set; }
}