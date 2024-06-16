using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户实体配置
/// </summary>
internal sealed class IdentityUserConfiguration : ModelConfiguration<IdentityUser>
{
    #region Overrides of ModelConfiguration<IdentityUser>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUser);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUser> builder)
    {
        // User Index
        builder.HasIndex(user => user.NormalizedUserName)
            .HasDatabaseName(IndexName("UserName"))
            .IsUnique();

        builder.HasIndex(user => user.NormalizedEmail)
            .HasDatabaseName(IndexName("Email"));

        builder.HasIndex(user => user.PhoneNumber)
            .HasDatabaseName(IndexName("PhoneNumber"));
    }

    #endregion
}