using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Data.Core;

#region PartitionBase

/// <summary>
///     基本并发分区模型
/// </summary>
public abstract class ConcurrencyPartition : ConcurrencyPartition<Guid>, IConcurrencyPartition;

/// <summary>
///     基本并发分区模型
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyPartition<THandler> :
    ConcurrencyPartition<Guid, THandler>,
    IConcurrencyPartition<THandler>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     基本并发分区模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyPartition<TKey, THandler> :
    ConcurrencyPartition<TKey, THandler, string>,
    IConcurrencyPartition<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     并发锁
    /// </summary>
    public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().IdToString();
}

/// <summary>
///     基本并发分区模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class ConcurrencyPartition<TKey, THandler, TConcurrencyStamp> :
    ConcurrencyPartition<TKey, THandler, TConcurrencyStamp, int>,
    IConcurrencyPartition<TKey, THandler, TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本并发分区模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class ConcurrencyPartition<TKey, THandler, TConcurrencyStamp, TPartition> :
    PartitionBase<TKey, THandler, TPartition>,
    IConcurrencyPartition<TKey, THandler, TConcurrencyStamp, TPartition>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    /// <summary>
    ///     并发锁
    /// </summary>
    public virtual TConcurrencyStamp? ConcurrencyStamp { get; set; }
}

/// <summary>
///     基本分区模型
/// </summary>
public abstract class PartitionBase : PartitionBase<Guid>, IPartitionBase;

/// <summary>
///     基本分区模型
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class PartitionBase<THandler> :
    PartitionBase<Guid, THandler>, IPartitionBase<THandler>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     基本分区模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class PartitionBase<TKey, THandler> :
    PartitionBase<TKey, THandler, int>,
    IPartitionBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>;

/// <summary>
///     基本分区模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TPartition"></typeparam>
public abstract class PartitionBase<TKey, THandler, TPartition> :
    ModelBase<TKey, THandler>,
    IPartitionBase<TKey, THandler, TPartition>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
    where TPartition : IEquatable<TPartition>
{
    /// <summary>
    ///     分区标识
    /// </summary>
    [Required]
    public virtual TPartition Partition { get; set; } = default!;
}

#endregion

#region ModelBase

/// <summary>
///     并发模型
/// </summary>
public abstract class ConcurrencyModel : ConcurrencyModel<Guid>, IConcurrencyModel;

/// <summary>
///     并发模型
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyModel<THandler> :
    ConcurrencyModel<Guid, THandler>,
    IConcurrencyModel<THandler>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     并发模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class ConcurrencyModel<TKey, THandler> :
    ConcurrencyModel<TKey, THandler, string>,
    IConcurrencyModel<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     并发锁
    /// </summary>
    public sealed override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().IdToString();
}

/// <summary>
///     并发模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
/// <typeparam name="TConcurrencyStamp"></typeparam>
public abstract class ConcurrencyModel<TKey, THandler, TConcurrencyStamp> :
    ModelBase<TKey, THandler>,
    IConcurrencyModel<TKey, THandler, TConcurrencyStamp>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     并发锁
    /// </summary>
    public virtual TConcurrencyStamp? ConcurrencyStamp { get; set; }
}

/// <summary>
///     基本模型
/// </summary>
public abstract class ModelBase : ModelBase<Guid>, IModelBase;

/// <summary>
///     基本模型
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class ModelBase<THandler> : ModelBase<Guid, THandler>, IModelBase<THandler>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     基本模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="THandler"></typeparam>
public abstract class ModelBase<TKey, THandler> : MateModelBase<TKey>, IModelBase<TKey, THandler>
    where TKey : IEquatable<TKey>
    where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     创建人
    /// </summary>
    [Required]
    public THandler CreateBy { get; set; } = default!;

    /// <summary>
    ///     更新人
    /// </summary>
    [Required]
    public THandler ModifyBy { get; set; } = default!;

    /// <summary>
    ///     移除人
    /// </summary>
    public THandler? RemoveBy { get; set; }
}

/// <summary>
///     基本模型
/// </summary>
public abstract class MateModelBase : MateModelBase<Guid>, IMateModelBase
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     基本模型
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class MateModelBase<TKey> : MateSlot, IMateModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    public virtual TKey Id { get; set; } = default!;
}

#endregion

#region KeySlot

/// <summary>
///     标识组件
/// </summary>
public abstract class KeySlot : KeySlot<Guid>, IKeySlot
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
///     标识组件
/// </summary>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class KeySlot<TKey> : IKeySlot<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    public virtual TKey Id { get; set; } = default!;
}

#endregion

#region MateSlot

/// <summary>
///     元数据组件
/// </summary>
public abstract class MateSlot : IMateSlot
{
    /// <summary>
    ///     创建时间
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; } = null;
}

#endregion