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
public interface INodeInfo : INodeInfo<Guid>;

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
public interface ITreeNodeInfo : INodeInfo, ITreeNodeInfo<Guid>;

/// <summary>
///     树节点接口
/// </summary>
/// <typeparam name="TNode">节点类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface ITreeNode<TNode, TKey> : INodeInfo<TKey>
    where TNode : ITreeNode<TNode, TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     子节点列表
    /// </summary>
    ICollection<TNode>? Children { get; set; }
}

/// <summary>
///     树节点接口
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface ITreeNode<TNode> : ITreeNode<TNode, Guid>
    where TNode : ITreeNode<TNode>;

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
public abstract class NodeInfo : NodeInfo<Guid>, INodeInfo;

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
public abstract class TreeNodeInfo : TreeNodeInfo<Guid>, ITreeNodeInfo;

#endregion

#region TreeNode

/// <summary>
///     树节点
/// </summary>
/// <typeparam name="TKey">键类型</typeparam>
/// <typeparam name="TTreeNode">节点类型</typeparam>
public abstract class TreeNode<TTreeNode, TKey> : NodeInfo<TKey>, ITreeNode<TTreeNode, TKey>
    where TTreeNode : TreeNode<TTreeNode, TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     子节点列表
    /// </summary>
    public ICollection<TTreeNode>? Children { get; set; }
}

/// <summary>
///     树节点
/// </summary>
public abstract class TreeNode<TTreeNode> : TreeNode<TTreeNode, Guid>, ITreeNode<TTreeNode>
    where TTreeNode : TreeNode<TTreeNode>;

#endregion

/// <summary>
///     树
/// </summary>
/// <typeparam name="TTreeNode">节点模板</typeparam>
/// <typeparam name="TTreeNodeInfo">节点信息模板</typeparam>
file abstract class Tree<TTreeNode, TTreeNodeInfo> : Tree<TTreeNode, TTreeNodeInfo, Guid>
    where TTreeNode : TreeNode<TTreeNode, Guid>
    where TTreeNodeInfo : ITreeNodeInfo<Guid>
{
    /// <summary>
    ///     构造树
    /// </summary>
    /// <param name="nodeList">节点信息列表</param>
    /// <param name="rootId">根id</param>
    protected Tree(IEnumerable<TTreeNodeInfo> nodeList, Guid rootId) : base(nodeList, rootId)
    {
    }
}

/// <summary>
///     树
/// </summary>
/// <typeparam name="TTreeNode">节点模板</typeparam>
/// <typeparam name="TTreeNodeInfo">节点信息模板</typeparam>
/// <typeparam name="TKey">键</typeparam>
file abstract class Tree<TTreeNode, TTreeNodeInfo, TKey> : TreeNode<TTreeNode, TKey>
    where TTreeNode : TreeNode<TTreeNode, TKey>
    where TTreeNodeInfo : ITreeNodeInfo<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     构造树
    /// </summary>
    /// <param name="nodeList">节点列表</param>
    /// <param name="rootId">根节点id</param>
    protected Tree(IEnumerable<TTreeNodeInfo> nodeList, TKey rootId)
    {
        var rootNode = Generate(rootId, nodeList);

        if (rootNode != null)
        {
            Id = rootNode.Id;
            Name = rootNode.Name;
            Children = rootNode.Children;
        }
    }

    /// <summary>
    ///     树节点选择器
    /// </summary>
    protected abstract Func<TTreeNodeInfo, TTreeNode> TreeNodeSelector { get; }

    /// <summary>
    ///     生成
    /// </summary>
    /// <param name="rootId">根节点id</param>
    /// <param name="treeNodeInfoList">节点列表</param>
    /// <returns></returns>
    protected TTreeNode? Generate(TKey rootId, IEnumerable<TTreeNodeInfo> treeNodeInfoList)
    {
        return BuildTree(rootId, treeNodeInfoList);
    }


    /// <summary>
    ///     构造树
    /// </summary>
    /// <param name="rootId">根结点标识</param>
    /// <param name="treeNodeInfos">节点信息列表</param>
    /// <returns></returns>
    protected virtual TTreeNode? BuildTree(TKey rootId, IEnumerable<TTreeNodeInfo> treeNodeInfos)
    {
        var treeNodeInfoList = treeNodeInfos.ToList();

        var rootNode = treeNodeInfoList.Where(info => info.Id.Equals(rootId)).Select(TreeNodeSelector).FirstOrDefault();

        if (rootNode != null)
        {
            var children = treeNodeInfoList
                .Where(info => info.ParentId.Equals(rootId))
                .Select(TreeNodeSelector)
                .ToList();

            if (children.Count > 0)
            {
                rootNode.Children = new List<TTreeNode>();

                foreach (var child in children)
                {
                    var childNode = BuildTree(child.Id, treeNodeInfoList);

                    if (childNode != null)
                        rootNode.Children.Add(childNode);
                }

                if (rootNode.Children.Count == 0)
                    rootNode.Children = null;
            }
        }

        return rootNode;
    }
}