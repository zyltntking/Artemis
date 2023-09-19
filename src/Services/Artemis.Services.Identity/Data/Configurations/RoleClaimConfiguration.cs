using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     角色凭据数据集配置
/// </summary>
public class RoleClaimConfiguration : IdentityConfiguration<ArtemisRoleClaim>
{
    #region Overrides of ArtemisConfiguration<ArtemisRoleClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色凭据数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisRoleClaim);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisRoleClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(roleClaim => roleClaim.Id)
            .HasComment("标识");

        builder.Property(roleClaim => roleClaim.RoleId)
            .HasComment("角色标识");

        builder.Property(roleClaim => roleClaim.ClaimType)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("凭据类型");

        builder.Property(roleClaim => roleClaim.ClaimValue)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("凭据类型");

        builder.Property(roleClaims => roleClaims.CheckStamp)
            .HasMaxLength(64)
            .IsRequired()
            .HasComment("校验戳");

        builder.Property(roleClaims => roleClaims.Description)
            .HasMaxLength(128)
            .HasComment("凭据描述");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisRoleClaim> builder)
    {
        // Role Claim Key
        builder.HasKey(roleClaim => roleClaim.Id)
            .HasName($"PK_{TableName}");

        // Role Claim Index
        builder.HasIndex(roleClaim => roleClaim.ClaimType)
            .HasDatabaseName($"IX_{TableName}_ClaimType");

        builder.HasIndex(roleClaim => new { roleClaim.CheckStamp, roleClaim.RoleId })
            .HasDatabaseName($"IX_{TableName}_CheckStamp_RoleId")
            .IsUnique();
    }

    #endregion
}