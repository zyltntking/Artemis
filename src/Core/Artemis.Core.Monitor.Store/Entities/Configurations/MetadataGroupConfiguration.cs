using Artemis.Data.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Core.Monitor.Store.Entities.Configurations;

/// <summary>
/// 元数据组模型配置
/// </summary>
public class MetadataGroupConfiguration : ModelBaseTypeConfiguration<MetadataGroup>
{
    #region Overrides of ModelBaseTypeConfiguration<MetadataGroup>

    /// <summary>
    ///    数据库类型
    /// </summary>
    protected override DbType DbType => Constants.DbType;

    /// <summary>
    /// 数据库字段备注配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldCommentConfigure(EntityTypeBuilder<MetadataGroup> builder)
    {
        base.FieldCommentConfigure(builder);

        builder.Property(entity => entity.Key).HasComment("数据键");

        builder.Property(entity => entity.Value).HasComment("数据值");
    }

    #endregion
}