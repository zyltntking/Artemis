namespace Artemis.Service.Shared.Identity.Transfer;

/// <summary>
///     用户信息
/// </summary>
public sealed record UserInfo : UserPackage, IUserInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion
}

/// <summary>
///     用户数据包
/// </summary>
public record UserPackage : UserSign, IUserPackage;

/// <summary>
///     用户标识信息
/// </summary>
public record UserSign : IUserSign
{
    #region Implementation of IUserInfo

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    #endregion
}

/// <summary>
///     用户认证结构
/// </summary>
public sealed record UserAuthentication : IUserAuthentication
{
    #region Implementation of IUserAuthentication

    /// <summary>
    ///     存储标识
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; } = null!;

    /// <summary>
    ///     密码哈希
    /// </summary>
    public required string PasswordHash { get; set; } = null!;

    #endregion
}