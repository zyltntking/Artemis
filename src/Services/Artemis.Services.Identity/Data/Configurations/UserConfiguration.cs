using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户数据集配置
/// </summary>
public class UserConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUser>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityUser>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    #region Overrides of ArtemisMateSlotConfiguration<ArtemisIdentityUser>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityUser> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.Id)
            .HasComment("标识");

        builder.Property(entity => entity.UserName)
            .HasComment("角色名");

        builder.Property(entity => entity.NormalizedUserName)
            .HasComment("规范化角色名");

        builder.Property(entity => entity.Email)
            .HasComment("邮箱地址");

        builder.Property(entity => entity.NormalizedEmail)
            .HasComment("规范化邮箱地址");

        builder.Property(entity => entity.EmailConfirmed)
            .HasComment("是否确认邮箱地址");

        builder.Property(entity => entity.PasswordHash)
            .HasComment("密码哈希");

        builder.Property(item => item.SecurityStamp)
            .HasComment("加密戳");

        builder.Property(item => item.ConcurrencyStamp)
            .HasComment("并发戳");

        builder.Property(entity => entity.PhoneNumber)
            .HasComment("电话号码");

        builder.Property(entity => entity.PhoneNumberConfirmed)
            .HasComment("是否确认电话号码");

        builder.Property(entity => entity.TwoFactorEnabled)
            .HasComment("是否允许双步认证");

        builder.Property(entity => entity.LockoutEnd)
            .HasComment("用户锁定到期时间标记");

        builder.Property(entity => entity.LockoutEnabled)
            .HasComment("是否允许锁定用户");

        builder.Property(entity => entity.AccessFailedCount)
            .HasComment("尝试错误数量");

    }

    #endregion

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUser> builder)
    {
        // Each User can have many UserClaims
        builder.HasMany(user => user.Claims)
            .WithOne(userClaim => userClaim.User)
            .HasForeignKey(userClaim => userClaim.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserLogins
        builder.HasMany(user => user.Logins)
            .WithOne(userLogin => userLogin.User)
            .HasForeignKey(userLogin => userLogin.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserTokens
        builder.HasMany(user => user.Tokens)
            .WithOne(userToken => userToken.User)
            .HasForeignKey(userToken => userToken.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many entries in the UserRole join table
        builder.HasMany(user => user.UserRoles)
            .WithOne(userRole => userRole.User)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}