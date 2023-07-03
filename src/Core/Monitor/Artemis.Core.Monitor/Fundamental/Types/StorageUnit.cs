using Artemis.Data.Core.Fundamental;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     存储单位
/// </summary>
public class StorageUnit : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static StorageUnit Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     B
    /// </summary>
    public static StorageUnit B = new(0, nameof(B));

    /// <summary>
    ///     KB
    /// </summary>
    public static StorageUnit KB = new(1, nameof(KB));

    /// <summary>
    ///     MB
    /// </summary>
    public static StorageUnit MB = new(2, nameof(MB));

    /// <summary>
    ///     GB
    /// </summary>
    public static StorageUnit GB = new(3, nameof(GB));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private StorageUnit(int id, string name) : base(id, name)
    {
    }
}