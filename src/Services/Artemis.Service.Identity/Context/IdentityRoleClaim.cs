using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证角色凭据实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityRoleClaimConfiguration))]
public sealed class IdentityRoleClaim : RoleClaim
{
    /// <summary>
    ///     凭据所属角色
    /// </summary>
    public required IdentityRole Role { get; set; }
}