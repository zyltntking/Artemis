using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ModelBaseTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IModelBase
{
    /// <summary>
    ///    数据库类型
    /// </summary>
    protected virtual DbType DbType => DbType.SqlServer;

    #region Implementation of IEntityTypeConfiguration<T>

    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        FieldCommentConfigure(builder);

        DataTypeConfigure(builder);
    }

    #endregion

    /// <summary>
    /// 数据库字段备注配置
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
    /// 数据库配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void DataTypeConfigure(EntityTypeBuilder<TEntity> builder)
    {
        var dataType = DataTypeAdapter.GetDataTypeSet(DbType);

        builder.Property(entity => entity.CreatedAt).HasColumnType(dataType.DateTime);

        builder.Property(entity => entity.UpdatedAt).HasColumnType(dataType.DateTime);

        builder.Property(entity => entity.DeletedAt).HasColumnType(dataType.DateTime);
    }
}