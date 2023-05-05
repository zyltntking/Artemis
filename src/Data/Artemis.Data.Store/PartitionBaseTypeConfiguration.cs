using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class PartitionBaseTypeConfiguration<TEntity> : ModelBaseTypeConfiguration<TEntity>
    where TEntity : class, IPartitionBase
{
    #region Overrides of ModelBaseTypeConfiguration<TEntity>

    /// <summary>
    ///     数据库字段备注配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldCommentConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.FieldCommentConfigure(builder);

        builder.Property(entity => entity.Partition).HasComment("分区标识");
    }

    /// <summary>
    ///     数据库配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void DataTypeConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.DataTypeConfigure(builder);

        builder.Property(entity => entity.Partition).HasColumnType(DataTypeSet.Integer);
    }

    #endregion
}