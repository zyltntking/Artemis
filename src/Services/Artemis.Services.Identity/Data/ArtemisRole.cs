using Artemis.Data.Core;
using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisRole
/// </summary>
[EntityTypeConfiguration(typeof(RoleConfiguration))]
public class ArtemisRole : Role, IModelBase
{
    /// <summary>
    ///     具备该角色的用户
    /// </summary>
    public virtual ICollection<ArtemisUser> Users { get; } = new List<ArtemisUser>();

    /// <summary>
    ///     用户角色表映射
    /// </summary>
    public virtual ICollection<ArtemisUserRole> UserRoles { get; } = new List<ArtemisUserRole>();

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public virtual ICollection<ArtemisRoleClaim> RoleClaims { get; } = new List<ArtemisRoleClaim>();

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