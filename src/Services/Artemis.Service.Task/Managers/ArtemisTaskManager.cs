using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Task.Managers;

/// <summary>
///     任务管理器
/// </summary>
public sealed class ArtemisTaskManager : Manager, IArtemisTaskManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="taskStore">存储访问器依赖</param>
    /// <param name="taskAgentStores"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="agentStore"></param>
    /// <param name="taskUnitStore"></param>
    /// <param name="taskTargetStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisTaskManager(
        IArtemisTaskStore taskStore,
        IArtemisAgentStore agentStore,
        IArtemisTaskUnitStore taskUnitStore,
        IArtemisTaskTargetStore taskTargetStore,
        IArtemisTaskAgentStores taskAgentStores,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        TaskStore = taskStore ?? throw new ArgumentNullException(nameof(taskStore));
        AgentStore = agentStore ?? throw new ArgumentNullException(nameof(agentStore));
        TaskUnitStore = taskUnitStore ?? throw new ArgumentNullException(nameof(taskUnitStore));
        TaskTargetStore = taskTargetStore ?? throw new ArgumentNullException(nameof(taskTargetStore));
        TaskAgentStores = taskAgentStores ?? throw new ArgumentNullException(nameof(taskAgentStores));
    }

    #region Overrides of KeyLessManager<ArtemisTask,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        TaskStore.Dispose();
        AgentStore.Dispose();
        TaskUnitStore.Dispose();
        TaskTargetStore.Dispose();
        TaskAgentStores.Dispose();
    }

    #endregion

    #region StoreAccess

    private IArtemisTaskStore TaskStore { get; }

    private IArtemisAgentStore AgentStore { get; }

    private IArtemisTaskUnitStore TaskUnitStore { get; }

    private IArtemisTaskTargetStore TaskTargetStore { get; }

    private IArtemisTaskAgentStores TaskAgentStores { get; }

    #endregion

    #region Implementation of IArtemisTaskManager

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
    public async Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskShip,
        string? taskMode,
        string? taskStatus,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        taskNameSearch ??= string.Empty;

        taskShip ??= string.Empty;

        taskMode ??= string.Empty;

        taskStatus ??= string.Empty;

        var query = TaskStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedTaskName = taskNameSearch.StringNormalize();

        query = query.WhereIf(
            taskNameSearch != string.Empty,
            task => EF.Functions.Like(task.NormalizedTaskName, $"%{normalizedTaskName}%"));

        query = query.WhereIf(taskShip != string.Empty, task => task.TaskShip == taskShip);

        query = query.WhereIf(taskMode != string.Empty, task => task.TaskMode == taskMode);

        query = query.WhereIf(taskStatus != string.Empty, task => task.TaskStatus == taskStatus);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(task => task.NormalizedTaskName);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var tasks = await query
            .ProjectToType<TaskInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<TaskInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = tasks
        };
    }

    /// <summary>
    ///     根据任务凭据获取任务信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TaskInfoTree> GetTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore
            .EntityQuery
            .Include(task => task.Children)
            .Where(task => task.Id == id)
            .ProjectToType<TaskInfoTree>()
            .SingleOrDefaultAsync(cancellationToken);

        return task ?? throw new EntityNotFoundException(nameof(ArtemisTask), id.IdToString()!);
    }

    #endregion
}