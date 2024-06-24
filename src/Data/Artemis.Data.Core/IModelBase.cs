namespace Artemis.Data.Core;

#region PartitionBase

/// <summary>
///     基本并发模型接口
/// </summary>
public interface IConcurrencyPartition : IPartitionBase, IConcurrencyPartition<Guid>;

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyPartition<TKey> :
    IPartitionBase<TKey>,
    IConcurrencyPartition<TKey, Guid>
    where TKey : IEquatable<TKey>;

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IConcurrencyPartition<TKey, THandler> :
    IConcurrencyPartition<TKey, THandler, string>,
    IConcurrencyStamp
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public interface IConcurrencyPartition<TKey, THandler, TConcurrencyStamp> :
    IConcurrencyPartition<TKey, THandler, TConcurrencyStamp, int>,
    IPartitionBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public interface IConcurrencyPartition<TKey, THandler, TConcurrencyStamp, TPartition> :
    IPartitionBase<TKey, THandler, TPartition>,
    IConcurrencyStamp<TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>;

/// <summary>
///     基本分区模型
/// </summary>
public interface IPartitionBase : IModelBase, IPartitionBase<Guid>;

/// <summary>
///     基本分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IPartitionBase<TKey> :
    IModelBase<TKey>,
    IPartitionBase<TKey, Guid>
    where TKey : IEquatable<TKey>;

/// <summary>
///     基本分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IPartitionBase<TKey, THandler> :
    IPartitionBase<TKey, THandler, int>,
    IPartitionSlot
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本分区模型接口
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public interface IPartitionBase<TKey, THandler, TPartition> :
    IModelBase<TKey, THandler>, IPartitionSlot<TPartition>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>;

#endregion

#region ModelBase

/// <summary>
///     基本并发模型接口
/// </summary>
public interface IConcurrencyModel : IModelBase, IConcurrencyModel<Guid>;

/// <summary>
///     基本并发模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyModel<TKey> :
    IModelBase<TKey>,
    IConcurrencyModel<TKey, Guid>
    where TKey : IEquatable<TKey>;

/// <summary>
///     基本并发模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IConcurrencyModel<TKey, THandler> :
    IConcurrencyModel<TKey, THandler, string>,
    IConcurrencyStamp
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本并发模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public interface IConcurrencyModel<TKey, THandler, TConcurrencyStamp> : IModelBase<TKey, THandler>,
    IConcurrencyStamp<TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本模型接口
/// </summary>
public interface IModelBase : IModelBase<Guid>, IKeySlot, IHandlerSlot;

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IModelBase<TKey> :
    IModelBase<TKey, Guid>
    where TKey : IEquatable<TKey>;

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public interface IModelBase<TKey, THandler> :
    IMateModelBase<TKey>,
    IHandlerSlot<THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本模型接口
/// </summary>
public interface IMateModelBase : IKeySlot, IMateModelBase<Guid>;

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IMateModelBase<TKey> : IKeySlot<TKey>, IMateSlot where TKey : IEquatable<TKey>;

#endregion

#region Slot&Stamp

#region KeySlot

/// <summary>
///     标识组件接口
/// </summary>
public interface IKeySlot : IKeySlot<Guid>;

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

#endregion

#region HandlerSlot

/// <summary>
///     标记组件接口
/// </summary>
public interface IHandlerSlot : IHandlerSlot<Guid>;

/// <summary>
///     标记组件接口
/// </summary>
/// <typeparam name="THandler"></typeparam>
public interface IHandlerSlot<THandler> where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     创建人
    /// </summary>
    THandler CreateBy { get; set; }

    /// <summary>
    ///     更新人
    /// </summary>
    THandler ModifyBy { get; set; }

    /// <summary>
    ///     移除人
    /// </summary>
    THandler? RemoveBy { get; set; }
}

#endregion

#region Stamp

/// <summary>
///     并发锁组件接口
/// </summary>
public interface IConcurrencyStamp : IConcurrencyStamp<string>;

/// <summary>
///     并发锁组件接口
/// </summary>
public interface IConcurrencyStamp<TConcurrencyStamp>
{
    /// <summary>
    ///     并发锁
    /// </summary>
    TConcurrencyStamp? ConcurrencyStamp { get; set; }
}

/// <summary>
///     检验戳组件接口
/// </summary>
public interface ICheckStamp : ICheckStamp<string>;

/// <summary>
///     检验戳组件接口
/// </summary>
public interface ICheckStamp<TCheckStamp>
{
    /// <summary>
    ///     校验戳
    /// </summary>
    TCheckStamp CheckStamp { get; set; }
}

/// <summary>
///     安全戳
/// </summary>
public interface ISecurityStamp : ISecurityStamp<string>;

/// <summary>
///     安全戳
/// </summary>
/// <typeparam name="TSecurityStamp"></typeparam>
public interface ISecurityStamp<TSecurityStamp>
{
    /// <summary>
    ///     安全戳
    /// </summary>
    TSecurityStamp? SecurityStamp { get; set; }
}

#endregion

#region PartitionSlot

/// <summary>
///     分区组件接口
/// </summary>
public interface IPartitionSlot : IPartitionSlot<int>;

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

#endregion