namespace Artemis.Service.Shared.Task;

/// <summary>
///     任务单元代理接口
/// </summary>
public interface ITaskUnitAgent
{
    /// <summary>
    ///     任务单元Id
    /// </summary>
    Guid TaskUnitId { get; set; }

    /// <summary>
    ///     代理Id
    /// </summary>
    Guid AgentId { get; set; }
}