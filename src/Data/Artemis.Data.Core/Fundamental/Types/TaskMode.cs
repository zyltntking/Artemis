using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     任务模式
/// </summary>
[Description("任务模式")]
public sealed class TaskMode : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private TaskMode(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     常规任务
    /// </summary>
    [Description("常规任务")]
    public static TaskMode Normal { get; } = new(0, nameof(Normal));

    /// <summary>
    ///     同步任务
    /// </summary>
    [Description("同步任务")]
    public static TaskMode Synchronous { get; } = new(1, nameof(Synchronous));

    /// <summary>
    ///     异步任务
    /// </summary>
    [Description("异步任务")]
    public static TaskMode Asynchronous { get; } = new(2, nameof(Asynchronous));

    /// <summary>
    ///     并行任务
    /// </summary>
    [Description("并行任务")]
    public static TaskMode Parallel { get; } = new(3, nameof(Parallel));

    /// <summary>
    ///     顺序任务
    /// </summary>
    [Description("顺序任务")]
    public static TaskMode Sequential { get; } = new(4, nameof(Sequential));

    /// <summary>
    ///     递归任务
    /// </summary>
    [Description("递归任务")]
    public static TaskMode Recursive { get; } = new(5, nameof(Recursive));

    /// <summary>
    ///     条件任务
    /// </summary>
    [Description("条件任务")]
    public static TaskMode Conditional { get; } = new(6, nameof(Conditional));
}