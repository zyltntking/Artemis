using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityRole
/// </summary>
[EntityTypeConfiguration(typeof(RoleConfiguration))]
public class ArtemisIdentityRole : IdentityRole<Guid>, IMateSlot
{
    /// <summary>
    ///     用户角色表映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public virtual ICollection<ArtemisIdentityRoleClaim>? RoleClaims { get; set; }

    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public virtual DateTime? DeletedAt { get; set; }

    #endregion
}