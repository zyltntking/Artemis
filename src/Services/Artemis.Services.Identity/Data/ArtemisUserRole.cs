using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserRole
/// </summary>
[EntityTypeConfiguration(typeof(UserRoleConfiguration))]
public class ArtemisUserRole : UserRole
{
    /// <summary>
    ///     用户映射
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;

    /// <summary>
    ///     角色映射
    /// </summary>
    public virtual ArtemisRole Role { get; set; } = null!;
}