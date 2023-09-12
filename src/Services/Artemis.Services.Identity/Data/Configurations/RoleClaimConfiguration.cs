using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 角色凭据数据集配置
/// </summary>
public sealed class RoleClaimConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityRoleClaim>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityRoleClaim>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色凭据数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityRoleClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.Id)
            .HasComment("标识");

        builder.Property(entity => entity.RoleId)
            .HasComment("角色标识");

        builder.Property(entity => entity.ClaimType)
            .HasComment("凭据类型");

        builder.Property(entity => entity.ClaimValue)
            .HasComment("凭据类型");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityRoleClaim> builder)
    {
        // Role Claim Index
        builder.HasIndex(entity => entity.ClaimType);
    }

    #endregion
}