using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Artemis.Service.Resource.Managers;

/// <summary>
/// 行政区划树管理器
/// </summary>
public interface IDivisionTreeManager : ITreeManager<ArtemisDivision, DivisionInfo, DivisionInfoTree, DivisionPackage>
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
/// 行政区划树管理器实现
/// </summary>
public class DivisionTreeManager : TreeManager<ArtemisDivision, DivisionInfo, DivisionInfoTree, DivisionPackage>, IDivisionTreeManager
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
}