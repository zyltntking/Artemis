using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artemis.Data.Core;

/// <summary>
///     分区模型
/// </summary>
public abstract class PartitionBase : PartitionBase<Guid>, IPartitionBase
{
    #region Overrides of ModelBase<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();

    #endregion
}

/// <summary>
///     分区模型
/// </summary>
/// <typeparam name="TKey">记录标识</typeparam>
public abstract class PartitionBase<TKey> : ModelBase<TKey>, IPartitionBase<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of IPartitionSlot

    /// <summary>
    ///     分区标识
    /// </summary>
    public int Partition { get; set; } = 0;

    #endregion
}

/// <summary>
///     基本模型
/// </summary>
public abstract class ModelBase : ModelBase<Guid>, IModelBase
{
    #region Overrides of ModelBase<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public sealed override Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     创建人
    /// </summary>
    [Required]
    public sealed override Guid CreateBy { get; set; } = Guid.Empty;

    /// <summary>
    ///     更新人
    /// </summary>
    [Required]
    public sealed override Guid ModifyBy { get; set; } = Guid.Empty;

    #endregion
}

/// <summary>
///     基本模型
/// </summary>
/// <typeparam name="TKey">记录标识</typeparam>
public abstract class ModelBase<TKey> : MateSlot, IModelBase<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of IKeySlot<TKey>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    public virtual TKey Id { get; set; } = default!;

    #endregion

    #region Implementation of IHandlerSlot<TKey>

    /// <summary>
    ///     创建人
    /// </summary>
    [Required]
    public virtual TKey CreateBy { get; set; } = default!;

    /// <summary>
    ///     更新人
    /// </summary>
    [Required]
    public virtual TKey ModifyBy { get; set; } = default!;

    /// <summary>
    ///     移除人
    /// </summary>
    public virtual TKey? RemoveBy { get; set; }

    #endregion
}

#region KeySlot

/// <summary>
///     标识组件
/// </summary>
public abstract class KeySlot : IKeySlot
{
    #region Overrides of KeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();

    #endregion
}

#endregion

#region MateSlot

/// <summary>
///     元数据组件
/// </summary>
public abstract class MateSlot : IMateSlot
{
    #region Implementation of IMateSlot

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

    #endregion
}

#endregion