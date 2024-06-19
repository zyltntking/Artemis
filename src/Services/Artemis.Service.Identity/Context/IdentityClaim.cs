using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证凭据实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityClaimConfiguration))]
public sealed class IdentityClaim : Claim
{
}