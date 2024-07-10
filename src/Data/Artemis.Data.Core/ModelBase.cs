using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Data.Core;

#region PartitionBase

/// <summary>
///     基本并发分区模型
/// </summary>
public abstract class ConcurrencyPartition : ConcurrencyPartition<Guid>, IConcurrencyPartition
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
public abstract class ConcurrencyPartition<TKey> : PartitionBase<TKey>, IConcurrencyPartition<TKey>
    where TKey : IEquatable<TKey>
{
    #region Implementation of IConcurrencyStamp

    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    public string? ConcurrencyStamp { get; set; }

    #endregion
}

/// <summary>
///     基本分区模型
/// </summary>
public abstract class PartitionBase : PartitionBase<Guid>, IPartitionBase
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
public abstract class PartitionBase<TKey> : ModelBase<TKey>, IPartitionBase<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of IPartitionSlot

    /// <summary>
    ///     分区标识
    /// </summary>
    [Required]
    public int Partition { get; set; }

    #endregion
}

#endregion

#region ModelBase

/// <summary>
///     并发模型
/// </summary>
public abstract class ConcurrencyModel : ConcurrencyModel<Guid>, IConcurrencyModel
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
public abstract class ConcurrencyModel<TKey> : ModelBase<TKey>, IConcurrencyModel<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of IConcurrencyStamp

    /// <summary>
    ///     并发锁
    /// </summary>
    [MaxLength(64)]
    public string? ConcurrencyStamp { get; set; }

    #endregion
}

/// <summary>
///     基本模型
/// </summary>
public abstract class ModelBase : ModelBase<Guid>, IModelBase
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
public abstract class ModelBase<TKey> : MateModelBase<TKey>, IHandlerSlot where TKey : IEquatable<TKey>
{
    #region Implementation of IHandlerSlot

    /// <summary>
    ///     创建人
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string CreateBy { get; set; } = null!;

    /// <summary>
    ///     更新人
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string ModifyBy { get; set; } = null!;

    /// <summary>
    ///     移除人
    /// </summary>
    [MaxLength(64)]
    public string? RemoveBy { get; set; }

    #endregion
}

/// <summary>
///     基本模型
/// </summary>
public abstract class MateModelBase : MateModelBase<Guid>, IKeySlot
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
public abstract class MateModelBase<TKey> : MateSlot, IKeySlot<TKey> where TKey : IEquatable<TKey>
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