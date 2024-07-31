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
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisTaskManager(
        IArtemisTaskStore taskStore,
        IArtemisAgentStore agentStore,
        IArtemisTaskUnitStore taskUnitStore,
        IArtemisTaskAgentStores taskAgentStores,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        TaskStore = taskStore ?? throw new ArgumentNullException(nameof(taskStore));
        AgentStore = agentStore ?? throw new ArgumentNullException(nameof(agentStore));
        TaskUnitStore = taskUnitStore ?? throw new ArgumentNullException(nameof(taskUnitStore));
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
        TaskAgentStores.Dispose();
    }

    #endregion

    #region StoreAccess

    private IArtemisTaskStore TaskStore { get; }

    private IArtemisAgentStore AgentStore { get; }

    private IArtemisTaskUnitStore TaskUnitStore { get; }

    private IArtemisTaskAgentStores TaskAgentStores { get; }

    #endregion

    #region Implementation of IArtemisTaskManager

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
    public async Task<PageResult<TaskInfo>> FetchTasksAsync(
        string? taskNameSearch,
        string? taskCodeSearch,
        string? designCodeSearch,
        string? taskShip,
        string? taskMode,
        string? taskState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        taskNameSearch ??= string.Empty;

        taskCodeSearch ??= string.Empty;

        designCodeSearch ??= string.Empty;

        taskShip ??= string.Empty;

        taskMode ??= string.Empty;

        taskState ??= string.Empty;

        var query = TaskStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedTaskName = taskNameSearch.StringNormalize();

        query = query.WhereIf(
            taskNameSearch != string.Empty,
            task => EF.Functions.Like(task.NormalizedTaskName, $"%{normalizedTaskName}%"));

        query = query.WhereIf(
            taskCodeSearch != string.Empty,
            task => EF.Functions.Like(task.TaskCode, $"%{taskCodeSearch}%"));

        query = query.WhereIf(
            designCodeSearch != string.Empty,
            task => EF.Functions.Like(task.DesignCode, $"%{designCodeSearch}%"));

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
    public Task<TaskInfo?> GetTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return TaskStore.FindMapEntityAsync<TaskInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     根据任务凭据获取任务信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TaskInfoTree> GetTaskTreeAsync(Guid id, CancellationToken cancellationToken = default)
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
    ///     创建任务
    /// </summary>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateTaskAsync(
        TaskPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

        task.NormalizedTaskName = package.TaskName.StringNormalize();
        task.TaskMode = TaskMode.Normal;
        task.TaskState = TaskState.Created;
        task.TaskShip = TaskShip.Normal;

        return await TaskStore.CreateAsync(task, cancellationToken);
    }

    /// <summary>
    ///     创建任务
    /// </summary>
    /// <param name="packages">任务信息</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> CreateTasksAsync(IEnumerable<TaskPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var tasks = packages.Select(package =>
        {
            var task = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

            task.NormalizedTaskName = package.TaskName.StringNormalize();
            task.TaskMode = TaskMode.Normal;
            task.TaskState = TaskState.Created;
            task.TaskShip = TaskShip.Normal;

            return task;
        });

        return TaskStore.CreateAsync(tasks, cancellationToken);
    }

    /// <summary>
    ///     更新任务
    /// </summary>
    /// <param name="id">任务表示</param>
    /// <param name="package">任务信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateTaskAsync(
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

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     更新任务
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateTasksAsync(
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

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), flag);
    }

    /// <summary>
    ///     删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
            return await TaskStore.DeleteAsync(task, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     删除任务
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteTasksAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var idList = ids.ToList();

        var tasks = await TaskStore.FindEntitiesAsync(idList, cancellationToken);

        var taskList = tasks.ToList();

        if (taskList.Any())
            return await TaskStore.DeleteAsync(taskList, cancellationToken);

        var flag = string.Join(',', idList.Select(id => id.IdToString()));

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), flag);
    }

    /// <summary>
    ///     创建子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="package"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateChildTaskAsync(
        Guid id,
        TaskPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
        {
            var childTask = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

            childTask.ParentId = id;

            childTask.NormalizedTaskName = package.TaskName.StringNormalize();
            childTask.TaskMode = TaskMode.Normal;
            childTask.TaskState = TaskState.Created;
            childTask.TaskShip = TaskShip.Child;

            var result = await TaskStore.CreateAsync(childTask, cancellationToken);

            if (task.ParentId == Guid.Empty && task.TaskShip != TaskShip.Root)
            {
                task.TaskShip = TaskShip.Root;

                await TaskStore.UpdateAsync(task, cancellationToken);
            }

            return result;
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     批量创建子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="packages"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateChildTasksAsync(
        Guid id,
        IEnumerable<TaskPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
        {
            var tasks = packages.Select(package =>
            {
                var childTask = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);

                childTask.NormalizedTaskName = package.TaskName.StringNormalize();
                childTask.TaskMode = TaskMode.Normal;
                childTask.TaskState = TaskState.Created;
                childTask.TaskShip = TaskShip.Child;

                return childTask;
            });

            var result = await TaskStore.CreateAsync(tasks, cancellationToken);

            if (task.ParentId == Guid.Empty && task.TaskShip != TaskShip.Root)
            {
                task.TaskShip = TaskShip.Root;

                await TaskStore.UpdateAsync(task, cancellationToken);
            }

            return result;
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     添加子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="childId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> AddChildTaskAsync(Guid id, Guid childId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
        {
            var childTask = await TaskStore.FindEntityAsync(childId, cancellationToken);

            if (childTask is not null)
            {
                childTask.ParentId = id;

                if (childTask.TaskShip != TaskShip.Child) childTask.TaskShip = TaskShip.Child;

                var result = await TaskStore.UpdateAsync(childTask, cancellationToken);

                if (task.ParentId == Guid.Empty && task.TaskShip != TaskShip.Root)
                {
                    task.TaskShip = TaskShip.Root;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), childId.GuidToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     批量添加子任务
    /// </summary>
    /// <param name="id"></param>
    /// <param name="childIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> AddChildTasksAsync(Guid id, IEnumerable<Guid> childIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(id, cancellationToken);

        if (task is not null)
        {
            var childIdList = childIds.ToList();

            var childTasks = await TaskStore.FindEntitiesAsync(childIdList, cancellationToken);

            var childTaskList = childTasks.ToList();

            if (childTaskList.Any())
            {
                childTasks = childTaskList.Select(item =>
                {
                    item.ParentId = id;

                    if (item.TaskShip != TaskShip.Child) item.TaskShip = TaskShip.Child;

                    return item;
                });

                var result = await TaskStore.UpdateAsync(childTasks, cancellationToken);

                if (task.ParentId == Guid.Empty && task.TaskShip != TaskShip.Root)
                {
                    task.TaskShip = TaskShip.Root;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask),
                string.Join(",", childIdList.Select(item => item.GuidToString())));
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), id.GuidToString());
    }

    /// <summary>
    ///     删除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteChildTaskAsync(Guid taskId, Guid childTaskId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(taskId, cancellationToken);

        if (task is not null)
        {
            var childTask = await TaskStore.FindEntityAsync(childTaskId, cancellationToken);

            if (childTask is not null)
            {
                var result = await TaskStore.DeleteAsync(childTask, cancellationToken);

                var hasChildrenExists = await TaskStore.EntityQuery
                    .AnyAsync(item => item.ParentId == taskId, cancellationToken);

                if (!hasChildrenExists)
                {
                    task.TaskShip = TaskShip.Normal;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), childTaskId.GuidToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), taskId.GuidToString());
    }

    /// <summary>
    ///     批量删除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchDeleteChildTaskAsync(Guid taskId, IEnumerable<Guid> childTaskIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(taskId, cancellationToken);

        if (task is not null)
        {
            var childTaskIdList = childTaskIds.ToList();

            var childTasks = await TaskStore.FindEntitiesAsync(childTaskIdList, cancellationToken);

            var childTaskList = childTasks.ToList();

            if (childTaskList.Any())
            {
                var result = await TaskStore.DeleteAsync(childTaskList, cancellationToken);

                var hasChildrenExists = await TaskStore.EntityQuery
                    .AnyAsync(item => item.ParentId == taskId, cancellationToken);

                if (!hasChildrenExists)
                {
                    task.TaskShip = TaskShip.Normal;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask),
                string.Join(",", childTaskIdList.Select(item => item.GuidToString())));
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), taskId.GuidToString());
    }

    /// <summary>
    ///     移除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> RemoveChildTaskAsync(Guid taskId, Guid childTaskId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(taskId, cancellationToken);

        if (task is not null)
        {
            var childTask = await TaskStore.FindEntityAsync(childTaskId, cancellationToken);

            if (childTask is not null)
            {
                childTask.ParentId = Guid.Empty;
                childTask.TaskShip = TaskShip.Normal;

                var result = await TaskStore.UpdateAsync(childTask, cancellationToken);

                var hasChildrenExists = await TaskStore.EntityQuery
                    .AnyAsync(item => item.ParentId == taskId, cancellationToken);

                if (!hasChildrenExists)
                {
                    task.TaskShip = TaskShip.Normal;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), childTaskId.GuidToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), taskId.GuidToString());
    }

    /// <summary>
    ///     批量移除子任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="childTaskIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> BatchRemoveChildTaskAsync(Guid taskId, IEnumerable<Guid> childTaskIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var task = await TaskStore.FindEntityAsync(taskId, cancellationToken);

        if (task is not null)
        {
            var childTaskIdList = childTaskIds.ToList();

            var childTasks = await TaskStore.FindEntitiesAsync(childTaskIdList, cancellationToken);

            var childTaskList = childTasks.ToList();

            if (childTaskList.Any())
            {
                childTasks = childTaskList.Select(item =>
                {
                    item.ParentId = Guid.Empty;
                    item.TaskShip = TaskShip.Normal;
                    return item;
                });

                var result = await TaskStore.UpdateAsync(childTasks, cancellationToken);

                var hasChildrenExists = await TaskStore.EntityQuery
                    .AnyAsync(item => item.ParentId == taskId, cancellationToken);

                if (!hasChildrenExists)
                {
                    task.TaskShip = TaskShip.Normal;

                    await TaskStore.UpdateAsync(task, cancellationToken);
                }

                return result;
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask),
                string.Join(",", childTaskIdList.Select(item => item.GuidToString())));
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisTask), taskId.GuidToString());
    }

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
    public async Task<PageResult<TaskUnitInfo>> FetchTaskUnitsAsync(
        Guid taskId,
        string? unitNameSearch,
        string? unitCodeSearch,
        string? designCodeSearch,
        string? taskUnitMode,
        string? taskUnitState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = taskId != default && await TaskStore.ExistsAsync(taskId, cancellationToken);

        if (exists)
        {
            unitNameSearch ??= string.Empty;
            unitCodeSearch ??= string.Empty;
            designCodeSearch ??= string.Empty;
            taskUnitMode ??= string.Empty;
            taskUnitState ??= string.Empty;

            var query = TaskStore
                .KeyMatchQuery(taskId)
                .SelectMany(task => task.TaskUnits!);

            var total = await query.LongCountAsync(cancellationToken);

            var normalizedUnitNameSearch = unitNameSearch.StringNormalize();

            query = query.WhereIf(
                unitNameSearch != string.Empty,
                task => EF.Functions.Like(task.NormalizedUnitName, $"%{normalizedUnitNameSearch}%"));

            query = query.WhereIf(
                unitCodeSearch != string.Empty,
                task => EF.Functions.Like(task.UnitCode, $"%{unitCodeSearch}%"));

            query = query.WhereIf(
                designCodeSearch != string.Empty,
                task => EF.Functions.Like(task.DesignCode, $"%{designCodeSearch}%"));

            query = query.WhereIf(taskUnitMode != string.Empty, task => task.TaskUnitMode == taskUnitMode);

            query = query.WhereIf(taskUnitState != string.Empty, task => task.TaskUnitState == taskUnitState);

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(task => task.NormalizedUnitName);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var taskUnits = await query
                .ProjectToType<TaskUnitInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<TaskUnitInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = taskUnits
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisTask), taskId.IdToString()!);
    }

    /// <summary>
    ///     获取任务单元信息
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="unitId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TaskUnitInfo?> GetTaskUnitAsync(Guid taskId, Guid unitId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = taskId != default && await TaskStore.ExistsAsync(taskId, cancellationToken);

        if (roleExists)
            return await TaskUnitStore.FindMapEntityAsync<TaskUnitInfo>(unitId, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisTask), taskId.GuidToString());
    }

    #endregion
}