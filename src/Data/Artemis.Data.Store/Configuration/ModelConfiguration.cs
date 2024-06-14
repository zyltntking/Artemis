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
    #region Overrides of ModelConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.FieldConfigure(builder);

        PartitionFieldConfigure(builder);
    }

    /// <summary>
    ///     分区字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void PartitionFieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.Partition)
            .IsRequired()
            .HasComment("分区标识")
            .HasColumnType(DataTypeSet.Integer);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.RelationConfigure(builder);

        PartitionIndexConfigure(builder);
    }

    /// <summary>
    ///     分区索引配置
    /// </summary>
    /// <param name="builder"></param>
    private void PartitionIndexConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex(entity => entity.Partition)
            .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ModelConfiguration<TEntity> : MateSlotModelConfiguration<TEntity>
    where TEntity : class, IModelBase
{
    /// <summary>
    ///     是否启用标识索引
    /// </summary>
    protected virtual bool UseHandlerIndex => false;

    #region Overrides of MateSlotModelConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.Id)
            .IsRequired()
            .HasComment("标识")
            .HasColumnType(DataTypeSet.Guid);

        EntityFieldConfigure(builder);

        HandlerFieldConfigure(builder);

        base.FieldConfigure(builder);
    }

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void EntityFieldConfigure(EntityTypeBuilder<TEntity> builder);

    /// <summary>
    ///     标识字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void HandlerFieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreateBy)
            .IsRequired()
            .HasComment("创建者标识")
            .HasColumnType(DataTypeSet.Guid);

        builder.Property(entity => entity.ModifyBy)
            .IsRequired()
            .HasComment("更新者标识")
            .HasColumnType(DataTypeSet.Guid);

        builder.Property(entity => entity.RemoveBy)
            .IsRequired(false)
            .HasComment("删除者标识")
            .HasColumnType(DataTypeSet.Guid);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id)
            .HasName(KeyName);

        EntityRelationConfigure(builder);

        if (UseHandlerIndex) HandlerIndexConfigure(builder);

        base.RelationConfigure(builder);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void EntityRelationConfigure(EntityTypeBuilder<TEntity> builder);

    /// <summary>
    ///     标识索引配置
    /// </summary>
    /// <param name="builder"></param>
    private void HandlerIndexConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex(entity => entity.CreateBy)
            .HasDatabaseName(IndexName("CreateBy"));
        builder.HasIndex(entity => entity.ModifyBy)
            .HasDatabaseName(IndexName("ModifyBy"));
        builder.HasIndex(entity => entity.RemoveBy)
            .HasDatabaseName(IndexName("RemoveBy"));
    }

    #endregion
}

/// <summary>
///     ArtemisMateSlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class MateSlotModelConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : class, IMateSlot
{
    /// <summary>
    ///     是否启用元数据索引
    /// </summary>
    protected virtual bool UseMateIndex => true;

    #region Overrides of ModelBaseConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        MetaFieldConfigure(builder);
    }

    /// <summary>
    ///     元数据字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void MetaFieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreatedAt)
            .IsRequired()
            .HasComment("创建时间")
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.UpdatedAt)
            .IsRequired()
            .HasComment("更新时间")
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.DeletedAt)
            .IsRequired(false)
            .HasComment("删除时间")
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateIndex) MetaIndexConfigure(builder);
    }

    /// <summary>
    ///     元数据索引配置
    /// </summary>
    /// <param name="builder"></param>
    private void MetaIndexConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex(entity => entity.CreatedAt)
            .HasDatabaseName(IndexName("CreatedAt"));
        builder.HasIndex(entity => entity.UpdatedAt)
            .HasDatabaseName(IndexName("UpdatedAt"));
        builder.HasIndex(entity => entity.DeletedAt)
            .HasDatabaseName(IndexName("DeletedAt"));
    }

    #endregion
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
    ///     生成主键名称
    /// </summary>
    protected virtual string KeyName => $"PK_{TableName}";

    /// <summary>
    ///     生成索引名称
    /// </summary>
    /// <param name="properties">字段名称</param>
    /// <returns></returns>
    protected virtual string IndexName(params string[] properties)
    {
        return $"IX_{TableName}_{string.Join('_', properties)}";
    }

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
    protected abstract void FieldConfigure(EntityTypeBuilder<TEntity> builder);

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void RelationConfigure(EntityTypeBuilder<TEntity> builder);
}