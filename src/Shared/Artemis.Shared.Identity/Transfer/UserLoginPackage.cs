using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;

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

    #region DefaultImlement

    /// <summary>
    ///     生成标识
    /// </summary>
    string GenerateFlag => $"{LoginProvider}:{ProviderKey}:{ProviderDisplayName}";

    #endregion
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
    #region DefaultImlement

    /// <summary>
    ///     生成标识
    /// </summary>
    public string GenerateFlag => $"{LoginProvider}:{ProviderKey}:{ProviderDisplayName}";

    #endregion

    #region Implementation of IUserLogin

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(32)]
    public string? ProviderDisplayName { get; set; }

    #endregion
}

/// <summary>
///     用户登录信息
/// </summary>
public sealed record UserLoginInfo : UserLoginPackage, IUserLoginInfo, IKeySlot<int>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    public required int Id { get; set; }


    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }
}