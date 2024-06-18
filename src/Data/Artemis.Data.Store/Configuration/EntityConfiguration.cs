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
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity, THandler> :
    ConcurrencyPartitionEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IConcurrencyPartition<THandler>
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity, TKey, THandler> :
    ConcurrencyPartitionEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IConcurrencyPartition<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    ConcurrencyPartitionEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IConcurrencyPartition<TKey, THandler, TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class ConcurrencyPartitionEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    PartitionBaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IConcurrencyPartition<TKey, THandler, TConcurrencyStamp, TPartition>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
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
/// <typeparam name="THandler"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity, THandler> :
    PartitionBaseEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IPartitionBase<THandler>
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity, TKey, THandler> :
    PartitionBaseEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IPartitionBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    PartitionBaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IPartitionBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class PartitionBaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    BaseModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IPartitionBase<TKey, THandler, TPartition>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Config

    /// <summary>
    ///     启用分区配置
    /// </summary>
    protected sealed override bool UsePartitionConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
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
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity, THandler> :
    ConcurrencyModelEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IConcurrencyModel<THandler>
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity, TKey, THandler> :
    ConcurrencyModelEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IConcurrencyModel<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    ConcurrencyModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IConcurrencyModel<TKey, THandler, TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.Property(entity => ((IPartitionSlot)entity).Partition)
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
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class ConcurrencyModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    BaseModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IConcurrencyModel<TKey, THandler, TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
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
/// <typeparam name="THandler"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity, THandler> :
    BaseModelEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IModelBase<THandler>
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity, TKey, THandler> :
    BaseModelEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IModelBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    BaseModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IModelBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.Property(entity => ((IPartitionSlot)entity).Partition)
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
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class BaseModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    MateModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IModelBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Config

    /// <summary>
    ///     是否启用操作者配置
    /// </summary>
    protected sealed override bool UseHandlerFieldConfiguration => true;


    /// <summary>
    ///     启用元数据索引配置
    /// </summary>
    protected sealed override bool UseHandlerIndexConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(entity => entity.CreateBy)
            .HasComment("创建者标识")
            .HasColumnType(DataTypeSet.Guid);

        builder.Property(entity => entity.ModifyBy)
            .HasComment("更新者标识")
            .HasColumnType(DataTypeSet.Guid);

        builder.Property(entity => entity.RemoveBy)
            .HasComment("删除者标识")
            .HasColumnType(DataTypeSet.Guid);
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
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

#endregion

