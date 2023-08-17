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
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.Partition).HasColumnType(DataTypeSet.Integer).HasComment("分区标识");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.RelationConfigure(builder);

        builder.HasKey(entity => entity.Partition).HasName($"{nameof(TEntity)}Partition");
    }

    #endregion
}