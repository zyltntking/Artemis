using Artemis.Data.Core;

namespace Artemis.Service.Shared.Task.Transfer;

/// <summary>
///     任务信息树
/// </summary>
public record TaskInfoTree : TaskInfo, ITreeBase<TaskInfoTree>
{
    /// <summary>
    ///     子任务
    /// </summary>
    public ICollection<TaskInfoTree>? Children { get; set; }
}

/// <summary>
///     任务信息
/// </summary>
public record TaskInfo : TaskPackage, ITaskInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     父标识
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    ///     任务归属
    /// </summary>
    public required string TaskShip { get; set; }

    /// <summary>
    ///     任务模式
    /// </summary>
    public required string TaskMode { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    public required string TaskState { get; set; }
}

/// <summary>
///     任务数据包
/// </summary>
public record TaskPackage : ITaskPackage
{
    /// <summary>
    ///     任务名称
    /// </summary>
    public required string TaskName { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}