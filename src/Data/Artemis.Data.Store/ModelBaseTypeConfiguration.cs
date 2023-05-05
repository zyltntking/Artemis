using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ModelBaseTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IModelBase
{
    /// <summary>
    ///     数据库类型
    /// </summary>
    protected virtual DbType DbType => DbType.SqlServer;

    /// <summary>
    ///     数据类型集合访问器
    /// </summary>
    protected DataTypeSet DataTypeSet => DataTypeAdapter.GetDataTypeSet(DbType);

    #region Implementation of IEntityTypeConfiguration<T>

    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        TableConfigure(builder);

        FieldCommentConfigure(builder);

        DataTypeConfigure(builder);

        RelationConfigure(builder);
    }

    #endregion

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void TableConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }

    /// <summary>
    ///     数据库字段备注配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void FieldCommentConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.Id).HasComment("标识");

        builder.Property(entity => entity.CreatedAt).HasComment("创建时间,初始化后不再进行任何变更");

        builder.Property(entity => entity.UpdatedAt).HasComment("更新时间,初始为创建时间");

        builder.Property(entity => entity.DeletedAt).HasComment("删除时间,启用软删除时生效");
    }

    /// <summary>
    ///     数据类型配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void DataTypeConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreatedAt).HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.UpdatedAt).HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.DeletedAt).HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }
}