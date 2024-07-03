namespace Artemis.Data.Shared.Task;

/// <summary>
///     任务目标接口
/// </summary>
public interface ITaskTarget
{
    /// <summary>
    ///     任务Id
    /// </summary>
    Guid TaskId { get; set; }

    /// <summary>
    ///     任务单元Id
    /// </summary>
    Guid TaskUnitId { get; set; }

    /// <summary>
    ///     目标Id
    /// </summary>
    Guid TargetId { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    string TaskStatus { get; set; }

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