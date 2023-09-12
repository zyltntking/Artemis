using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
/// ArtemisIdentityUserRole
/// </summary>
[EntityTypeConfiguration(typeof(UserRoleConfiguration))]
public sealed class ArtemisIdentityUserRole : IdentityUserRole<Guid>, IMateSlot
{
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
    /// 用户映射
    /// </summary>
    public ArtemisIdentityUser User { get; set; } = null!;

    /// <summary>
    /// 角色映射
    /// </summary>
    public ArtemisIdentityRole Role { get; set; } = null!;
}