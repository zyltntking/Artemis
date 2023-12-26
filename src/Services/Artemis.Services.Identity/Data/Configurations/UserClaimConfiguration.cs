using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户凭据数据集配置
/// </summary>
public class UserClaimConfiguration : IdentityConfiguration<ArtemisUserClaim>
{
    #region Overrides of ModelBaseConfiguration<ArtemisUserClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisUserClaim);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userClaim => userClaim.Id)
            .HasComment("标识");

        builder.Property(userClaim => userClaim.UserId)
            .HasComment("用户标识");

        builder.Property(userClaim => userClaim.ClaimType)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("凭据类型");

        builder.Property(userClaim => userClaim.ClaimValue)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("凭据类型");

        builder.Property(roleClaims => roleClaims.CheckStamp)
            .HasMaxLength(64)
            .IsRequired()
            .HasComment("校验戳");

        builder.Property(role => role.Description)
            .HasMaxLength(128)
            .HasComment("凭据描述");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserClaim> builder)
    {
        MetaIndexConfigure(builder);

        // User ClaimPackage Key
        builder.HasKey(userClaim => userClaim.Id)
            .HasName($"PK_{TableName}");

        // User ClaimPackage Index
        builder.HasIndex(userClaim => new { userClaim.ClaimType, userClaim.ClaimValue })
            .HasDatabaseName($"IX_{TableName}_ClaimType_ClaimValue");

        builder.HasIndex(userClaim => userClaim.CheckStamp)
            .HasDatabaseName($"IX_{TableName}_CheckStamp")
            .IsUnique();
    }

    #endregion
}