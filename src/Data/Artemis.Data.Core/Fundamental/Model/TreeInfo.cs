namespace Artemis.Data.Core.Fundamental.Model;

#region Interface

/// <summary>
///     节点信息接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface INodeInfo<TKey>
    where TKey : IEquatable<TKey>
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
///     节点信息接口
/// </summary>
public interface INodeInfo : INodeInfo<Guid>
{
}

/// <summary>
///     树节点信息接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface ITreeNodeInfo<TKey> : INodeInfo<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     父节点
    /// </summary>
    TKey ParentId { get; set; }
}

/// <summary>
///     树节点信息接口
/// </summary>
public interface ITreeNodeInfo : INodeInfo, ITreeNodeInfo<Guid>
{
}

#endregion

#region NodeInfo

/// <summary>
///     节点信息
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class NodeInfo<TKey> : INodeInfo<TKey>
    where TKey : IEquatable<TKey>
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
///     节点信息
/// </summary>
public abstract class NodeInfo : NodeInfo<Guid>, INodeInfo
{
}

#endregion

#region TreeNodeInfo

/// <summary>
///     树节点信息
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class TreeNodeInfo<TKey> : NodeInfo<TKey>, ITreeNodeInfo<TKey>
    where TKey : IEquatable<TKey>
{
    #region Implementation of ITreeNodeInfo<TKey>

    /// <summary>
    ///     父节点
    /// </summary>
    public required TKey ParentId { get; set; } = default!;

    #endregion
}

/// <summary>
///     树节点信息
/// </summary>
public abstract class TreeNodeInfo : TreeNodeInfo<Guid>, ITreeNodeInfo
{
}

#endregion

/// <summary>
///     树节点
/// </summary>
/// <typeparam name="TKey">键类型</typeparam>
/// <typeparam name="TNode">节点类型</typeparam>
public abstract class TreeNode<TNode, TKey> : TreeNodeInfo<TKey>
    where TNode : TreeNode<TNode, TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     子节点列表
    /// </summary>
    public ICollection<TNode>? Children { get; set; }
}

/// <summary>
///     树节点
/// </summary>
public abstract class TreeNode<TNode> : TreeNode<TNode, Guid>
    where TNode : TreeNode<TNode>
{
}

/// <summary>
///     树
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class Tree<TNode, TKey> : TreeNode<TNode, TKey>
    where TNode : TreeNode<TNode, TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     构造树
    /// </summary>
    /// <param name="rootId"></param>
    /// <param name="nodeList"></param>
    public TNode? BuildTree(TKey rootId, IEnumerable<TNode> nodeList)
    {
        var treeNodes = nodeList.ToList();

        var root = treeNodes.FirstOrDefault(x => x.Id.Equals(rootId));

        if (root != null)
        {
            var children = treeNodes.Where(x => x.ParentId.Equals(rootId)).ToList();

            if (children.Count > 0)
            {
                root.Children = new List<TNode>();

                foreach (var child in children)
                {
                    var childNode = BuildTree(child.Id, treeNodes);

                    if (childNode != null) root.Children.Add(childNode);
                }

                if (root.Children.Count == 0) root.Children = null;
            }
        }

        return root;
    }
}

/// <summary>
///     树
/// </summary>
/// <typeparam name="TNode">子节点模板</typeparam>
public abstract class Tree<TNode> : Tree<TNode, Guid>
    where TNode : TreeNode<TNode, Guid>
{
}