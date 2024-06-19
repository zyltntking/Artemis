using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户凭据实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserClaimConfiguration))]
public sealed class IdentityUserClaim : UserClaim
{
    /// <summary>
    ///     所属用户
    /// </summary>
    public required IdentityUser User { get; set; }
}