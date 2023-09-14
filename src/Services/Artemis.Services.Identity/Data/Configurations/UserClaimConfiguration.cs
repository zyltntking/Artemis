using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户凭据数据集配置
/// </summary>
public class UserClaimConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserClaim>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityUserClaim>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityUserClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userClaim => userClaim.Id)
            .HasComment("标识");

        builder.Property(userClaim => userClaim.UserId)
            .HasComment("用户标识");

        builder.Property(userClaim => userClaim.ClaimType)
            .HasMaxLength(128).HasComment("凭据类型");

        builder.Property(userClaim => userClaim.ClaimValue)
            .HasMaxLength(128).HasComment("凭据类型");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUserClaim> builder)
    {
        // User Claim Key
        builder.HasKey(userClaim => userClaim.Id).HasName("PK_UserClaims");

        // User Claim Index
        builder.HasIndex(userClaim => userClaim.ClaimType).HasDatabaseName("IX_UserClaim_ClaimType");
    }

    #endregion
}