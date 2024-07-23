namespace Artemis.Data.Core;

/// <summary>
///     树基础接口
/// </summary>
public interface ITreeBase<TTreeNode> : ITreeBase<TTreeNode, Guid> where TTreeNode : ITreeBase<TTreeNode, Guid>;

/// <summary>
///     树基础接口
/// </summary>
public interface ITreeBase<TTreeNode, TKey> where TTreeNode : ITreeBase<TTreeNode, TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     节点标识
    /// </summary>
    TKey Id { get; set; }

    /// <summary>
    ///     父节点标识
    /// </summary>
    TKey ParentId { get; set; }

    /// <summary>
    ///     子节点
    /// </summary>
    ICollection<TTreeNode>? Children { get; set; }
}