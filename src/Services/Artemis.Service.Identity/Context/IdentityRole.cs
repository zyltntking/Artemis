using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证角色实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityRoleConfiguration))]
public sealed class IdentityRole : Role
{
    /// <summary>
    ///     具备该角色的用户
    /// </summary>
    public ICollection<IdentityUser>? Users { get; set; }

    /// <summary>
    ///     用户角色表映射
    /// </summary>
    public ICollection<IdentityUserRole>? UserRoles { get; set; }

    /// <summary>
    ///     角色凭据映射
    /// </summary>
    public ICollection<IdentityRoleClaim>? RoleClaims { get; set; }
}