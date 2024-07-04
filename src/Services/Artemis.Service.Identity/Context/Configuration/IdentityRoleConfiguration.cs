using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证角色实体配置
/// </summary>
internal sealed class IdentityRoleConfiguration : ConcurrencyModelEntityConfiguration<IdentityRole>
{
    #region Overrides of ModelConfiguration<IdentityRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityRole).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityRole> builder)
    {
        // Role Index
        builder.HasIndex(role => role.NormalizedName)
            .HasDatabaseName(IndexName("Name"))
            .IsUnique();

        // Each Role can have many associated RoleClaims
        builder.HasMany(role => role.RoleClaims)
            .WithOne(roleClaim => roleClaim.Role)
            .HasForeignKey(roleClaim => roleClaim.RoleId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityRoleClaim).TableName(),
                nameof(IdentityRole).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}