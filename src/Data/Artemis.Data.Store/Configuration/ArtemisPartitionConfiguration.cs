using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store.Configuration;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ArtemisPartitionConfiguration<TEntity> : ArtemisModelConfiguration<TEntity>
    where TEntity : class, IPartitionBase
{
    #region Overrides of ArtemisModelConfiguration<TEntity>

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
        builder.HasIndex(entity => entity.Partition);
    }

    #endregion
}