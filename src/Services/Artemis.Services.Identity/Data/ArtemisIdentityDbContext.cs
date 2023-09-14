using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     Artemis认证数据上下文
/// </summary>
public class ArtemisIdentityDbContext : IdentityDbContext<
    ArtemisIdentityUser,
    ArtemisIdentityRole,
    Guid,
    ArtemisIdentityUserClaim,
    ArtemisIdentityUserRole,
    ArtemisIdentityUserLogin,
    ArtemisIdentityRoleClaim,
    ArtemisIdentityUserToken>
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options">创建配置</param>
    public ArtemisIdentityDbContext(DbContextOptions<ArtemisIdentityDbContext> options)
        : base(options)
    {
    }

    #region OnModelCreating

    /// <summary>
    ///     Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">
    ///     The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }

    #endregion
}