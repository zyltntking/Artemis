using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store.Configuration;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class PartitionConfiguration<TEntity> : ModelConfiguration<TEntity>
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

        builder.Property(entity => entity.Partition)
            .HasColumnType(DataTypeSet.Integer);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex(entity => entity.Partition)
            .HasDatabaseName($"IX_{TableName}_Partition");
    }

    #endregion
}

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ModelConfiguration<TEntity> : MateSlotModelConfiguration<TEntity>
    where TEntity : class, IModelBase, IKeySlot, IMateSlot
{
}

/// <summary>
///     ArtemisMateSlot
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class MateSlotModelConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : class, IMateSlot
{
    #region Overrides of ModelBaseConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreatedAt)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.UpdatedAt)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.DeletedAt)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     元索引配置
    /// </summary>
    /// <param name="builder"></param>
    protected void MetaIndexConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex(entity => entity.CreatedAt)
            .HasDatabaseName($"IX_{TableName}_CreatedAt");
        builder.HasIndex(entity => entity.UpdatedAt)
            .HasDatabaseName($"IX_{TableName}_UpdatedAt");
        builder.HasIndex(entity => entity.DeletedAt)
            .HasDatabaseName($"IX_{TableName}_DeletedAt");
    }

    #endregion
}

/// <summary>
///     ArtemisKeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeySlotModelConfiguration<TEntity> : KeySlotModelConfiguration<TEntity, Guid>
    where TEntity : class, IKeySlot
{
}

/// <summary>
///     ArtemisKeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class KeySlotModelConfiguration<TEntity, TKey> : BaseConfiguration<TEntity>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     Artemis抽象类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    #region Implementation of IEntityTypeConfiguration<TEntity>

    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        TableConfigure(builder);

        FieldConfigure(builder);

        RelationConfigure(builder);
    }

    #endregion

    /// <summary>
    ///     数据库类型
    /// </summary>
    protected virtual DbType DbType => DbType.PostgreSql;

    /// <summary>
    ///     数据类型集合访问器
    /// </summary>
    protected DataTypeSet DataTypeSet => DataTypeAdapter.GetDataTypeSet(DbType);

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected virtual string DataSetDescription => nameof(TEntity);

    /// <summary>
    ///     表名
    /// </summary>
    protected virtual string TableName => nameof(TEntity);

    /// <summary>
    ///     架构名
    /// </summary>
    protected virtual string? SchemaName => null;

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    private void TableConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(TableName, SchemaName, table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }
}