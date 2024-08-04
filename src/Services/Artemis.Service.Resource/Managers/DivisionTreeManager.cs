using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Managers;

/// <summary>
///     行政区划树管理器
/// </summary>
public interface
    IDivisionTreeManager : ISelfReferenceTreeManager<ArtemisDivision, DivisionInfo, DivisionInfoTree, DivisionPackage>
{
    /// <summary>
    ///     根据行政区划信息查搜索政区划
    /// </summary>
    /// <param name="divisionNameSearch">任务名搜索值</param>
    /// <param name="divisionLevel"></param>
    /// <param name="divisionType">任务归属搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="divisionCodeSearch"></param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<DivisionInfo>> FetchDivisionsAsync(
        string? divisionNameSearch,
        string? divisionCodeSearch,
        int? divisionLevel,
        string? divisionType,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     行政区划树管理器实现
/// </summary>
public class DivisionTreeManager :
    SelfReferenceTreeManager<ArtemisDivision, DivisionInfo, DivisionInfoTree, DivisionPackage>,
    IDivisionTreeManager
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    public DivisionTreeManager(IArtemisDivisionStore divisionStore) : base(divisionStore)
    {
    }

    #region Implementation of IDivisionTreeManager

    /// <summary>
    ///     根据行政区划信息查搜索政区划
    /// </summary>
    /// <param name="divisionNameSearch">任务名搜索值</param>
    /// <param name="divisionLevel"></param>
    /// <param name="divisionType">任务归属搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="divisionCodeSearch"></param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<DivisionInfo>> FetchDivisionsAsync(
        string? divisionNameSearch,
        string? divisionCodeSearch,
        int? divisionLevel,
        string? divisionType,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        divisionNameSearch ??= string.Empty;
        divisionCodeSearch ??= string.Empty;
        divisionType ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            divisionNameSearch != string.Empty,
            division => EF.Functions.Like(division.Name, $"%{divisionNameSearch}%"));

        query = query.WhereIf(
            divisionCodeSearch != string.Empty,
            division => EF.Functions.Like(division.Code, $"%{divisionCodeSearch}%"));

        query = query.WhereIf(divisionLevel != null, division => division.Level == divisionLevel);

        query = query.WhereIf(divisionType != string.Empty, division => division.Type == divisionType);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var divisions = await query
            .ProjectToType<DivisionInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<DivisionInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = divisions
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
    protected override async Task<ArtemisDivision> MapNewEntity(
        DivisionPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var entity = await base.MapNewEntity(package, loopIndex, cancellationToken);
        entity.Level = 1;
        return entity;
    }

    /// <summary>
    ///     在添加子节点之前
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override Task BeforeAddChildNode(
        ArtemisDivision parent,
        ArtemisDivision child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        child.Level = parent.Level + 1;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     在移除子节点之前
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override Task BeforeRemoveChildNode(
        ArtemisDivision parent,
        ArtemisDivision child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        child.Level = 1;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<List<DivisionInfo>> FetchNonRootTreeNodeList(Guid key,
        CancellationToken cancellationToken)
    {
        var divisionCode = await EntityStore
            .KeyMatchQuery(key)
            .Select(division => division.Code)
            .FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(divisionCode))
        {
            var list = await EntityStore
                .EntityQuery
                .Where(division => division.Code.StartsWith(divisionCode))
                .ProjectToType<DivisionInfo>().ToListAsync(cancellationToken);

            return list;
        }

        return [];
    }

    #endregion
}