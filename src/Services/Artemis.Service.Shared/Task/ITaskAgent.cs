namespace Artemis.Service.Shared.Task;

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
    ///     代理Id
    /// </summary>
    Guid AgentId { get; set; }
}