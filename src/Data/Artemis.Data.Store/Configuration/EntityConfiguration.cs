using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store.Configuration;

#region ConcurrencyPartitionEntityConfiguration

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity> :
    ConcurrencyPartitionEntityConfiguration<TEntity, Guid>
    where TEntity : class, IConcurrencyPartition;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity, TKey> :
    PartitionBaseEntityConfiguration<TEntity, TKey>
    where TEntity : class, IConcurrencyPartition<TKey>
    where TKey : IEquatable<TKey>
{
    #region Config

    /// <summary>
    ///     启用并发戳字段配置
    /// </summary>
    protected override bool UseConcurrencyStampFieldConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseConcurrencyStampFieldConfiguration)
            builder.Property(entity => entity.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

#endregion

#region PartitionBaseEntityConfiguration

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity> :
    PartitionBaseEntityConfiguration<TEntity, Guid>
    where TEntity : class, IPartitionBase;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity, TKey> :
    BaseModelEntityConfiguration<TEntity, TKey>
    where TEntity : class, IPartitionBase<TKey>
    where TKey : IEquatable<TKey>
{
    #region Config

    /// <summary>
    ///     启用分区配置
    /// </summary>
    protected override bool UsePartitionConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            builder.Property(entity => entity.Partition)
                .IsRequired()
                .HasComment("分区标识");
    }

    /// <summary>
    ///     分区标识插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            builder.HasIndex(entity => entity.Partition)
                .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

#endregion

#region ConcurrencyModelEntityConfiguration

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity> :
    ConcurrencyModelEntityConfiguration<TEntity, Guid>
    where TEntity : class, IConcurrencyModel;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity, TKey> : BaseModelEntityConfiguration<TEntity, TKey>
    where TEntity : class, IConcurrencyModel<TKey>
    where TKey : IEquatable<TKey>
{
    #region Config

    /// <summary>
    ///     启用并发戳字段配置
    /// </summary>
    protected override bool UseConcurrencyStampFieldConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseConcurrencyStampFieldConfiguration)
            builder.Property(entity => entity.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

#endregion

#region BaseModelEntityConfiguration

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity> :
    BaseModelEntityConfiguration<TEntity, Guid>
    where TEntity : class, IModelBase;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity, TKey> : KeySlotEntityConfiguration<TEntity, TKey>
    where TEntity : class, IModelBase<TKey>
    where TKey : IEquatable<TKey>
{
    #region Config

    /// <summary>
    ///     启用操作者索引配置
    /// </summary>
    protected override bool UseHandlerIndexConfiguration => true;

    /// <summary>
    ///     启用操作者字段配置
    /// </summary>
    protected override bool UseHandlerFieldConfiguration => true;

    /// <summary>
    ///     启用元数据索引配置
    /// </summary>
    protected override bool UseMateIndexConfiguration => true;

    /// <summary>
    ///     启用元数据字段配置
    /// </summary>
    protected override bool UseMateFieldConfiguration => true;

    #endregion

    #region Slot

    #region MateSlot

    /// <summary>
    ///     元数据插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void MateSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateFieldConfiguration)
        {
            builder.Property(entity => entity.CreatedAt)
                .HasComment("创建时间")
                .IsRequired();

            builder.Property(entity => entity.UpdatedAt)
                .HasComment("更新时间")
                .IsRequired();

            builder.Property(entity => entity.DeletedAt)
                .HasComment("删除时间");
        }
    }

    /// <summary>
    ///     元数据插件索引配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void MateSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateIndexConfiguration && Generator.IsInherit<TEntity>(typeof(IMateSlot)))
        {
            builder.HasIndex(entity => entity.CreatedAt)
                .HasDatabaseName(IndexName("CreatedAt"));
            builder.HasIndex(entity => entity.UpdatedAt)
                .HasDatabaseName(IndexName("UpdatedAt"));
            builder.HasIndex(entity => entity.DeletedAt)
                .HasDatabaseName(IndexName("DeletedAt"));
        }
    }

    #endregion

    #region HandlerSlot

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerFieldConfiguration)
        {
            builder.Property(entity => entity.CreateBy)
                .HasComment("创建者标识")
                .IsRequired();

            builder.Property(entity => entity.ModifyBy)
                .HasComment("更新者标识")
                .IsRequired();

            builder.Property(entity => entity.RemoveBy)
                .HasComment("删除者标识");
        }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration)
        {
            builder.HasIndex(entity => entity.CreateBy)
                .HasDatabaseName(IndexName("CreateBy"));
            builder.HasIndex(entity => entity.ModifyBy)
                .HasDatabaseName(IndexName("ModifyBy"));
            builder.HasIndex(entity => entity.RemoveBy)
                .HasDatabaseName(IndexName("RemoveBy"));
        }
    }

    #endregion

    #endregion
}

