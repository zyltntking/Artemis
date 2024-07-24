using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Core.Fundamental.Types;
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
    /// <param name="taskState">任务状态搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskShip,
        string? taskMode,
        string? taskState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        taskNameSearch ??= string.Empty;

        taskShip ??= string.Empty;

        taskMode ??= string.Empty;

        taskState ??= string.Empty;

        var query = TaskStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedTaskName = taskNameSearch.StringNormalize();

        query = query.WhereIf(
            taskNameSearch != string.Empty,
            task => EF.Functions.Like(task.NormalizedTaskName, $"%{normalizedTaskName}%"));

        query = query.WhereIf(taskShip != string.Empty, task => task.TaskShip == taskShip);

        query = query.WhereIf(taskMode != string.Empty, task => task.TaskMode == taskMode);

        query = query.WhereIf(taskState != string.Empty, task => task.TaskState == taskState);

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

    /// <summary>
    /// 创建任务
    /// </summary>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Data.Store.StoreResult> CreateTaskAsync(
        TaskPackage package, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedName = package.TaskName.StringNormalize();

        var exists = await TaskStore.EntityQuery
            .AnyAsync(task => task.NormalizedTaskName == normalizedName, cancellationToken);

        if (exists)
            return Data.Store.StoreResult.EntityFoundFailed(nameof(Context.ArtemisTask), package.TaskName);

        var task = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

        task.NormalizedTaskName = normalizedName;
        task.TaskMode = TaskMode.Normal.Name;
        task.TaskState = TaskState.Created.Name;
        task.TaskShip = TaskShip.Normal.Name;

        return await TaskStore.CreateAsync(task, cancellationToken);
    }

    /// <summary>
    /// 创建任务
    /// </summary>
    /// <param name="packages">任务信息</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Data.Store.StoreResult> CreateTasksAsync(IEnumerable<TaskPackage> packages, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var tasks = packages.Select(package =>
        {
            var task = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

            task.NormalizedTaskName = package.TaskName.StringNormalize();
            task.TaskMode = TaskMode.Normal.Name;
            task.TaskState = TaskState.Created.Name;
            task.TaskShip = TaskShip.Normal.Name;

            return task;
        });

        return TaskStore.CreateAsync(tasks, cancellationToken);
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="id">任务表示</param>
    /// <param name="package">任务信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<Data.Store.StoreResult> UpdateTaskAsync(
        Guid id, 
        TaskPackage package, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
        {
            package.Adapt(task);
            task.NormalizedTaskName = package.TaskName.StringNormalize();

            return await TaskStore.UpdateAsync(task, cancellationToken);
        }

        return Data.Store.StoreResult.EntityNotFoundFailed(nameof(Context.ArtemisTask), id.IdToString()!);
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Data.Store.StoreResult> UpdateTasksAsync(
        IDictionary<Guid, TaskPackage> dictionary, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var ids = dictionary.Keys;

        var tasks = await TaskStore.FindEntitiesAsync(ids, cancellationToken);

        var taskList = tasks.ToList();

        if (taskList.Any())
        {
            tasks = taskList.Select(task =>
            {
                var package = dictionary[task.Id];

                package.Adapt(task);
                task.NormalizedTaskName = package.TaskName.StringNormalize();

                return task;
            });

            return await TaskStore.UpdateAsync(tasks, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(id => id.IdToString()));

        return Data.Store.StoreResult.EntityNotFoundFailed(nameof(Context.ArtemisTask), flag);
    }

    #endregion
}