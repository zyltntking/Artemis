using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserConfiguration))]
public sealed class IdentityUser : User
{
}