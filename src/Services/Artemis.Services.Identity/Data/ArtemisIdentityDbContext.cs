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

    #region Overrides of IdentityDbContext<ArtemisIdentityUser,ArtemisIdentityRole,Guid,ArtemisIdentityUserClaim,ArtemisIdentityUserRole,ArtemisIdentityUserLogin,ArtemisIdentityRoleClaim,ArtemisIdentityUserToken>

    /// <summary>
    ///     Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">
    ///     The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<ArtemisIdentityUser>(entity =>
        {
            entity.ToTable(nameof(ArtemisIdentityUser));

            // Each User can have many UserClaims
            entity.HasMany(user => user.Claims)
                .WithOne(userClaim => userClaim.User)
                .HasForeignKey(userClaim => userClaim.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            entity.HasMany(user => user.Logins)
                .WithOne(userLogin => userLogin.User)
                .HasForeignKey(userLogin => userLogin.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            entity.HasMany(user => user.Tokens)
                .WithOne(userToken => userToken.User)
                .HasForeignKey(userToken => userToken.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            entity.HasMany(user => user.UserRoles)
                .WithOne(userRole => userRole.User)
                .HasForeignKey(userRole => userRole.UserId)
                .IsRequired();
        });

        builder.Entity<ArtemisIdentityUserClaim>(entity => { entity.ToTable(nameof(ArtemisIdentityUserClaim)); });

        builder.Entity<ArtemisIdentityUserLogin>(entity => { entity.ToTable(nameof(ArtemisIdentityUserLogin)); });

        builder.Entity<ArtemisIdentityUserToken>(entity => { entity.ToTable(nameof(ArtemisIdentityUserToken)); });

        builder.Entity<ArtemisIdentityRole>(entity =>
        {
            entity.ToTable(nameof(ArtemisIdentityRole));

            // Each Role can have many entries in the UserRole join table
            entity.HasMany(role => role.UserRoles)
                .WithOne(userRole => userRole.Role)
                .HasForeignKey(userRole => userRole.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            entity.HasMany(role => role.RoleClaims)
                .WithOne(roleClaim => roleClaim.Role)
                .HasForeignKey(roleClaim => roleClaim.RoleId)
                .IsRequired();
        });

        builder.Entity<ArtemisIdentityRoleClaim>(entity => { entity.ToTable(nameof(ArtemisIdentityRoleClaim)); });

        builder.Entity<ArtemisIdentityUserRole>(entity => { entity.ToTable(nameof(ArtemisIdentityUserRole)); });
    }

    #endregion
}