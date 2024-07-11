using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
///     角色凭据模型
/// </summary>
public class RoleClaim : StandardClaim, IRoleClaim
{
    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    [Comment("角色标识")]
    public required Guid RoleId { get; set; }
}