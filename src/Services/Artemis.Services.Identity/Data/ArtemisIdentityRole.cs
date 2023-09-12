using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
/// ArtemisIdentityRole
/// </summary>
[EntityTypeConfiguration(typeof(RoleConfiguration))]
public sealed class ArtemisIdentityRole : IdentityRole<Guid>, IMateSlot
{
    /// <summary>
    /// 角色编码
    /// </summary>
    [Required]
    [MaxLength(16)]
    public string Code { get; set; } = null!;

    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    #endregion

    /// <summary>
    /// 用户角色表映射
    /// </summary>
    public ICollection<ArtemisIdentityUserRole>? UserRoles { get; set; }

    /// <summary>
    /// 角色凭据映射
    /// </summary>
    public ICollection<ArtemisIdentityRoleClaim>? RoleClaims { get; set; }
}