using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证凭据实体配置
/// </summary>
internal sealed class IdentityClaimConfiguration : KeySlotEntityConfiguration<IdentityClaim, int>
{
    #region Overrides of ModelConfiguration<IdentityClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证凭据数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityClaim);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityClaim> builder)
    {
        // Claim Index
        builder.HasIndex(roleClaim => new { roleClaim.ClaimType, roleClaim.ClaimValue })
            .HasDatabaseName(IndexName("ClaimType", "ClaimValue"));

        builder.HasIndex(roleClaim => roleClaim.CheckStamp)
            .HasDatabaseName(IndexName("CheckStamp"))
            .IsUnique();
    }

    #endregion
}