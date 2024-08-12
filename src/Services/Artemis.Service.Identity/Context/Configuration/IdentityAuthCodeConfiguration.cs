using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证验证码实体配置
/// </summary>
internal sealed class IdentityAuthCodeConfiguration : BaseModelEntityConfiguration<IdentityAuthCode>
{
    #region Overrides of ModelConfiguration<IdentityClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证验证码数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityAuthCode).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<IdentityAuthCode> builder)
    {
        builder.Property(authCode => authCode.SendTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(authCode => authCode.ExpireTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityAuthCode> builder)
    {
        // Index
        builder.HasIndex(authCode => authCode.Sign)
            .HasDatabaseName(IndexName(nameof(IdentityAuthCode.Sign)));

        builder.HasIndex(authCode => authCode.Code)
            .HasDatabaseName(IndexName(nameof(IdentityAuthCode.Code)));

        builder.HasIndex(authCode => authCode.SendTime)
            .HasDatabaseName(IndexName(nameof(IdentityAuthCode.SendTime)));

        builder.HasIndex(authCode => authCode.Used)
            .HasDatabaseName(IndexName(nameof(IdentityAuthCode.Used)));
    }

    #endregion
}