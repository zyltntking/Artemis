namespace Artemis.Data.Core.Fundamental.Model;

#region Interface

/// <summary>
///     节点接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface INodeInfo<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     标识
    /// </summary>
    TKey Id { get; set; }

    /// <summary>
    ///     名称
    /// </summary>
    string Name { get; set; }
}

/// <summary>
///     节点接口
/// </summary>
public interface INodeInfo : INodeInfo<Guid>
{
}

/// <summary>
///     树节点接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface ITreeNodeInfo<TKey> : INodeInfo<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     父节点
    /// </summary>
    public TKey ParentId { get; set; }
}

/// <summary>
///     树节点接口
/// </summary>
public interface ITreeNodeInfo : INodeInfo, ITreeNodeInfo<Guid>
{
}

#endregion

/// <summary>
///     节点
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class NodeInfo<TKey> : INodeInfo<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of INodeInfo<TKey>

    /// <summary>
    ///     标识
    /// </summary>
    public required TKey Id { get; set; }

    /// <summary>
    ///     名称
    /// </summary>
    public required string Name { get; set; }

    #endregion
}

/// <summary>
///     节点
/// </summary>
public abstract class NodeInfo : NodeInfo<Guid>, INodeInfo
{
}

/// <summary>
///     树节点
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class TreeNodeInfo<TKey> : ITreeNodeInfo<TKey> where TKey : IEquatable<TKey>
{
    #region Implementation of ITreeNodeInfo<TKey>

    /// <summary>
    ///     父节点
    /// </summary>
    public required TKey ParentId { get; set; } = default!;

    #endregion

    #region Implementation of INodeInfo<TKey>

    /// <summary>
    ///     标识
    /// </summary>
    public required TKey Id { get; set; }

    /// <summary>
    ///     名称
    /// </summary>
    public required string Name { get; set; }

    #endregion
}

/// <summary>
///     树节点
/// </summary>
public abstract class TreeNodeInfo : TreeNodeInfo<Guid>, ITreeNodeInfo
{
}

/// <summary>
///     树
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class Tree<TNode, TKey> : INodeInfo<TKey> where TNode : INodeInfo<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     子节点
    /// </summary>
    public ICollection<TNode> Children { get; set; } = default!;

    #region Implementation of INodeInfo<TKey>

    /// <summary>
    ///     标识
    /// </summary>
    public required TKey Id { get; set; } = default!;

    /// <summary>
    ///     名称
    /// </summary>
    public string Name { get; set; } = default!;

    #endregion
}

/// <summary>
///     树
/// </summary>
/// <typeparam name="TNode">子节点模板</typeparam>
public abstract class Tree<TNode> : Tree<TNode, Guid>, INodeInfo where TNode : INodeInfo
{
}