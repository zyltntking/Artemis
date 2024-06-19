using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户令牌实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserTokenConfiguration))]
public sealed class IdentityUserToken : UserToken
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public required IdentityUser User { get; set; }
}