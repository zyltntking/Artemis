namespace Artemis.Data.Shared.Task;

/// <summary>
///     任务代理接口
/// </summary>
public interface ITaskAgent
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
    ///     代理Id
    /// </summary>
    Guid AgentId { get; set; }
}