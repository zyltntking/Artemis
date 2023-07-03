using System.ComponentModel;
using Artemis.Data.Core.Fundamental;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     主机类型
/// </summary>
[Description("主机类型")]
public class HostType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static HostType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     网关
    /// </summary>
    public static HostType Gateway = new(0, nameof(Gateway));

    /// <summary>
    ///     缓存
    /// </summary>
    public static HostType Cache = new(1, nameof(Cache));

    /// <summary>
    ///     队列
    /// </summary>
    public static HostType Queue = new(2, nameof(Queue));

    /// <summary>
    ///     服务
    /// </summary>
    public static HostType Service = new(3, nameof(Service));

    /// <summary>
    ///     日志
    /// </summary>
    public static HostType Log = new(4, nameof(Log));

    /// <summary>
    ///     文件
    /// </summary>
    public static HostType File = new(5, nameof(File));

    /// <summary>
    ///     接口
    /// </summary>
    public static HostType Interface = new(6, nameof(Interface));

    /// <summary>
    ///     数据库
    /// </summary>
    public static HostType Database = new(7, nameof(Database));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private HostType(int id, string name) : base(id, name)
    {
    }
}