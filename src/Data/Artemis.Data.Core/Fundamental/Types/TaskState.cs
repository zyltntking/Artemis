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
    /// 已完成创建，具备待执行条件
    /// </summary>
    [Description("已完成创建，具备待执行条件")]
    public static TaskState ReadyToWaiting { get; } = new(2, nameof(ReadyToWaiting));

    /// <summary>
    ///     待执行
    /// </summary>
    [Description("待执行")]
    public static TaskState Waiting { get; } = new(3, nameof(Waiting));

    /// <summary>
    /// 已转入待执行状态，具备执行条件
    /// </summary>
    [Description("已转入待执行状态，具备执行条件")]
    public static TaskState ReadyToRunning { get; } = new(4, nameof(ReadyToRunning));

    /// <summary>
    ///     执行中
    /// </summary>
    [Description("执行中")]
    public static TaskState Running { get; } = new(5, nameof(Running));

    /// <summary>
    ///    执行中，具备完成条件
    /// </summary>
    [Description("执行中，具备完成条件")]
    public static TaskState ReadyToCompleted { get; } = new(6, nameof(ReadyToCompleted));

    /// <summary>
    ///     已完成
    /// </summary>
    [Description("已完成")]
    public static TaskState Completed { get; } = new(7, nameof(Completed));

    /// <summary>
    /// 已停止
    /// </summary>
    [Description("已停止")]
    public static TaskState Stopped { get; } = new(8, nameof(Stopped));

    /// <summary>
    ///     已取消
    /// </summary>
    [Description("已取消")]
    public static TaskState Canceled { get; } = new(9, nameof(Canceled));

    /// <summary>
    ///     已关闭
    /// </summary>
    [Description("已关闭")]
    public static TaskState Close { get; } = new(10, nameof(Close));
}