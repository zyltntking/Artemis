using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户凭据实体配置
/// </summary>
internal sealed class IdentityUserClaimConfiguration : KeySlotEntityConfiguration<IdentityUserClaim, int>
{
    #region Overrides of ModelConfiguration<IdentityUserClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户凭据数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUserClaim).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUserClaim> builder)
    {
        // Role Claim Index
        builder.HasIndex(userClaim => new { userClaim.ClaimType, userClaim.ClaimValue })
            .HasDatabaseName(IndexName("ClaimType", "ClaimValue"));

        builder.HasIndex(userClaim => new { userClaim.UserId, userClaim.CheckStamp })
            .HasDatabaseName(IndexName("UserId", "CheckStamp"))
            .IsUnique();
    }

    #endregion
}