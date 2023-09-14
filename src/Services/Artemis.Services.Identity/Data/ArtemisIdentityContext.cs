using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     Artemis认证数据上下文
/// </summary>
public class ArtemisIdentityContext : IdentityDbContext<
    ArtemisUser,
    ArtemisRole,
    Guid,
    ArtemisUserClaim,
    ArtemisUserRole,
    ArtemisUserLogin,
    ArtemisRoleClaim,
    ArtemisUserToken>
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options">创建配置</param>
    public ArtemisIdentityContext(DbContextOptions<ArtemisIdentityContext> options)
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