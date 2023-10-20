using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Models;

/// <summary>
///     用户角色映射
/// </summary>
public class UserRole : IdentityUserRole<Guid>, IKeySlot<int>, IUserRole
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Required]
    public virtual required int Id { get; set; }

    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    public override required Guid UserId { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    [Required]
    public override required Guid RoleId { get; set; }
}