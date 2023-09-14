using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
/// ArtemisIdentityRoleClaim
/// </summary>
[EntityTypeConfiguration(typeof(RoleClaimConfiguration))]
public class ArtemisIdentityRoleClaim : IdentityRoleClaim<Guid>, IMateSlot
{
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

    /// <summary>
    /// 凭据所属角色
    /// </summary>
    public virtual ArtemisIdentityRole Role { get; set; } = null!;
}