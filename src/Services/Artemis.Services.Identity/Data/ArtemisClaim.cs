using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisClaim
/// </summary>
[EntityTypeConfiguration(typeof(ClaimConfiguration))]
public class ArtemisClaim : Claim
{
}