using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     任务状态
/// </summary>
[Description("任务状态")]
public sealed class TaskState : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private TaskState(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     初始化
    /// </summary>
    [Description("初始化")]
    public static TaskState Initial { get; } = new(0, nameof(Initial));

    /// <summary>
    ///     已创建
    /// </summary>
    [Description("已创建")]
    public static TaskState Created { get; } = new(1, nameof(Created));

    /// <summary>
    ///     待执行
    /// </summary>
    [Description("待执行")]
    public static TaskState Waiting { get; } = new(2, nameof(Waiting));

    /// <summary>
    ///     执行中
    /// </summary>
    [Description("执行中")]
    public static TaskState Running { get; } = new(3, nameof(Running));

    /// <summary>
    ///     已完成
    /// </summary>
    [Description("已完成")]
    public static TaskState Completed { get; } = new(4, nameof(Completed));

    /// <summary>
    ///     已取消
    /// </summary>
    [Description("已取消")]
    public static TaskState Canceled { get; } = new(5, nameof(Canceled));
}