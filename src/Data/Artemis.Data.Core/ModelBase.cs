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
    public override Guid Id { get; set; } = Guid.NewGuid();

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
    public virtual int Partition { get; set; } = DateTime.Now.Month;

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
    public override Guid Id { get; set; } = Guid.NewGuid();

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

    #region Implementation of IModelBase<TKey>

    /// <summary>
    ///     生成键
    /// </summary>
    [NotMapped]
    public string GenerateKey => Id.ToString()!;

    #endregion
}

/// <summary>
///     分区组件
/// </summary>
public abstract class PartitionSlot : IPartitionSlot
{
    #region Implementation of IPartitionSlot

    /// <summary>
    ///     分区标识
    /// </summary>
    public virtual int Partition { get; set; } = 0;

    #endregion
}

/// <summary>
///     标识组件
/// </summary>
public abstract class KeySlot : KeySlot<Guid>, IKeySlot
{
    #region Overrides of KeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override Guid Id { get; set; } = Guid.NewGuid();

    #endregion
}

/// <summary>
///     标识组件
/// </summary>
/// <typeparam name="TKey">记录标识</typeparam>
public abstract class KeySlot<TKey> : IKeySlot<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of IKeySlot<TKey>

    /// <summary>
    ///     存储标识
    /// </summary>
    [Key]
    public virtual TKey Id { get; set; } = default!;

    #endregion
}

/// <summary>
///     元数据组件
/// </summary>
public abstract class MateSlot : IMateSlot
{
    #region Implementation of IMateSlot

    /// <summary>
    ///     创建时间
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     更新时间
    /// </summary>
    public virtual DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     删除时间
    /// </summary>
    public virtual DateTime? DeletedAt { get; set; }

    #endregion
}