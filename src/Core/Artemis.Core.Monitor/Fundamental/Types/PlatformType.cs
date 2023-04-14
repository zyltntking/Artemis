using Artemis.Data.Core.Fundamental;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     平台类型
/// </summary>
public class PlatformType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static PlatformType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     Linux
    /// </summary>
    public static PlatformType Linux = new(0, nameof(Linux));

    /// <summary>
    ///     Windows
    /// </summary>
    public static PlatformType Windows = new(1, nameof(Windows));

    /// <summary>
    ///     MacOS
    /// </summary>
    public static PlatformType MacOS = new(2, nameof(MacOS));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private PlatformType(int id, string name) : base(id, name)
    {
    }
}