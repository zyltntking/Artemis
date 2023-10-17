using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     凭据管理器
/// </summary>
public class ClaimManager : Manager<ArtemisClaim>, IClaimManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="claimStore">存储访问器依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ClaimManager(
        IArtemisClaimStore claimStore,
        IDistributedCache? cache = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        ILogger? logger = null) : base(claimStore, cache, optionsAccessor, logger)
    {
    }

    #region StoreAccess

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IArtemisClaimStore ClaimStore => (IArtemisClaimStore)Store;

    #endregion

    #region Overrides of Manager<ArtemisClaim,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        ClaimStore.Dispose();
    }

    #endregion

    #region Implementation of IClaimManager

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

        var claims = await query
            .OrderBy(claim => claim.ClaimType)
            .Page(page, size)
            .ProjectToType<ClaimInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<ClaimInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Data = claims
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
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果和创建成功的凭据实例</returns>
    public async Task<(StoreResult result, ClaimInfo? claim)> CreateClaimAsync(
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await ClaimStore.EntityQuery
            .Where(claim => claim.CheckStamp == package.CheckStamp)
            .AnyAsync(cancellationToken);

        if (exists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisClaim), package.GenerateFlag), default);

        var claim = Instance.CreateInstance<ArtemisClaim, ClaimPackage>(package);

        var result = await ClaimStore.CreateAsync(claim, cancellationToken);

        return (result, claim.Adapt<ClaimInfo>());
    }

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public Task<StoreResult> CreateClaimsAsync(
        IEnumerable<ClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claimPackages = packages.ToList();

        var checkStamps = claimPackages.Select(claim => claim.CheckStamp).ToList();

        var storedCheckStamps = ClaimStore.EntityQuery
            .Where(claim => checkStamps.Contains(claim.CheckStamp))
            .Select(claim => claim.CheckStamp)
            .ToList();

        var notSetCheckStamps = checkStamps.Except(storedCheckStamps).ToList();

        if (notSetCheckStamps.Any())
        {
            var claims = claimPackages
                .Where(claim => notSetCheckStamps.Contains(claim.CheckStamp))
                .Select(Instance.CreateInstance<ArtemisClaim, ClaimPackage>)
                .ToList();

            return ClaimStore.CreateAsync(claims, cancellationToken);
        }

        var flag = string.Join(',', claimPackages.Select(item => item.GenerateFlag));

        return Task.FromResult(StoreResult.EntityFoundFailed(nameof(ArtemisClaim), flag));
    }

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新成功的凭据信息</returns>
    public async Task<(StoreResult result, ClaimInfo? claim)> UpdateClaimAsync(
        Guid id,
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claim = await ClaimStore.FindEntityAsync(id, cancellationToken);

        if (claim is not null)
        {
            package.Adapt(claim);

            var result = await ClaimStore.UpdateAsync(claim, cancellationToken);

            return (result, claim.Adapt<ClaimInfo>());
        }

        return (StoreResult.EntityNotFoundFailed(nameof(ArtemisClaim), id.ToString()), default);
    }

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateClaimsAsync(
        IEnumerable<KeyValuePair<Guid, ClaimPackage>> packages,
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

                return claim;
            }).ToList();

            return await ClaimStore.UpdateAsync(claims, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(item => item.ToString()));

        return StoreResult.EntityFoundFailed(nameof(ArtemisClaim), flag);
    }

    /// <summary>
    ///     创建或更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    public async Task<(StoreResult result, ClaimInfo? role)> UpdateOrCreateClaimAsync(Guid id, ClaimPackage package,
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
    public async Task<StoreResult> DeleteClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var claim = await ClaimStore.FindEntityAsync(id, cancellationToken);

        if (claim is not null)
            return await ClaimStore.DeleteAsync(claim, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="ids">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteClaimsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var idList = ids.ToList();

        var claims = await ClaimStore.FindEntitiesAsync(idList, cancellationToken);

        var claimList = claims.ToList();

        if (claimList.Any())
            return await ClaimStore.DeleteAsync(claimList, cancellationToken);

        var flag = string.Join(',', idList.Select(id => id.ToString()));

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisClaim), flag);
    }

    #endregion
}