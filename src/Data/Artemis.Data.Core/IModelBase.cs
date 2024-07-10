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
public interface IConcurrencyPartition<TKey> : IPartitionBase<TKey>, IConcurrencyStamp where TKey : IEquatable<TKey>;

/// <summary>
///     基本分区模型
/// </summary>
public interface IPartitionBase : IModelBase, IPartitionBase<Guid>;

/// <summary>
///     基本分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IPartitionBase<TKey> : IModelBase<TKey>, IPartitionSlot where TKey : IEquatable<TKey>;

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
public interface IConcurrencyModel<TKey> : IModelBase<TKey>, IConcurrencyStamp where TKey : IEquatable<TKey>;

/// <summary>
///     基本模型接口
/// </summary>
public interface IModelBase : IModelBase<Guid>, IKeySlot;

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IModelBase<TKey> : IKeySlot<TKey>, IMateSlot, IHandlerSlot where TKey : IEquatable<TKey>;

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
public interface IHandlerSlot
{
    /// <summary>
    ///     创建人
    /// </summary>
    string CreateBy { get; set; }

    /// <summary>
    ///     更新人
    /// </summary>
    string ModifyBy { get; set; }

    /// <summary>
    ///     移除人
    /// </summary>
    string? RemoveBy { get; set; }
}

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

/// <summary>
///     安全戳
/// </summary>
public interface ISecurityStamp
{
    /// <summary>
    ///     安全戳
    /// </summary>
    string? SecurityStamp { get; set; }
}

/// <summary>
///     分区组件接口
/// </summary>
public interface IPartitionSlot
{
    /// <summary>
    ///     分区标识
    /// </summary>
    int Partition { get; set; }
}

#endregion