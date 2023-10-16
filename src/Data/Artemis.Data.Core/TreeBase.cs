using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     节点结构接口
/// </summary>
public interface INode : INode<Guid>, IKeySlot
{
}

/// <summary>
///     节点结构接口
/// </summary>
/// <typeparam name="TKey">键类型</typeparam>
public interface INode<TKey> : IKeySlot<TKey>
    where TKey : IEquatable<TKey>

{
    /// <summary>
    ///     父节点键
    /// </summary>
    TKey ParentId { get; set; }
}

/// <summary>
///     基本树结构接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ITree<TEntity> : ITree<TEntity, Guid>
    where TEntity : IKeySlot<Guid>
{
}

/// <summary>
///     基本树结构接口
/// </summary>
/// <typeparam name="TKey">键类型</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ITree<TEntity, TKey>
    where TKey : IEquatable<TKey>
    where TEntity : IKeySlot<TKey>
{
    /// <summary>
    ///     当前节点
    /// </summary>
    TEntity Node { get; set; }

    /// <summary>
    ///     子节点集合
    /// </summary>
    ICollection<ITree<TEntity, TKey>>? Children { get; set; }
}

#endregion

#region Record

/// <summary>
///     树结构数据
/// </summary>
[DataContract]
public abstract record Node : Node<Guid>, INode
{
    #region Overrides of Node<Guid>

    /// <summary>
    ///     节点键
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required Guid Id { get; set; }

    /// <summary>
    ///     父节点键
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required Guid ParentId { get; set; }

    #endregion
}

/// <summary>
///     树结构数据
/// </summary>
/// <typeparam name="TKey">标识类型</typeparam>
[DataContract]
public abstract record Node<TKey> : INode<TKey>
    where TKey : IEquatable<TKey>
{
    #region Implementation of INode<TKey>

    /// <summary>
    ///     节点键
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required TKey Id { get; set; }

    /// <summary>
    ///     父节点键
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required TKey ParentId { get; set; }

    #endregion
}

/// <summary>
///     基本树结构
/// </summary>
/// <typeparam name="TEntity"></typeparam>
[DataContract]
public record Tree<TEntity> : Tree<TEntity, Guid>, ITree<TEntity>
    where TEntity : IKeySlot<Guid>
{
    #region Overrides of TreeBase<Guid,TEntity>

    /// <summary>
    ///     当前节点
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required TEntity Node { get; set; }

    /// <summary>
    ///     子节点集合
    /// </summary>
    [DataMember(Order = 2)]
    public override ICollection<ITree<TEntity, Guid>>? Children { get; set; }

    #endregion
}

/// <summary>
///     基本树结构
/// </summary>
/// <typeparam name="TKey">键</typeparam>
/// <typeparam name="TEntity">实体类型</typeparam>
[DataContract]
public record Tree<TEntity, TKey> : ITree<TEntity, TKey>
    where TKey : IEquatable<TKey>
    where TEntity : IKeySlot<TKey>
{
    #region Implementation of ITreeBase<TKey,TEntity>

    /// <summary>
    ///     当前节点
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required TEntity Node { get; set; }

    /// <summary>
    ///     子节点集合
    /// </summary>
    [DataMember(Order = 2)]
    public virtual ICollection<ITree<TEntity, TKey>>? Children { get; set; }

    #endregion
}

#endregion

#region Extinsions

/// <summary>
///     树扩展
/// </summary>
public static class TreeExtensions
{
}

#endregion