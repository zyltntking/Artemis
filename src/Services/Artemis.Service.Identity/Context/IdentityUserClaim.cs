using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证用户凭据实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityUserClaimConfiguration))]
internal sealed class IdentityUserClaim : UserClaim
{
}