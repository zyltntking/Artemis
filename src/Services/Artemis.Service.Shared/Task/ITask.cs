using Artemis.Data.Core;

namespace Artemis.Service.Shared.Task;

/// <summary>
///     任务接口
/// </summary>
public interface ITask : ITaskInfo
{
    /// <summary>
    ///     标准化任务名
    /// </summary>
    string NormalizedTaskName { get; set; }
}

/// <summary>
///     任务信息接口
/// </summary>
public interface ITaskInfo : ITaskPackage, IKeySlot, IParentKeySlot
{
    /// <summary>
    ///     任务归属
    /// </summary>
    string TaskShip { get; set; }

    /// <summary>
    ///     任务模式
    /// </summary>
    string TaskMode { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    string TaskState { get; set; }
}

/// <summary>
///     任务数据包接口
/// </summary>
public interface ITaskPackage
{
    /// <summary>
    ///     任务名称
    /// </summary>
    string TaskName { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    DateTime? EndTime { get; set; }
}