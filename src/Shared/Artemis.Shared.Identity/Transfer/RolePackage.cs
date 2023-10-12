using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     基本角色接口
/// </summary>
internal interface IRole
{
    /// <summary>
    ///     角色名
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    string? Description { get; set; }
}

#endregion

/// <summary>
///     基本角色信息
/// </summary>
[DataContract]
public record RolePackage : IRole
{
    #region Implementation of IRole

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public virtual required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 2)]
    public virtual string? Description { get; set; }

    #endregion
}

/// <summary>
///     角色信息
/// </summary>
[DataContract]
public record RoleInfo : RolePackage, IKeySlot
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public override required string Name { get; set; } = null!;

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public override string? Description { get; set; }
}