#endregion

#region KeySlotEntityConfiguration

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity> : KeySlotEntityConfiguration<TEntity, Guid>
    where TEntity : class, IKeySlot;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity, TKey> : BaseEntityConfiguration<TEntity, TKey>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
{
    #region Config

    /// <summary>
    ///     启用键配置
    /// </summary>
    protected override bool UseKeyConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     键插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void KeySlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration)
            builder.Property(entity => entity.Id)
                .IsRequired()
                .HasComment("标识");
    }

    /// <summary>
    ///     键插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void KeySlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration)
            builder.HasKey(entity => entity.Id)
                .HasName(KeyName);
    }

    #endregion
}

#endregion

#region BaseEntityConfiguration

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseEntityConfiguration<TEntity> :
    BaseEntityConfiguration<TEntity, Guid>
    where TEntity : class
{
    #region Slot

    #region KeySlot

    /// <summary>
    ///     键插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void KeySlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IKeySlot)))
                builder.Property(entity => ((IKeySlot)entity).Id)
                    .IsRequired()
                    .HasComment("标识");
    }

    /// <summary>
    ///     键插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void KeySlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IKeySlot)))
                builder.HasKey(entity => ((IKeySlot)entity).Id)
                    .HasName(KeyName);
    }

    #endregion

    #endregion
}

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
    where TKey : IEquatable<TKey>
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
    protected string KeyName => TableName.KeyName();

    /// <summary>
    ///     生成索引名称
    /// </summary>
    /// <param name="properties">字段名称</param>
    /// <returns></returns>
    protected string IndexName(params string[] properties)
    {
        return TableName.IndexName(properties);
    }

    /// <summary>
    ///     生成外键名称
    /// </summary>
    /// <param name="subTableName"></param>
    /// <param name="mainTableName"></param>
    /// <returns></returns>
    protected string ForeignKeyName(string subTableName, string mainTableName)
    {
        return subTableName.ForeignKeyName(mainTableName);
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
    private void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        KeySlotFieldConfiguration(builder);

        EntityFieldConfigure(builder);

        SecurityStampFieldConfiguration(builder);

        ConcurrencyStampFieldConfiguration(builder);

        CheckStampFieldConfiguration(builder);

        PartitionSlotFieldConfiguration(builder);

        MateSlotFieldConfiguration(builder);

        HandlerSlotFieldConfiguration(builder);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    private void RelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
        KeySlotRelationConfiguration(builder);

        EntityRelationConfigure(builder);

        CheckStampRelationConfiguration(builder);

        PartitionSlotRelationConfiguration(builder);

        MateSlotRelationConfiguration(builder);

        HandlerSlotRelationConfiguration(builder);
    }

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void EntityFieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void EntityRelationConfigure(EntityTypeBuilder<TEntity> builder)
    {
    }

    #region SlotConfig

    /// <summary>
    ///     启用键配置
    /// </summary>
    protected virtual bool UseKeyConfiguration => false;

    /// <summary>
    ///     启用分区配置
    /// </summary>
    protected virtual bool UsePartitionConfiguration => false;

    /// <summary>
    ///     启用校验戳配置
    /// </summary>
    protected virtual bool UseCheckStampConfiguration => false;

    /// <summary>
    ///     启用安全戳配置
    /// </summary>
    protected virtual bool UseSecurityStampFieldConfiguration => false;

    /// <summary>
    ///     启用并发戳字段配置
    /// </summary>
    protected virtual bool UseConcurrencyStampFieldConfiguration => false;

    /// <summary>
    ///     启用元数据字段配置
    /// </summary>
    protected virtual bool UseMateFieldConfiguration => false;

    /// <summary>
    ///     启用元数据索引配置
    /// </summary>
    protected virtual bool UseMateIndexConfiguration => false;

    /// <summary>
    ///     启用操作者配置
    /// </summary>
    protected virtual bool UseHandlerFieldConfiguration => false;

    /// <summary>
    ///     启用操作者配置
    /// </summary>
    protected virtual bool UseHandlerIndexConfiguration => false;

    #endregion

    #region Slot

    #region SecurityStamp

    /// <summary>
    ///     加密戳插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void SecurityStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseSecurityStampFieldConfiguration && Generator.IsInherit<TEntity>(typeof(ISecurityStamp)))
            builder.Property(entity => ((ISecurityStamp)entity).SecurityStamp)
                .HasComment("加密戳")
                .IsRequired();
    }

    #endregion

    #region CheckStamp

    /// <summary>
    ///     校验戳插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void CheckStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseCheckStampConfiguration && Generator.IsInherit<TEntity>(typeof(ICheckStamp)))
            builder.Property(entity => ((ICheckStamp)entity).CheckStamp)
                .HasComment("校验戳")
                .IsRequired();
    }

    /// <summary>
    ///     校验戳插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    private void CheckStampRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseCheckStampConfiguration && Generator.IsInherit<TEntity>(typeof(ICheckStamp)))
            builder.HasIndex(entity => ((ICheckStamp)entity).CheckStamp)
                .HasDatabaseName(IndexName("CheckStamp"));
    }

    #endregion

    #region ConcurrencyStamp

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseConcurrencyStampFieldConfiguration && Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion

    #region KeySlot

    /// <summary>
    ///     键插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void KeySlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration && Generator.IsInherit<TEntity>(typeof(IKeySlot<TKey>)))
            builder.Property(entity => ((IKeySlot<TKey>)entity).Id)
                .IsRequired()
                .HasComment("标识");
    }

    /// <summary>
    ///     键插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void KeySlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseKeyConfiguration && Generator.IsInherit<TEntity>(typeof(IKeySlot<TKey>)))
            builder.HasKey(entity => ((IKeySlot<TKey>)entity).Id)
                .HasName(KeyName);
    }

    #endregion

    #region ParttionSlot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration && Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
            builder.Property(entity => ((IPartitionSlot)entity).Partition)
                .HasComment("分区标识")
                .IsRequired();
    }

    /// <summary>
    ///     分区标识插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void PartitionSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration && Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
            builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                .HasDatabaseName(IndexName("Partition"));
    }

    #endregion

    #region MateSlot

    /// <summary>
    ///     元数据插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void MateSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateFieldConfiguration && Generator.IsInherit<TEntity>(typeof(IMateSlot)))
        {
            builder.Property(entity => ((IMateSlot)entity).CreatedAt)
                .HasComment("创建时间")
                .IsRequired();

            builder.Property(entity => ((IMateSlot)entity).UpdatedAt)
                .HasComment("更新时间")
                .IsRequired();

            builder.Property(entity => ((IMateSlot)entity).DeletedAt)
                .HasComment("删除时间");
        }
    }

    /// <summary>
    ///     元数据插件索引配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void MateSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateIndexConfiguration && Generator.IsInherit<TEntity>(typeof(IMateSlot)))
        {
            builder.HasIndex(entity => ((IMateSlot)entity).CreatedAt)
                .HasDatabaseName(IndexName("CreatedAt"));
            builder.HasIndex(entity => ((IMateSlot)entity).UpdatedAt)
                .HasDatabaseName(IndexName("UpdatedAt"));
            builder.HasIndex(entity => ((IMateSlot)entity).DeletedAt)
                .HasDatabaseName(IndexName("DeletedAt"));
        }
    }

    #endregion

    #region HandlerSlot

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerFieldConfiguration && Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
        {
            builder.Property(entity => ((IHandlerSlot)entity).CreateBy)
                .HasComment("创建者标识")
                .IsRequired();

            builder.Property(entity => ((IHandlerSlot)entity).ModifyBy)
                .HasComment("更新者标识")
                .IsRequired();

            builder.Property(entity => ((IHandlerSlot)entity).RemoveBy)
                .HasComment("删除者标识");
        }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration && Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
        {
            builder.HasIndex(entity => ((IHandlerSlot)entity).CreateBy)
                .HasDatabaseName(IndexName("CreateBy"));
            builder.HasIndex(entity => ((IHandlerSlot)entity).ModifyBy)
                .HasDatabaseName(IndexName("ModifyBy"));
            builder.HasIndex(entity => ((IHandlerSlot)entity).RemoveBy)
                .HasDatabaseName(IndexName("RemoveBy"));
        }
    }

    #endregion

    #endregion
}

#endregion