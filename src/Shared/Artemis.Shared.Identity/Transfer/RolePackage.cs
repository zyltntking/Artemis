using System.ComponentModel.DataAnnotations;
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
public record RolePackage : IRole
{
    #region Implementation of IRole

    /// <summary>
    ///     角色名
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    [MaxLength(256)]
    public string? Description { get; set; }

    #endregion
}

/// <summary>
///     角色信息
/// </summary>
public sealed record RoleInfo : RolePackage, IKeySlot
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    public required Guid Id { get; set; }
}