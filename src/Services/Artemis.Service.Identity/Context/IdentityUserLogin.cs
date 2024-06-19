using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户登录实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserLoginConfiguration))]
public sealed class IdentityUserLogin : UserLogin
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public required IdentityUser User { get; set; }
}