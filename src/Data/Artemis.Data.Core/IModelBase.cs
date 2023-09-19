namespace Artemis.Data.Core;

/// <summary>
///     基本分区模型
/// </summary>
public interface IPartitionBase : IModelBase, IPartitionBase<Guid>
{
}

/// <summary>
///     基本分区模型
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IPartitionBase<TKey> : IModelBase<TKey>, IPartitionSlot where TKey : IEquatable<TKey>
{
}

/// <summary>
///     基本模型接口
/// </summary>
public interface IModelBase : IKeySlot, IModelBase<Guid>
{
}

/// <summary>
///     基本模型接口
/// </summary>
/// <typeparam name="TKey">基本记录标识</typeparam>
public interface IModelBase<TKey> : IKeySlot<TKey>, IMateSlot where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="prefix">前缀</param>
    string GenerateKey(string? prefix)
    {
        return prefix == null ? Id.ToString()! : $"{prefix}:{Id}";
    }
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

/// <summary>
///     标识组件接口
/// </summary>
public interface IKeySlot : IKeySlot<Guid>
{
}

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