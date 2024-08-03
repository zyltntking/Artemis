using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Managers;

/// <summary>
/// 数据字典管理器接口
/// </summary>
public interface IDataDictionaryManager : IOtmManager<
    ArtemisDataDictionary, DataDictionaryInfo, DataDictionaryPackage, 
    ArtemisDataDictionaryItem, DataDictionaryItemInfo, DataDictionaryItemPackage>
{
    /// <summary>
    ///     根据字典信息查搜字典
    /// </summary>
    /// <param name="dictionaryNameSearch">字典名搜索值</param>
    /// <param name="dictionaryType">字典类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<DataDictionaryInfo>> FetchDictionariesAsync(
        string? dictionaryNameSearch,
        string? dictionaryType,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据字典项目信息查搜字典项目
    /// </summary>
    /// <param name="id">字典标识</param>
    /// <param name="dictionaryItemKeySearch">字典项目键搜索值</param>
    /// <param name="dictionaryItemValueSearch">字典项目值搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<DataDictionaryItemInfo>> FetchDictionaryItemsAsync(
        Guid id,
        string? dictionaryItemKeySearch,
        string? dictionaryItemValueSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 数据字典管理器
/// </summary>
public class DataDictionaryManager : OtmManager<
    ArtemisDataDictionary, DataDictionaryInfo, DataDictionaryPackage, 
    ArtemisDataDictionaryItem, DataDictionaryItemInfo, DataDictionaryItemPackage>, IDataDictionaryManager
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    public DataDictionaryManager(
        IArtemisDataDictionaryStore dictionaryStore,
        IArtemisDataDictionaryItemStore dictionaryItemStore) : base(dictionaryStore, dictionaryItemStore)
    {
    }

    #region Overrides

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected override Expression<Func<ArtemisDataDictionaryItem, bool>> SubEntityKeyMatcher(Guid key)
    {
        return item => item.DataDictionaryId == key;
    }

    /// <summary>
    /// 设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected override void SetSubEntityRelationalKey(ArtemisDataDictionaryItem subEntity, Guid key)
    {
        subEntity.DataDictionaryId = key;
    }

    #endregion

    #region Implementation of IDataDictionaryManager

    /// <summary>
    ///     根据字典信息查搜字典
    /// </summary>
    /// <param name="dictionaryNameSearch">字典名搜索值</param>
    /// <param name="dictionaryType">字典类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<DataDictionaryInfo>> FetchDictionariesAsync(
        string? dictionaryNameSearch, 
        string? dictionaryType, 
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        dictionaryNameSearch ??= string.Empty;
        dictionaryType ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            dictionaryNameSearch != string.Empty,
            dictionary => EF.Functions.Like(
                dictionary.Name, 
                $"%{dictionaryNameSearch}%"));

        query = query.WhereIf(dictionaryType != string.Empty, dictionary => dictionary.Type == dictionaryType);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var dictionaries = await query
            .ProjectToType<DataDictionaryInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<DataDictionaryInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = dictionaries
        };
    }

    /// <summary>
    ///     根据字典项目信息查搜字典项目
    /// </summary>
    /// <param name="id">字典标识</param>
    /// <param name="dictionaryItemKeySearch">字典项目键搜索值</param>
    /// <param name="dictionaryItemValueSearch">字典项目值搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<DataDictionaryItemInfo>> FetchDictionaryItemsAsync(
        Guid id, 
        string? dictionaryItemKeySearch, 
        string? dictionaryItemValueSearch,
        int page = 1, 
        int size = 20, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await EntityStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {

            dictionaryItemKeySearch ??= string.Empty;
            dictionaryItemValueSearch ??= string.Empty;

            var query = SubEntityStore.EntityQuery
                .Where(item => item.DataDictionaryId == id);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                dictionaryItemKeySearch != string.Empty,
                dictionaryItem => EF.Functions.Like(
                    dictionaryItem.Key,
                    $"%{dictionaryItemKeySearch}%"));

            query = query.WhereIf(
                dictionaryItemValueSearch != string.Empty,
                dictionaryItem => EF.Functions.Like(
                    dictionaryItem.Value,
                    $"%{dictionaryItemValueSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(division => division.CreateBy);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var dictionaryItems = await query
                .ProjectToType<DataDictionaryItemInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<DataDictionaryItemInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = dictionaryItems
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisDataDictionary), id.GuidToString());
    }

    #endregion
}