using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本用户角色信息接口
/// </summary>
internal interface IUserRole
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    Guid RoleId { get; set; }
}

#endregion

/// <summary>
///     基本用户角色信息
/// </summary>
[DataContract]
public record UserRolePackage : IUserRole
{
    #region Implementation of IUserRole

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required Guid RoleId { get; set; }

    #endregion
}

/// <summary>
///     用户角色信息
/// </summary>
[DataContract]
public sealed record UserRoleInfo : UserRolePackage, IKeySlot<int>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid RoleId { get; set; }
}