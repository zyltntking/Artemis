namespace Artemis.Data.Core;

#region PartitionBase

/// <summary>
///     基本并发分区模型
/// </summary>
public interface IConcurrencyPartitionBase : IPartitionBase, IConcurrencyPartitionBase<Guid>, IConcurrencyModelBase
{
}

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyPartitionBase<TKey> : IPartitionBase<TKey>, IConcurrencyModelBase<TKey>
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     基本分区模型
/// </summary>
public interface IPartitionBase : IModelBase, IPartitionBase<Guid>
{
}

/// <summary>
///     基本分区模型
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IPartitionBase<TKey> : IModelBase<TKey>, IPartitionSlot
    where TKey : IEquatable<TKey>
{
}

#endregion

#region ModelBase

/// <summary>
///     基本并发模型接口
/// </summary>
public interface IConcurrencyModelBase : IModelBase, IConcurrencyModelBase<Guid>
{
}

/// <summary>
///     基本并发模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyModelBase<TKey> : IModelBase<TKey>, IConcurrencyStamp
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     基本模型接口
/// </summary>
public interface IModelBase : IKeySlot, IModelBase<Guid>, IMarkSlot
{
}

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IModelBase<TKey> : IKeySlot<TKey>, IMateSlot, IMarkSlot<TKey>
    where TKey : IEquatable<TKey>
{
}

#endregion

#region KeySlot

/// <summary>
///     标识组件接口
/// </summary>
public interface IKeySlot : IKeySlot<Guid>
{
}

/// <summary>
///     标识组件接口
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IKeySlot<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    TKey Id { get; set; }
}

#endregion

#region MateSlot

/// <summary>
///     元数据组件接口
/// </summary>
public interface IMateSlot
{
    /// <summary>
    ///     创建时间
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     删除时间
    /// </summary>
    DateTime? DeletedAt { get; set; }
}

/// <summary>
///     标记组件接口
/// </summary>
public interface IMarkSlot : IMarkSlot<Guid>
{
}

/// <summary>
///     标记组件接口
/// </summary>
/// <typeparam name="TMark"></typeparam>
public interface IMarkSlot<TMark> where TMark : IEquatable<TMark>
{
    /// <summary>
    ///     创建人
    /// </summary>
    TMark CreateBy { get; set; }

    /// <summary>
    ///     更新人
    /// </summary>
    TMark ModifyBy { get; set; }

    /// <summary>
    ///     移除人
    /// </summary>
    TMark? RemoveBy { get; set; }
}

#endregion

#region Stamp

/// <summary>
///     并发锁组件接口
/// </summary>
public interface IConcurrencyStamp
{
    /// <summary>
    ///     并发锁
    /// </summary>
    string? ConcurrencyStamp { get; set; }
}

/// <summary>
///     检验戳组件接口
/// </summary>
public interface ICheckStamp
{
    /// <summary>
    ///     校验戳
    /// </summary>
    string CheckStamp { get; set; }
}

#endregion

#region PartitionSlot

/// <summary>
///     分区组件接口
/// </summary>
public interface IPartitionSlot : IPartitionSlot<int>
{
}

/// <summary>
///     分区组件接口
/// </summary>
/// <typeparam name="TPartition">分区标识类型</typeparam>
public interface IPartitionSlot<TPartition> where TPartition : IEquatable<TPartition>
{
    /// <summary>
    ///     分区标识
    /// </summary>
    TPartition Partition { get; set; }
}

#endregion