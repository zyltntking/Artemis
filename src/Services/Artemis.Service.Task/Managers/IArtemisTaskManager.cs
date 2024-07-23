using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Shared.Task.Transfer;

namespace Artemis.Service.Task.Managers;

/// <summary>
///     任务管理接口
/// </summary>
public interface IArtemisTaskManager : IManager
{
    /// <summary>
    ///     根据任务信息搜索任务
    /// </summary>
    /// <param name="taskNameSearch">任务名搜索值</param>
    /// <param name="taskShip">任务归属搜索值</param>
    /// <param name="taskMode">任务模式搜索值</param>
    /// <param name="taskStatus">任务状态搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskShip,
        string? taskMode,
        string? taskStatus,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据任务凭据获取任务信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TaskInfoTree> GetTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}