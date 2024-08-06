using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Artemis.Service.Resource.Stores;

namespace Artemis.Service.Resource.Managers;

/// <summary>
///     系统模块树管理器接口
/// </summary>
public interface ISystemModuleTreeManager : ISelfReferenceTreeManager<
    ArtemisSystemModule, SystemModuleInfo, SystemModuleInfoTree, SystemModulePackage>
{
    /// <summary>
    ///     根据系统模块信息查搜索系统模块
    /// </summary>
    /// <param name="moduleNameSearch">任务名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<SystemModuleInfo>> FetchSystemModulesAsync(
        string? moduleNameSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 系统模块树管理器实现
/// </summary>
public class SystemModuleTreeManager : SelfReferenceTreeManager<
    ArtemisSystemModule, SystemModuleInfo, SystemModuleInfoTree, SystemModulePackage>, ISystemModuleTreeManager
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    public SystemModuleTreeManager(IArtemisSystemModuleStore entityStore) : base(entityStore)
    {
    }

    #region Overrides

    /// <summary>
    ///     获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<List<SystemModuleInfo>> FetchNonRootTreeNodeList(Guid key, CancellationToken cancellationToken)
    {
        return EntityStore.EntityQuery.ProjectToType<SystemModuleInfo>().ToListAsync(cancellationToken);
    }

    #endregion

    #region Implementation

    /// <summary>
    ///     根据系统模块信息查搜索系统模块
    /// </summary>
    /// <param name="moduleNameSearch">任务名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<SystemModuleInfo>> FetchSystemModulesAsync(
        string? moduleNameSearch, 
        int page = 1, 
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        moduleNameSearch ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            moduleNameSearch != string.Empty,
            module => EF.Functions.Like(module.Name, $"%{moduleNameSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var modules = await query
            .ProjectToType<SystemModuleInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<SystemModuleInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = modules
        };
    }

    #endregion
}