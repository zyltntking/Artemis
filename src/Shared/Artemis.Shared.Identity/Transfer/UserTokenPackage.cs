using System.ComponentModel.DataAnnotations;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户令牌信息接口
/// </summary>
internal interface IUserToken
{
    /// <summary>
    ///     登录提供程序
    /// </summary>
    string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    string? Value { get; set; }

    #region DefaultImlement

    /// <summary>
    ///     生成标识
    /// </summary>
    string GenerateFlag => $"{LoginProvider}:{Name}";

    #endregion
}

/// <summary>
///     用户令牌信息文档接口
/// </summary>
file interface IUserTokenDocument : IUserToken
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

#endregion

/// <summary>
///     基本用户令牌信息
/// </summary>
public record UserTokenPackage : IUserToken
{
    #region DefaultImlement

    /// <summary>
    ///     生成标识
    /// </summary>
    public string GenerateFlag => $"{LoginProvider}:{Name}";

    #endregion

    #region Implementation of IUserToken

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    public string? Value { get; set; }

    #endregion
}

/// <summary>
///     用户登录信息
/// </summary>
public sealed record UserTokenInfo : UserTokenPackage, IUserTokenDocument
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }
}