using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Artemis.Service.Resource.Managers;

/// <summary>
/// 标准管理器接口
/// </summary>
public interface IStandardManager : IRequiredOneToManyManager<
    ArtemisStandardCatalog, StandardCatalogInfo, StandardCatalogPackage,
    ArtemisStandardItem, StandardItemInfo, StandardItemPackage>
{
    /// <summary>
    ///     根据标准信息查搜标准目录
    /// </summary>
    /// <param name="catalogNameSearch">目录名搜索值</param>
    /// <param name="catalogCodeSearch">目录编码搜索值</param>
    /// <param name="catalogType">目录类型</param>
    /// <param name="catalogState">目录状态</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<StandardCatalogInfo>> FetchStandardCatalogsAsync(
        string? catalogNameSearch,
        string? catalogCodeSearch,
        string? catalogType,
        string? catalogState,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据标准项目信息查搜标准项目
    /// </summary>
    /// <param name="id">标准目录标识</param>
    /// <param name="itemNameSearch">项目名称搜索值</param>
    /// <param name="itemCodeSearch">项目编码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<StandardItemInfo>> FetchStandardItemsAsync(
        Guid id,
        string? itemNameSearch,
        string? itemCodeSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 标准管理器
/// </summary>
public class StandardManager : RequiredOneToManyManager<
    ArtemisStandardCatalog, StandardCatalogInfo, StandardCatalogPackage,
    ArtemisStandardItem, StandardItemInfo, StandardItemPackage>, IStandardManager
{
    /// <summary>
    ///     管理器构造
    /// </summary>
    public StandardManager(
        IArtemisStandardCatalogStore catalogStore,
        IArtemisStandardItemStore itemStore) : base(catalogStore, itemStore)
    {
    }

    #region Overrides

    /// <summary>
    ///     映射到新实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<ArtemisStandardCatalog> MapNewEntity(
        StandardCatalogPackage package, 
        int loopIndex, 
        CancellationToken cancellationToken = default)
    {
        var catalog = await base.MapNewEntity(package, loopIndex, cancellationToken);
        catalog.State = StandardState.Current;
        catalog.Type = StandardType.VisualStandard;
        return catalog;
    }

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected override Expression<Func<ArtemisStandardItem, bool>> SubEntityKeyMatcher(Guid key)
    {
        return item => item.StandardCatalogId == key;
    }

    /// <summary>
    /// 设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected override void SetSubEntityRelationalKey(ArtemisStandardItem subEntity, Guid key)
    {
        subEntity.StandardCatalogId = key;
    }

    #endregion

    #region Implementation of IStandardManager

    /// <summary>
    ///     根据标准信息查搜标准目录
    /// </summary>
    /// <param name="catalogNameSearch">目录名搜索值</param>
    /// <param name="catalogCodeSearch">目录编码搜索值</param>
    /// <param name="catalogType">目录类型</param>
    /// <param name="catalogState">目录状态</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<StandardCatalogInfo>> FetchStandardCatalogsAsync(
        string? catalogNameSearch, 
        string? catalogCodeSearch, 
        string? catalogType,
        string? catalogState,
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        catalogNameSearch ??= string.Empty;
        catalogCodeSearch ??= string.Empty;
        catalogType ??= string.Empty;
        catalogState ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            catalogNameSearch != string.Empty,
            catalog => EF.Functions.Like(
                catalog.Name, $"%{catalogNameSearch}%"));

        query = query.WhereIf(
            catalogCodeSearch != string.Empty,
            catalog => EF.Functions.Like(
                catalog.Code, $"%{catalogCodeSearch}%"));

        query = query.WhereIf(catalogType != string.Empty, catalog => catalog.Type == catalogType);

        query = query.WhereIf(catalogState != string.Empty, catalog => catalog.State == catalogState);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var catalogs = await query
            .ProjectToType<StandardCatalogInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<StandardCatalogInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = catalogs
        };
    }

    /// <summary>
    ///     根据标准项目信息查搜标准项目
    /// </summary>
    /// <param name="id">标准目录标识</param>
    /// <param name="itemNameSearch">项目名称搜索值</param>
    /// <param name="itemCodeSearch">项目编码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<StandardItemInfo>> FetchStandardItemsAsync(
        Guid id, 
        string? itemNameSearch, 
        string? itemCodeSearch, 
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);
        
        itemNameSearch ??= string.Empty;
        itemCodeSearch ??= string.Empty;

        var query = SubEntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            itemNameSearch != string.Empty,
            item => EF.Functions.Like(
                item.Name, $"%{itemNameSearch}%"));

        query = query.WhereIf(
            itemCodeSearch != string.Empty,
            item => EF.Functions.Like(
                item.Code, $"%{itemCodeSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var items = await query
            .ProjectToType<StandardItemInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<StandardItemInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = items
        };
    }

    #endregion
}