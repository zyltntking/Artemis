namespace Artemis.Data.Core;

/// <summary>
///     基本分区模型
/// </summary>
public interface IConcurrencyPartitionBase : IPartitionBase, IConcurrencyPartitionBase<Guid>, IConcurrencyModelBase
{
}

/// <summary>
///     基本并发分区模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyPartitionBase<TKey> : IPartitionBase<TKey>, IConcurrencyModelBase<TKey>
    where TKey : IEquatable<TKey>
{
}

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
public interface IPartitionBase<TKey> : IModelBase<TKey>, IPartitionSlot
    where TKey : IEquatable<TKey>
{
}

/// <summary>
///     基本并发模型接口
/// </summary>
public interface IConcurrencyModelBase : IModelBase, IConcurrencyModelBase<Guid>
{
}

/// <summary>
///     基本并发模型接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IConcurrencyModelBase<TKey> : IModelBase<TKey>, IConcurrencyStamp
    where TKey : IEquatable<TKey>
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
public interface IModelBase<TKey> : IKeySlot<TKey>, IMateSlot
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     生成键
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="space">空间</param>
    /// <param name="key"></param>
    string GenerateKey(string? prefix = null, string? space = null, string? key = null)
    {
        var list = new List<string>();

        if (prefix is not null)
            list.Add(prefix); //1

        list.Add(GetType().Name); //2

        if (space is not null)
            list.Add(space); //3

        list.Add(key ?? Id.ToString()!);

        return string.Join(":", list);
    }
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

/// <summary>
///     并发锁组件接口
/// </summary>
public interface IConcurrencyStamp
{
    /// <summary>
    ///     并发锁
    /// </summary>
    string? ConcurrencyStamp { get; set; }
}

/// <summary>
///     检验戳组件接口
/// </summary>
public interface ICheckStamp
{
    /// <summary>
    ///     校验戳
    /// </summary>
    string CheckStamp { get; set; }
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