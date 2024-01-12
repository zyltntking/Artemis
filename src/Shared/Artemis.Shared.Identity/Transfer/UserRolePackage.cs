using System.ComponentModel.DataAnnotations;

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
public record UserRolePackage : IUserRole
{
    #region Implementation of IUserRole

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    public required Guid RoleId { get; set; }

    #endregion
}

/// <summary>
///     用户角色信息
/// </summary>
public sealed record UserRoleInfo : UserRolePackage;