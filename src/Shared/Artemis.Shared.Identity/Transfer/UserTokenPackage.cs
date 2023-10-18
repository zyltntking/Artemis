using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户令牌信息接口
/// </summary>
public interface IUserToken
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
[DataContract]
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
    [DataMember(Order = 1)]
    public virtual required string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public virtual required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    [DataMember(Order = 3)]
    public virtual string? Value { get; set; }

    #endregion
}

/// <summary>
///     用户登录信息
/// </summary>
[DataContract]
public record UserTokenInfo : UserTokenPackage, IUserTokenDocument, IKeySlot<int>
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
    ///     令牌名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 4)]
    public override required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    [DataMember(Order = 5)]
    public override string? Value { get; set; }
}