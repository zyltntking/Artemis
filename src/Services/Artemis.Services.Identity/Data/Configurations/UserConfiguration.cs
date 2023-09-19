using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户数据集配置
/// </summary>
public class UserConfiguration : IdentityConfiguration<ArtemisUser>
{
    #region Overrides of ArtemisConfiguration<ArtemisUser>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisUser);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUser> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(user => user.Id)
            .HasComment("标识");

        builder.Property(user => user.UserName)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("用户名");

        builder.Property(user => user.NormalizedUserName)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("规范化用户名");

        builder.Property(user => user.Email)
            .HasMaxLength(128)
            .HasComment("邮箱地址");

        builder.Property(user => user.NormalizedEmail)
            .HasMaxLength(128)
            .HasComment("规范化邮箱地址");

        builder.Property(user => user.EmailConfirmed)
            .HasComment("是否确认邮箱地址");

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("密码哈希");

        builder.Property(user => user.SecurityStamp)
            .HasMaxLength(64)
            .HasComment("密码锁");

        builder.Property(user => user.ConcurrencyStamp)
            .HasMaxLength(64)
            .IsConcurrencyToken()
            .HasComment("并发锁");

        builder.Property(user => user.PhoneNumber)
            .HasMaxLength(16)
            .HasComment("电话号码");

        builder.Property(user => user.PhoneNumberConfirmed)
            .HasComment("是否确认电话号码");

        builder.Property(user => user.TwoFactorEnabled)
            .HasComment("是否允许双步认证");

        builder.Property(user => user.LockoutEnd)
            .HasComment("用户锁定到期时间标记");

        builder.Property(user => user.LockoutEnabled)
            .HasComment("是否允许锁定用户");

        builder.Property(user => user.AccessFailedCount)
            .HasComment("尝试错误数量");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUser> builder)
    {
        // User Key
        builder.HasKey(user => user.Id)
            .HasName($"PK_{TableName}");

        // User Index
        builder.HasIndex(user => user.NormalizedUserName)
            .HasDatabaseName($"IX_{TableName}_UserName")
            .IsUnique();

        builder.HasIndex(user => user.NormalizedEmail)
            .HasDatabaseName($"IX_{TableName}_Email");

        builder.HasIndex(user => user.PhoneNumber)
            .HasDatabaseName($"IX_{TableName}_PhoneNumber");

        // Each User can have many UserClaims
        builder.HasMany(user => user.Claims)
            .WithOne(userClaim => userClaim.User)
            .HasForeignKey(userClaim => userClaim.UserId)
            .HasConstraintName($"FK_{nameof(ArtemisUserClaim)}_{TableName}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserLogins
        builder.HasMany(user => user.Logins)
            .WithOne(userLogin => userLogin.User)
            .HasForeignKey(userLogin => userLogin.UserId)
            .HasConstraintName($"FK_{nameof(ArtemisUserLogin)}_{TableName}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserTokens
        builder.HasMany(user => user.Tokens)
            .WithOne(userToken => userToken.User)
            .HasForeignKey(userToken => userToken.UserId)
            .HasConstraintName($"FK_{nameof(ArtemisUserToken)}_{TableName}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many entries in the UserRole join table
        // builder.HasMany(user => user.UserRoles)
        //    .WithOne(userRole => userRole.User)
        //    .HasForeignKey(userRole => userRole.UserId)
        //    .HasConstraintName($"FK_{nameof(ArtemisUserRole)}_{nameof(ArtemisUser)}_Id")
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}