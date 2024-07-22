using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Stores;
using Artemis.Service.Shared.Identity.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证资源管理器
/// </summary>
public sealed class IdentityResourceManager : Manager, IIdentityResourceManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="claimStore">存储访问器依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityResourceManager(
        IIdentityClaimStore claimStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        ClaimStore = claimStore ?? throw new ArgumentNullException(nameof(claimStore));
    }

    #region StoreAccess

    /// <summary>
    ///     认证凭据存储访问器
    /// </summary>
    private IIdentityClaimStore ClaimStore { get; }

    #endregion

    #region Overrides of KeyLessManager<IdentityClaim,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        ClaimStore.Dispose();
    }

    #endregion

    #region Implementation of IIdentityResourceManager

    /// <summary>
    ///     根据凭据信息搜索凭据
    /// </summary>
    /// <param name="claimTypeSearch">凭据类型搜索值</param>
    /// <param name="claimValueSearch">凭据值搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<ClaimInfo>> FetchClaimsAsync(
        string? claimTypeSearch,
        string? claimValueSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        claimTypeSearch ??= string.Empty;

        claimValueSearch ??= string.Empty;

        var query = ClaimStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            claimTypeSearch != string.Empty,
            claim => EF.Functions.Like(
                claim.ClaimType,
                $"%{claimTypeSearch}%"));

        query = query.WhereIf(
            claimValueSearch != string.Empty,
            claim => EF.Functions.Like(
                claim.ClaimValue,
                $"%{claimValueSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(claim => claim.ClaimType);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var claims = await query
            .ProjectToType<ClaimInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<ClaimInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = claims
        };
    }

    /// <summary>
    ///     根据凭据标识获取凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>凭据信息</returns>
    public Task<ClaimInfo?> GetClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return ClaimStore.FindMapEntityAsync<ClaimInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="package">凭据</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateClaimAsync(
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

       var checkStamp = Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue);

        var exists = await ClaimStore.EntityQuery
            .Where(claim => claim.CheckStamp == checkStamp)
            .AnyAsync(cancellationToken);

        var summary = Normalize.KeyValuePairSummary(package.ClaimType, package.ClaimType);

        if (exists) return StoreResult.EntityFoundFailed(nameof(IdentityClaim), summary);

        var claim = Instance.CreateInstance<IdentityClaim, ClaimPackage>(package);

        claim.CheckStamp = checkStamp;

        return await ClaimStore.CreateAsync(claim, cancellationToken);
    }

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateClaimsAsync(
        IEnumerable<ClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claimPackages = packages.ToList();

        var checkStamps = claimPackages
            .Select(claim => Normalize.KeyValuePairStamp(claim.ClaimType, claim.ClaimValue))
            .ToList();

        var storedCheckStamps = await ClaimStore.EntityQuery
            .Where(claim => checkStamps.Contains(claim.CheckStamp))
            .Select(claim => claim.CheckStamp)
            .ToListAsync(cancellationToken);

        var notSetCheckStamps = checkStamps.Except(storedCheckStamps).ToList();

        if (notSetCheckStamps.Any())
        {
            var claims = claimPackages
                .Where(item => notSetCheckStamps
                    .Contains(Normalize.KeyValuePairStamp(item.ClaimType, item.ClaimValue)))
                .Select(item =>
                {
                    var checkStamp = Normalize.KeyValuePairStamp(item.ClaimType, item.ClaimValue);
                    var claim = Instance.CreateInstance<IdentityClaim, ClaimPackage>(item);
                    claim.CheckStamp = checkStamp;
                    return claim;
                })
                .ToList();

            return await ClaimStore.CreateAsync(claims, cancellationToken);
        }

        var flag = $"{string.Join(',', claimPackages
            .Select(item => Normalize.KeyValuePairSummary(item.ClaimType, item.ClaimValue)))}";

        return StoreResult.EntityFoundFailed(nameof(IdentityClaim), flag);
    }

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新成功的凭据信息</returns>
    public async Task<StoreResult> UpdateClaimAsync(
        Guid id, 
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claim = await ClaimStore.FindEntityAsync(id, cancellationToken);

        if (claim is not null)
        {
            package.Adapt(claim);
            claim.CheckStamp = Normalize.KeyValuePairStamp(claim.ClaimType, claim.ClaimValue);

            return await ClaimStore.UpdateAsync(claim, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityClaim), id.IdToString()!);
    }

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateClaimsAsync(
        IDictionary<Guid, ClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyValuePairs = packages.ToList();

        var dictionary = keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        var ids = dictionary.Keys;

        var claims = await ClaimStore.FindEntitiesAsync(ids, cancellationToken);

        var claimList = claims.ToList();

        if (claimList.Any())
        {
            claims = claimList.Select(claim =>
            {
                var package = dictionary[claim.Id];

                package.Adapt(claim);
                claim.CheckStamp = Normalize.KeyValuePairStamp(claim.ClaimType, claim.ClaimValue);

                return claim;
            }).ToList();

            return await ClaimStore.UpdateAsync(claims, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(item => item.IdToString()));

        return StoreResult.EntityFoundFailed(nameof(IdentityClaim), flag);
    }

    /// <summary>
    ///     创建或更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    public async Task<StoreResult> CreateOrUpdateClaimAsync(
        Guid id, 
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await ClaimStore.ExistsAsync(id, cancellationToken);

        if (exists)
            return await UpdateClaimAsync(id, package, cancellationToken);

        return await CreateClaimAsync(package, cancellationToken);
    }

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteClaimAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claim = await ClaimStore.FindEntityAsync(id, cancellationToken);

        if (claim is not null)
            return await ClaimStore.DeleteAsync(claim, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(IdentityClaim), id.IdToString()!);
    }

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="ids">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteClaimsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var idList = ids.ToList();

        var claims = await ClaimStore.FindEntitiesAsync(idList, cancellationToken);

        var claimList = claims.ToList();

        if (claimList.Any())
            return await ClaimStore.DeleteAsync(claimList, cancellationToken);

        var flag = string.Join(',', idList.Select(id => id.IdToString()));

        return StoreResult.EntityNotFoundFailed(nameof(IdentityClaim), flag);
    }

    #endregion
}