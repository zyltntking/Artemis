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
/// 任务目标管理器接口
/// </summary>
public interface ITaskUnitTargetManager : IRequiredOneToManyManager<
    ArtemisTaskUnit, TaskUnitInfo, TaskUnitPackage,
    ArtemisTaskUnitTarget, TaskUnitTargetInfo, TaskUnitTargetPackage>
{
    /// <summary>
    /// 查询任务目标信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="targetNameSearch"></param>
    /// <param name="targetCodeSearch"></param>
    /// <param name="designCodeSearch"></param>
    /// <param name="targetType"></param>
    /// <param name="targetState"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<TaskUnitTargetInfo>> FetchTaskUnitTargetsAsync(
        Guid id,
        string? targetNameSearch,
        string? targetCodeSearch,
        string? designCodeSearch,
        string? targetType,
        string? targetState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 任务目标管理器实现
/// </summary>
public class TaskUnitUnitTargetManager : RequiredOneToManyManager<
    ArtemisTaskUnit, TaskUnitInfo, TaskUnitPackage, 
    ArtemisTaskUnitTarget, TaskUnitTargetInfo, TaskUnitTargetPackage>,
    ITaskUnitTargetManager
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    public TaskUnitUnitTargetManager(
        IArtemisTaskUnitStore taskUnitStore,
        IArtemisTaskUnitTargetStore taskUnitTargetStore) : base(taskUnitStore, taskUnitTargetStore)
    {
    }

    #region Overrides 

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected override Expression<Func<ArtemisTaskUnitTarget, bool>> SubEntityKeyMatcher(Guid key)
    {
        return item => item.TaskUnitId == key;
    }

    /// <summary>
    ///     设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected override void SetSubEntityRelationalKey(ArtemisTaskUnitTarget subEntity, Guid key)
    {
        subEntity.TaskUnitId = key;
    }

    #endregion

    #region Implementation of ITaskUnitTargetManager

    /// <summary>
    /// 查询任务目标信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="targetNameSearch"></param>
    /// <param name="targetCodeSearch"></param>
    /// <param name="designCodeSearch"></param>
    /// <param name="targetType"></param>
    /// <param name="targetState"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PageResult<TaskUnitTargetInfo>> FetchTaskUnitTargetsAsync(
        Guid id, 
        string? targetNameSearch, 
        string? targetCodeSearch, 
        string? designCodeSearch,
        string? targetType, 
        string? targetState, 
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await EntityStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            targetNameSearch ??= string.Empty;
            targetCodeSearch ??= string.Empty;
            designCodeSearch ??= string.Empty;
            targetType ??= string.Empty;
            targetState ??= string.Empty;

            var query = SubEntityStore.EntityQuery;

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                targetNameSearch != string.Empty,
                target => EF.Functions.Like(
                    target.TargetName, $"%{targetNameSearch}%"));

            query = query.WhereIf(
                targetCodeSearch != string.Empty,
                target => EF.Functions.Like(
                    target.TargetCode, $"%{targetCodeSearch}%"));

            query = query.WhereIf(
                designCodeSearch != string.Empty,
                target => EF.Functions.Like(
                    target.DesignCode, $"%{designCodeSearch}%"));

            query = query.WhereIf(targetType != string.Empty, target => target.TargetType == targetType);

            query = query.WhereIf(targetState != string.Empty, target => target.TargetState == targetState);

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(division => division.CreateBy);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var targets = await query
                .ProjectToType<TaskUnitTargetInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<TaskUnitTargetInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = targets
            };

        }

        throw new EntityNotFoundException(nameof(ArtemisTaskUnitTarget), id.GuidToString());
    }

    #endregion
}