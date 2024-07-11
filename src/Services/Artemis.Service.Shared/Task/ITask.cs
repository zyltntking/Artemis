namespace Artemis.Service.Shared.Task;

/// <summary>
///     任务接口
/// </summary>
public interface ITask
{
    /// <summary>
    ///     任务名称
    /// </summary>
    string TaskName { get; set; }

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