using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户登录信息接口
/// </summary>
public interface IUserLogin
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
///     用户登录信息文档接口
/// </summary>
file interface IUserLoginDocument : IUserLogin
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
[DataContract]
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
    [DataMember(Order = 1)]
    public virtual required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 2)]
    public virtual required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public virtual string? ProviderDisplayName { get; set; }

    #endregion
}

/// <summary>
///     用户登录信息
/// </summary>
[DataContract]
public record UserLoginInfo : UserLoginPackage, IUserLoginDocument, IKeySlot<int>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required int Id { get; set; }


    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required Guid UserId { get; set; }

    /// <summary>
    ///     登录提供程序
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 3)]
    public override required string LoginProvider { get; set; }

    /// <summary>
    ///     提供程序密钥
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 4)]
    public override required string ProviderKey { get; set; }

    /// <summary>
    ///     提供程序显示名称
    /// </summary>
    [MaxLength(32)]
    [DataMember(Order = 5)]
    public override string? ProviderDisplayName { get; set; }
}