using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户凭据数据集配置
/// </summary>
public sealed class UserClaimConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserClaim>
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

        builder.Property(entity => entity.Id)
            .HasComment("标识");

        builder.Property(entity => entity.UserId)
            .HasComment("用户标识");

        builder.Property(entity => entity.ClaimType)
            .HasComment("凭据类型");

        builder.Property(entity => entity.ClaimValue)
            .HasComment("凭据类型");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUserClaim> builder)
    {
        // User Claim Index
        builder.HasIndex(entity => entity.ClaimType);
    }

    #endregion
}