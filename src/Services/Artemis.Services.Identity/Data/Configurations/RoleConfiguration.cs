using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     角色数据集配置
/// </summary>
public class RoleConfiguration : ModelConfiguration<ArtemisRole>
{
    #region Overrides of ModelBaseConfiguration<ArtemisRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisRole);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisRole> builder)
    {
        builder.Property(role => role.Id)
            .HasComment("标识");

        builder.Property(role => role.Name)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("角色名");

        builder.Property(role => role.NormalizedName)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("规范化角色名");

        builder.Property(role => role.ConcurrencyStamp)
            .HasMaxLength(64)
            .IsConcurrencyToken()
            .HasComment("并发锁");

        builder.Property(role => role.Description)
            .HasMaxLength(256)
            .HasComment("角色描述");

        base.FieldConfigure(builder);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisRole> builder)
    {
        // Role Key
        builder.HasKey(role => role.Id)
            .HasName($"PK_{TableName}");

        // Role Index
        builder.HasIndex(role => role.NormalizedName)
            .HasDatabaseName($"IX_{TableName}_Name")
            .IsUnique();

        MetaIndexConfigure(builder);

        // Each Role can have many associated RoleClaims
        builder.HasMany(role => role.RoleClaims)
            .WithOne(roleClaim => roleClaim.Role)
            .HasForeignKey(roleClaim => roleClaim.RoleId)
            .HasConstraintName($"FK_{nameof(ArtemisUserClaim)}_{TableName}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}