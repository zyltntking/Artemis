﻿using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store.Configuration;

/// <summary>
///     ArtemisMateSlot
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ArtemisMateSlotConfiguration<TEntity> : ArtemisConfiguration<TEntity>
    where TEntity : class, IMateSlot
{
    #region Overrides of ArtemisConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreatedAt).HasColumnType(DataTypeSet.DateTime)
            .HasComment("创建时间,初始化后不再进行任何变更");

        builder.Property(entity => entity.UpdatedAt).HasColumnType(DataTypeSet.DateTime).HasComment("更新时间,初始为创建时间");

        builder.Property(entity => entity.DeletedAt).HasColumnType(DataTypeSet.DateTime).HasComment("删除时间,启用软删除时生效");
    }

    #endregion
}

/// <summary>
///     ArtemisKeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ArtemisKeySlotConfiguration<TEntity> : ArtemisConfiguration<TEntity>
    where TEntity : class, IKeySlot
{
    #region Overrides of ArtemisConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.Id).HasComment("标识");
    }

    #endregion
}

/// <summary>
///     Artemis抽象类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ArtemisConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    /// <summary>
    ///     数据库类型
    /// </summary>
    protected virtual DbType DbType => DbType.SqlServer;

    /// <summary>
    ///     数据类型集合访问器
    /// </summary>
    protected DataTypeSet DataTypeSet => DataTypeAdapter.GetDataTypeSet(DbType);

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected virtual string DataSetDescription => nameof(TEntity);

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
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void TableConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(nameof(TEntity), table => table.HasComment(DataSetDescription));
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