using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
///     认证数据集上下文
/// </summary>
public class IdentityContext : DbContext
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="options">配置</param>
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    /// <summary>
    ///     凭据数据集
    /// </summary>
    public virtual DbSet<IdentityClaim> Claims { get; set; } = default!;

    /// <summary>
    ///     用户数据集
    /// </summary>
    public virtual DbSet<IdentityUser> Users { get; set; } = default!;

    /// <summary>
    ///     用户档案数据集
    /// </summary>
    public virtual DbSet<IdentityUserProfile> UserProfiles { get; set; } = default!;

    /// <summary>
    ///     用户凭据数据集
    /// </summary>
    public virtual DbSet<IdentityUserClaim> UserClaims { get; set; } = default!;

    /// <summary>
    ///     用户登录数据集
    /// </summary>
    public virtual DbSet<IdentityUserLogin> UserLogins { get; set; } = default!;

    /// <summary>
    ///     用户令牌数据集
    /// </summary>
    public virtual DbSet<IdentityUserToken> UserTokens { get; set; } = default!;

    /// <summary>
    ///     用户角色关系映射数据集
    /// </summary>
    public virtual DbSet<IdentityUserRole> UserRoles { get; set; } = default!;

    /// <summary>
    ///     角色数据集
    /// </summary>
    public virtual DbSet<IdentityRole> Roles { get; set; } = default!;

    /// <summary>
    ///     角色凭据数据集
    /// </summary>
    public virtual DbSet<IdentityRoleClaim> RoleClaims { get; set; } = default!;

    #region Overrides of DbContext

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        // User Role Map
        modelBuilder.Entity<IdentityRole>()
            .HasMany(role => role.Users) //left side
            .WithMany(user => user.Roles) //right side
            .UsingEntity<IdentityUserRole>(
                userRoleLeft => userRoleLeft
                    .HasOne(userRole => userRole.User)
                    .WithMany(user => user.UserRoles)
                    .HasForeignKey(userRole => userRole.UserId)
                    .HasConstraintName(nameof(IdentityUserRole).ForeignKeyName(nameof(IdentityUser)))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(user => user.Id),
                userRoleRight => userRoleRight
                    .HasOne(userRole => userRole.Role)
                    .WithMany(role => role.UserRoles)
                    .HasForeignKey(userRole => userRole.RoleId)
                    .HasConstraintName(nameof(IdentityUserRole).ForeignKeyName(nameof(IdentityRole)))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(role => role.Id),
                userRoleJoin =>
                {
                    userRoleJoin.HasKey(userRole => new { userRole.UserId, userRole.RoleId })
                        .HasName(nameof(IdentityUserRole).KeyName());
                });
    }

    #endregion
}