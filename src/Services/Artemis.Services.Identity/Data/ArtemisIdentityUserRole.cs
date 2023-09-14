using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisIdentityUserRole
/// </summary>
[EntityTypeConfiguration(typeof(UserRoleConfiguration))]
public class ArtemisIdentityUserRole : UserRole
{
    /// <summary>
    ///     用户映射
    /// </summary>
    public virtual ArtemisIdentityUser User { get; set; } = null!;

    /// <summary>
    ///     角色映射
    /// </summary>
    public virtual ArtemisIdentityRole Role { get; set; } = null!;
}