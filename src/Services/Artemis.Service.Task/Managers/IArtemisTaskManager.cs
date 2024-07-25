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
    /// <param name="taskState">任务状态搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="taskCodeSearch"></param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskCodeSearch,
        string? taskShip,
        string? taskMode,
        string? taskState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        return FetchTasksAsync(taskNameSearch, taskCodeSearch, null, taskShip, taskMode, taskState, page, size,
            cancellationToken);
    }

    /// <summary>
    ///     根据任务信息搜索任务
    /// </summary>
    /// <param name="taskNameSearch">任务名搜索值</param>
    /// <param name="designCodeSearch"></param>
    /// <param name="taskShip">任务归属搜索值</param>
    /// <param name="taskMode">任务模式搜索值</param>
    /// <param name="taskState">任务状态搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="taskCodeSearch"></param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskCodeSearch,
        string? designCodeSearch,
        string? taskShip,
        string? taskMode,
        string? taskState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据任务凭据获取任务信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TaskInfo?> GetTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据任务凭据获取任务树信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TaskInfoTree> GetTaskTreeAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建任务
    /// </summary>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateTaskAsync(
        TaskPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建任务
    /// </summary>
    /// <param name="packages">任务信息</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateTasksAsync(
        IEnumerable<TaskPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新任务
    /// </summary>
    /// <param name="id">任务表示</param>
    /// <param name="package">任务信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<StoreResult> UpdateTaskAsync(
        Guid id,
        TaskPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新任务
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UpdateTasksAsync(
        IDictionary<Guid, TaskPackage> dictionary,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除任务
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteTasksAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateChildTaskAsync(
        Guid id,
        TaskPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量创建子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateChildTasksAsync(
        Guid id,
        IEnumerable<TaskPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="childId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> AddChildTaskAsync(
        Guid id,
        Guid childId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量添加子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="childIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> AddChildTasksAsync(
        Guid id,
        IEnumerable<Guid> childIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteChildTaskAsync(
        Guid taskId,
        Guid childTaskId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchDeleteChildTaskAsync(
        Guid taskId,
        IEnumerable<Guid> childTaskIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     移除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> RemoveChildTaskAsync(
        Guid taskId,
        Guid childTaskId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量移除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> BatchRemoveChildTaskAsync(
        Guid taskId,
        IEnumerable<Guid> childTaskIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据任务单元信息搜索任务单元
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="unitNameSearch"></param>
    /// <param name="unitCodeSearch"></param>
    /// <param name="designCodeSearch"></param>
    /// <param name="taskUnitMode"></param>
    /// <param name="taskUnitState"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<TaskUnitInfo>> FetchTaskUnitsAsync(
        Guid taskId,
        string? unitNameSearch,
        string? unitCodeSearch,
        string? designCodeSearch,
        string? taskUnitMode,
        string? taskUnitState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取任务单元信息
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="unitId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TaskUnitInfo?> GetTaskUnitAsync(
        Guid taskId,
        Guid unitId,
        CancellationToken cancellationToken = default);
}