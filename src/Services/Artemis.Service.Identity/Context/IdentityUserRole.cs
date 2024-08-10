using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户角色关系实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserRoleConfiguration))]
public sealed class IdentityUserRole : UserRole
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public IdentityUser? User { get; set; }

    /// <summary>
    ///     所属角色
    /// </summary>
    public IdentityRole? Role { get; set; }
}