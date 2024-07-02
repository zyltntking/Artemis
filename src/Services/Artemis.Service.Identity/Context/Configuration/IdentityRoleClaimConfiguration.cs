using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证角色凭据实体配置
/// </summary>
internal sealed class IdentityRoleClaimConfiguration : KeySlotEntityConfiguration<IdentityRoleClaim, int>
{
    #region Overrides of ModelConfiguration<IdentityRoleClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色凭据数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityRoleClaim);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityRoleClaim> builder)
    {
        // Role Claim Index
        builder.HasIndex(roleClaim => new { roleClaim.ClaimType, roleClaim.ClaimValue })
            .HasDatabaseName(IndexName("ClaimType", "ClaimValue"));

        builder.HasIndex(roleClaim => new { roleClaim.RoleId, roleClaim.CheckStamp })
            .HasDatabaseName(IndexName("RoleId", "CheckStamp"))
            .IsUnique();
    }

    #endregion
}