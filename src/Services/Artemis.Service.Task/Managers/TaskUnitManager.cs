using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Shared.Task.Transfer;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Managers;

/// <summary>
/// 任务单元管理器接口
/// </summary>
public interface ITaskUnitManager : IRequiredOneToManyManager<
    ArtemisTask, TaskInfo, TaskPackage,
    ArtemisTaskUnit, TaskUnitInfo, TaskUnitPackage>
{
    /// <summary>
    /// 查询任务单元信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="unitNameSearch"></param>
    /// <param name="unitCodeSearch"></param>
    /// <param name="designCodeSearch"></param>
    /// <param name="taskUnitState"></param>
    /// <param name="taskUnitMode"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<TaskUnitInfo>> FetchTaskUnitsAsync(
        Guid id,
        string? unitNameSearch,
        string? unitCodeSearch,
        string? designCodeSearch,
        string? taskUnitState,
        string? taskUnitMode,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 任务单元管理器实现
/// </summary>
public class TaskUnitManager : RequiredOneToManyManager<
    ArtemisTask, TaskInfo, TaskPackage,
    ArtemisTaskUnit, TaskUnitInfo, TaskUnitPackage>,
    ITaskUnitManager
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    public TaskUnitManager(
        IArtemisTaskStore taskStore,
        IArtemisTaskUnitStore taskUnitStore) : base(taskStore, taskUnitStore)
    {
    }

    #region Overrides

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected override Expression<Func<ArtemisTaskUnit, bool>> SubEntityKeyMatcher(Guid key)
    {
        return item => item.TaskId == key;
    }

    /// <summary>
    ///     设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected override void SetSubEntityRelationalKey(ArtemisTaskUnit subEntity, Guid key)
    {
        subEntity.TaskId = key;
    }

    #endregion

    #region Implementation of ITaskUnitManager

    /// <summary>
    /// 查询任务单元信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="unitNameSearch"></param>
    /// <param name="unitCodeSearch"></param>
    /// <param name="designCodeSearch"></param>
    /// <param name="taskUnitState"></param>
    /// <param name="taskUnitMode"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PageResult<TaskUnitInfo>> FetchTaskUnitsAsync(
        Guid id, 
        string? unitNameSearch, 
        string? unitCodeSearch, 
        string? designCodeSearch,
        string? taskUnitState,
        string? taskUnitMode,
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await EntityStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            unitNameSearch ??= string.Empty;
            unitCodeSearch ??= string.Empty;
            designCodeSearch ??= string.Empty;
            taskUnitState ??= string.Empty;
            taskUnitMode ??= string.Empty;

            var query = SubEntityStore.EntityQuery
                .Where(item => item.TaskId == id);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                unitNameSearch != string.Empty,
                unit => EF.Functions.Like(
                    unit.UnitName, $"%{unitNameSearch}%"));

            query = query.WhereIf(
                unitCodeSearch != string.Empty,
                unit => EF.Functions.Like(
                    unit.UnitCode, $"%{unitCodeSearch}%"));

            query = query.WhereIf(
                designCodeSearch != string.Empty,
                unit => EF.Functions.Like(
                    unit.DesignCode, $"%{designCodeSearch}%"));

            query = query.WhereIf(taskUnitState != string.Empty, unit => unit.TaskUnitState == taskUnitState);

            query = query.WhereIf(taskUnitMode != string.Empty, unit => unit.TaskUnitMode == taskUnitMode);

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(division => division.CreateBy);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var units = await query
                .ProjectToType<TaskUnitInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<TaskUnitInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = units
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisTask), id.GuidToString());

    }

    #endregion
}