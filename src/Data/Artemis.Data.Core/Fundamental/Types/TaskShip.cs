using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     任务归属
/// </summary>
[Description("任务归属")]
public sealed class TaskShip : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private TaskShip(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     根任务
    /// </summary>
    [Description("根任务")]
    public static TaskShip Root { get; } = new(0, nameof(Root));

    /// <summary>
    ///     常规任务
    /// </summary>
    [Description("常规任务")]
    public static TaskShip Normal { get; } = new(1, nameof(Normal));

    /// <summary>
    ///     子任务
    /// </summary>
    [Description("子任务")]
    public static TaskShip Child { get; } = new(2, nameof(Child));
}