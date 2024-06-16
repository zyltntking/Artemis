using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户角色关系实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserRoleConfiguration))]
internal sealed class IdentityUserRole : UserRole
{
}