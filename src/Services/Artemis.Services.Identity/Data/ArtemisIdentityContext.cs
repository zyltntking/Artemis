using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Services.Identity.Data;

/// <summary>
///     Artemis认证数据上下文
/// </summary>
public sealed class ArtemisIdentityContext : IdentityDbContext<
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
        builder.HasDefaultSchema("identity");

        // User Role Map
        builder.Entity<ArtemisRole>()
            .HasMany(role => role.Users) //left side
            .WithMany(user => user.Roles) //right side
            .UsingEntity<ArtemisUserRole>(
                userRoleLeft => userRoleLeft
                    .HasOne(userRole => userRole.User)
                    .WithMany(user => user.UserRoles)
                    .HasForeignKey(userRole => userRole.UserId)
                    .HasConstraintName($"FK_{nameof(ArtemisUserClaim)}_{nameof(ArtemisUser)}_Id")
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(user => user.Id),
                userRoleRight => userRoleRight
                    .HasOne(userRole => userRole.Role)
                    .WithMany(role => role.UserRoles)
                    .HasForeignKey(userRole => userRole.RoleId)
                    .HasConstraintName($"FK_{nameof(ArtemisUserRole)}_{nameof(ArtemisRole)}_Id")
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(role => role.Id),
                userRoleJoin =>
                {
                    userRoleJoin.HasKey(userRole => new { userRole.UserId, userRole.RoleId })
                        .HasName($"PK_{nameof(ArtemisUserRole)}");

                    userRoleJoin.HasAlternateKey(userRole => userRole.Id).HasName($"AK_{nameof(ArtemisUserRole)}");
                });
    }

    #endregion
}