#region MateModelEntityConfiguration

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class MateModelEntityConfiguration<TEntity> :
    MateModelEntityConfiguration<TEntity, Guid>
    where TEntity : class, IMateModelBase
{
    #region Slot

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerFieldConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
            {
                builder.Property(entity => ((IHandlerSlot)entity).CreateBy)
                    .HasComment("创建者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).ModifyBy)
                    .HasComment("更新者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).RemoveBy)
                    .HasComment("删除者标识")
                    .HasColumnType(DataTypeSet.Guid);
            }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
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
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class MateModelEntityConfiguration<TEntity, THandler> :
    MateModelEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IMateModelBase
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class MateModelEntityConfiguration<TEntity, TKey, THandler> :
    MateModelEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IMateModelBase<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class MateModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    MateModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IMateModelBase<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.Property(entity => ((IPartitionSlot)entity).Partition)
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
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     元数据模型类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class MateModelEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    KeySlotEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IMateModelBase<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Config

    /// <summary>
    ///     启用元数据字段配置
    /// </summary>
    protected sealed override bool UseMateFieldConfiguration => true;

    /// <summary>
    ///     启用元数据索引配置
    /// </summary>
    protected sealed override bool UseMateIndexConfiguration => true;

    #endregion
}

#endregion

#region KeySlotEntityConfiguration

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity> :
    KeySlotEntityConfiguration<TEntity, Guid>
    where TEntity : class, IKeySlot
{
    #region Slot

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerFieldConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
            {
                builder.Property(entity => ((IHandlerSlot)entity).CreateBy)
                    .HasComment("创建者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).ModifyBy)
                    .HasComment("更新者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).RemoveBy)
                    .HasComment("删除者标识")
                    .HasColumnType(DataTypeSet.Guid);
            }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
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
}

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity, THandler> :
    KeySlotEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class, IKeySlot
    where THandler : IEquatable<THandler>;

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity, TKey, THandler> :
    KeySlotEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    KeySlotEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.Property(entity => ((IPartitionSlot)entity).Partition)
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
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     KeySlot类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class KeySlotEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> :
    BaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition>
    where TEntity : class, IKeySlot<TKey>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    #region Config

    /// <summary>
    ///     启用键配置
    /// </summary>
    protected sealed override bool UseKeyConfiguration => true;

    #endregion

    #region Slot

    /// <summary>
    ///     键插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void KeySlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
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

    /// <summary>
    ///     操作者插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerFieldConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
            {
                builder.Property(entity => ((IHandlerSlot)entity).CreateBy)
                    .HasComment("创建者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).ModifyBy)
                    .HasComment("更新者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot)entity).RemoveBy)
                    .HasComment("删除者标识")
                    .HasColumnType(DataTypeSet.Guid);
            }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot)))
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
}

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class BaseEntityConfiguration<TEntity, THandler> :
    BaseEntityConfiguration<TEntity, Guid, THandler>
    where TEntity : class
    where THandler : IEquatable<THandler>
{
    #region Slot

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
}

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class BaseEntityConfiguration<TEntity, TKey, THandler> :
    BaseEntityConfiguration<TEntity, TKey, THandler, string>
    where TEntity : class
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp)))
            builder.Property(entity => ((IConcurrencyStamp)entity).ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasComment("并发锁");
    }

    #endregion
}

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class BaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp> :
    BaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, int>
    where TEntity : class
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    #region Slot

    /// <summary>
    ///     分区标识插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected sealed override void PartitionSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.Property(entity => ((IPartitionSlot)entity).Partition)
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
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot)))
                builder.HasIndex(entity => ((IPartitionSlot)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion
}

/// <summary>
///     抽象实体类型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class
    BaseEntityConfiguration<TEntity, TKey, THandler, TConcurrencyStamp, TPartition> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
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
    protected string KeyName => $"PK_{TableName}";

    /// <summary>
    ///     生成索引名称
    /// </summary>
    /// <param name="properties">字段名称</param>
    /// <returns></returns>
    protected string IndexName(params string[] properties)
    {
        return $"IX_{TableName}_{string.Join('_', properties)}";
    }

    /// <summary>
    ///     生成外键名称
    /// </summary>
    /// <param name="subTableName"></param>
    /// <param name="mainTableName"></param>
    /// <returns></returns>
    protected string ForeignKeyName(string subTableName, string mainTableName)
    {
        return $"FK_{subTableName}_{mainTableName}";
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

        ConcurrencyStampFieldConfiguration(builder);

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

        PartitionSlotRelationConfiguration(builder);

        MateSlotRelationConfiguration(builder);

        HandlerSlotRelationConfiguration(builder);
    }

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void EntityFieldConfigure(EntityTypeBuilder<TEntity> builder);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void EntityRelationConfigure(EntityTypeBuilder<TEntity> builder);

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
    ///     是否启用元数据字段配置
    /// </summary>
    protected virtual bool UseMateFieldConfiguration => false;

    /// <summary>
    ///     是否启用元数据索引配置
    /// </summary>
    protected virtual bool UseMateIndexConfiguration => false;

    /// <summary>
    ///     是否启用操作者配置
    /// </summary>
    protected virtual bool UseHandlerFieldConfiguration => false;

    /// <summary>
    ///     是否启用操作者配置
    /// </summary>
    protected virtual bool UseHandlerIndexConfiguration => false;

    #endregion

    #region Slot

    #region ConcurrencyStamp

    /// <summary>
    ///     并发锁字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void ConcurrencyStampFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (Generator.IsInherit<TEntity>(typeof(IConcurrencyStamp<TConcurrencyStamp>)))
            builder.Property(entity => ((IConcurrencyStamp<TConcurrencyStamp>)entity).ConcurrencyStamp)
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
        if (UseKeyConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IKeySlot<TKey>)))
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
        if (UseKeyConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IKeySlot<TKey>)))
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
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot<TPartition>)))
                builder.Property(entity => ((IPartitionSlot<TPartition>)entity).Partition)
                    .IsRequired()
                    .HasComment("分区标识");
    }

    /// <summary>
    ///     分区标识插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void PartitionSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UsePartitionConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IPartitionSlot<TPartition>)))
                builder.HasIndex(entity => ((IPartitionSlot<TPartition>)entity).Partition)
                    .HasDatabaseName(IndexName("Partition"));
    }

    #endregion

    #region MateSlot

    /// <summary>
    ///     元数据插件字段配置
    /// </summary>
    /// <param name="builder"></param>
    private void MateSlotFieldConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateFieldConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
            {
                builder.Property(entity => ((IMateSlot)entity).CreatedAt)
                    .HasComment("创建时间")
                    .HasColumnType(DataTypeSet.DateTime);

                builder.Property(entity => ((IMateSlot)entity).UpdatedAt)
                    .HasComment("更新时间")
                    .HasColumnType(DataTypeSet.DateTime);

                builder.Property(entity => ((IMateSlot)entity).DeletedAt)
                    .HasComment("删除时间")
                    .HasColumnType(DataTypeSet.DateTime);
            }
    }

    /// <summary>
    ///     元数据插件索引配置
    /// </summary>
    /// <param name="builder"></param>
    private void MateSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseMateIndexConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IMateSlot)))
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
        if (UseHandlerFieldConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
            {
                builder.Property(entity => ((IHandlerSlot<THandler>)entity).CreateBy)
                    .HasComment("创建者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot<THandler>)entity).ModifyBy)
                    .HasComment("更新者标识")
                    .HasColumnType(DataTypeSet.Guid);

                builder.Property(entity => ((IHandlerSlot<THandler>)entity).RemoveBy)
                    .HasComment("删除者标识")
                    .HasColumnType(DataTypeSet.Guid);
            }
    }

    /// <summary>
    ///     操作者插件关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void HandlerSlotRelationConfiguration(EntityTypeBuilder<TEntity> builder)
    {
        if (UseHandlerIndexConfiguration)
            if (Generator.IsInherit<TEntity>(typeof(IHandlerSlot<THandler>)))
            {
                builder.HasIndex(entity => ((IHandlerSlot<THandler>)entity).CreateBy)
                    .HasDatabaseName(IndexName("CreateBy"));
                builder.HasIndex(entity => ((IHandlerSlot<THandler>)entity).ModifyBy)
                    .HasDatabaseName(IndexName("ModifyBy"));
                builder.HasIndex(entity => ((IHandlerSlot<THandler>)entity).RemoveBy)
                    .HasDatabaseName(IndexName("RemoveBy"));
            }
    }

    #endregion

    #endregion
}

#endregion