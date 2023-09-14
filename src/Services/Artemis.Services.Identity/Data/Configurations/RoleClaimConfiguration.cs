using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     角色凭据数据集配置
/// </summary>
public class RoleClaimConfiguration : ArtemisIdentityConfiguration<ArtemisRoleClaim>
{
    #region Overrides of ArtemisConfiguration<ArtemisRoleClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色凭据数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisRoleClaim> builder)
    {
        builder.ToTable(nameof(ArtemisRoleClaim), table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisRoleClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(roleClaim => roleClaim.Id).HasComment("标识");

        builder.Property(roleClaim => roleClaim.RoleId).HasComment("角色标识");

        builder.Property(roleClaim => roleClaim.ClaimType)
            .HasMaxLength(128).HasComment("凭据类型");

        builder.Property(roleClaim => roleClaim.ClaimValue)
            .HasMaxLength(128).HasComment("凭据类型");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisRoleClaim> builder)
    {
        // Role Claim Key
        builder.HasKey(roleClaim => roleClaim.Id).HasName("PK_RoleClaims");

        // Role Claim Index
        builder.HasIndex(roleClaim => roleClaim.ClaimType).HasDatabaseName("IX_RoleClaim_ClaimType");
    }

    #endregion
}