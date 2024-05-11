using System.ComponentModel.DataAnnotations;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户登录信息接口
/// </summary>
internal interface IUserLogin
{
    /// <summary>
    ///     登录提供程序
    /// </summary>
    string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    string? ProviderDisplayName { get; set; }
}

/// <summary>
///     用户登录信息接口
/// </summary>
file interface IUserLoginInfo : IUserLogin
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

#endregion

/// <summary>
///     基本用户登录信息
/// </summary>
public record UserLoginPackage : IUserLogin
{
    #region Implementation of IUserLogin

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(256)]
    public required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(128)]
    public string? ProviderDisplayName { get; set; }

    #endregion
}

/// <summary>
///     用户登录信息
/// </summary>
public sealed record UserLoginInfo : UserLoginPackage, IUserLoginInfo
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }
}