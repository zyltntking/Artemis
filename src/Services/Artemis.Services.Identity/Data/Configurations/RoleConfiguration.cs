using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 角色数据集配置
/// </summary>
public sealed class RoleConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityRole>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityRole>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityRole> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.Id)
            .HasComment("标识");

        builder.Property(entity => entity.Name)
            .HasComment("角色名");

        builder.Property(entity => entity.NormalizedName)
            .HasComment("规范化角色名");

        builder.Property(entity => entity.ConcurrencyStamp)
            .HasComment("并发戳");

        builder.Property(entity => entity.Code)
            .HasComment("角色编码");

    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityRole> builder)
    {
        // Role Index
        builder.HasIndex(role => role.Code);

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(role => role.UserRoles)
            .WithOne(userRole => userRole.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each Role can have many associated RoleClaims
        builder.HasMany(role => role.RoleClaims)
            .WithOne(roleClaim => roleClaim.Role)
            .HasForeignKey(roleClaim => roleClaim.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}