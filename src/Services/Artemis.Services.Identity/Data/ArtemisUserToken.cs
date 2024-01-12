using Artemis.Services.Identity.Data.Configurations;
using Artemis.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     ArtemisUserToken
/// </summary>
[EntityTypeConfiguration(typeof(UserTokenConfiguration))]
public class ArtemisUserToken : UserToken
{
    /// <summary>
    ///     凭据所属用户
    /// </summary>
    public virtual ArtemisUser User { get; set; } = null!;
}