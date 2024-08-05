using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Managers;

/// <summary>
///     任务树管理器
/// </summary>
public interface ITaskTreeManager : ISelfReferenceTreeManager<ArtemisTask, TaskInfo, TaskInfoTree, TaskPackage>
{
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
}

/// <summary>
///     任务树管理器实现
/// </summary>
public class TaskTreeManager : SelfReferenceTreeManager<ArtemisTask, TaskInfo, TaskInfoTree, TaskPackage>,
    ITaskTreeManager
{
    /// <summary>
    ///     独立模型管理器构造
    /// </summary>
    /// <param name="taskStore"></param>
    public TaskTreeManager(IArtemisTaskStore taskStore) : base(taskStore)
    {
    }

    #region Implementation of ITaskTreeManager

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

        var query = EntityStore.EntityQuery;

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

    #endregion

    #region Overrides

    /// <summary>
    ///     映射到新实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<ArtemisTask> MapNewEntity(
        TaskPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var task = Instance.CreateInstance<ArtemisTask, TaskPackage>(package);
        task.ParentId = null;
        task.NormalizedTaskName = package.TaskName.StringNormalize();
        task.TaskMode = TaskMode.Normal;
        task.TaskState = TaskState.Created;
        task.TaskShip = TaskShip.Normal;

        return System.Threading.Tasks.Task.FromResult(task);
    }

    /// <summary>
    ///     覆盖实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<ArtemisTask> MapOverEntity(ArtemisTask entity, TaskPackage package, int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var task = package.Adapt(entity);

        task.NormalizedTaskName = package.TaskName.StringNormalize();

        return System.Threading.Tasks.Task.FromResult(task);
    }


    /// <summary>
    ///     获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<List<TaskInfo>> FetchNonRootTreeNodeList(Guid key,
        CancellationToken cancellationToken)
    {
        var taskCode = await EntityStore
            .KeyMatchQuery(key)
            .Select(task => task.TaskCode)
            .FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(taskCode))
        {
            var list = await EntityStore.EntityQuery
                .Where(task => task.TaskCode != null && task.TaskCode.StartsWith(taskCode))
                .ProjectToType<TaskInfo>()
                .ToListAsync(cancellationToken);

            return list;
        }

        return [];
    }

    /// <summary>
    ///     在添加子节点之前
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override System.Threading.Tasks.Task BeforeAddChildNode(
        ArtemisTask parent, 
        ArtemisTask child, 
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        child.TaskShip = TaskShip.Child;

        return System.Threading.Tasks.Task.CompletedTask;
    }

    /// <summary>
    ///     在添加子节点之后
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="child"></param>
    protected override System.Threading.Tasks.Task AfterAddChildNode(
        ArtemisTask parent,
        ArtemisTask? child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        if (parent.ParentId == null && parent.TaskShip != TaskShip.Root)
        {
            parent.TaskShip = TaskShip.Root;

            return EntityStore.UpdateAsync(parent, cancellationToken);
        }

        return System.Threading.Tasks.Task.CompletedTask;
    }

    /// <summary>
    ///     在移除子节点之前
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override async System.Threading.Tasks.Task BeforeRemoveChildNode(
        ArtemisTask parent,
        ArtemisTask child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));
        var exists = await EntityStore.EntityQuery.AnyAsync(task => task.ParentId == child.Id, cancellationToken);

        child.TaskShip = exists ? TaskShip.Root : TaskShip.Normal;
    }

    /// <summary>
    ///     在移除子节点之后
    /// </summary>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    protected override async System.Threading.Tasks.Task AfterRemoveChildNode(
        ArtemisTask parent,
        ArtemisTask? child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        if (parent.TaskShip != TaskShip.Child)
        {
            var exists = await EntityStore.EntityQuery.AnyAsync(task => task.ParentId == parent.Id, cancellationToken);

            if (!exists)
            {
                parent.TaskShip = TaskShip.Normal;
            }

            await EntityStore.UpdateAsync(parent, cancellationToken);
        }
    }

    #endregion
}