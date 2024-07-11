using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     用户角色关系模型
/// </summary>
public class UserRole : IUserRole
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [Comment("用户标识")]
    public required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [Comment("角色标识")]
    public required Guid RoleId { get; set; }
}