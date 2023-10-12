using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
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
///     角色管理器
/// </summary>
public class RoleManager : Manager<ArtemisRole>, IRoleManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="roleStore">角色存储访问器</param>
    /// <param name="userStore">用户存储管理器</param>
    /// <param name="userRoleStore">用户角色存储访问器</param>
    /// <param name="roleClaimStore">角色凭据存储访问器</param>
    /// <param name="optionsAccessor"></param>
    /// <param name="cache">缓存以来</param>
    /// <param name="logger">日志依赖</param>
    public RoleManager(
        IArtemisRoleStore roleStore,
        IArtemisUserStore userStore,
        IArtemisUserRoleStore userRoleStore,
        IArtemisRoleClaimStore roleClaimStore,
        ILogger? logger = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        IDistributedCache? cache = null) : base(roleStore, cache, optionsAccessor, logger)
    {
        UserStore = userStore;
        UserRoleStore = userRoleStore;
        RoleClaimStore = roleClaimStore;
    }

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        RoleStore.Dispose();
        UserStore.Dispose();
        UserRoleStore.Dispose();
        RoleClaimStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IArtemisRoleStore RoleStore => (IArtemisRoleStore)Store;

    /// <summary>
    /// 用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore { get; }

    /// <summary>
    ///     角色凭据存储访问器
    /// </summary>
    private IArtemisRoleClaimStore RoleClaimStore { get; }

    /// <summary>
    /// 用户角色存储访问器
    /// </summary>
    private IArtemisUserRoleStore UserRoleStore { get; }

    #endregion

    #region Implementation of IRoleManager

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken"></param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<RoleInfo>> FetchRolesAsync(
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        nameSearch ??= string.Empty;

        var query = RoleStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedName = NormalizeKey(nameSearch);

        query = query.WhereIf(
            nameSearch != string.Empty,
            role => EF.Functions.Like(
                role.NormalizedName,
                $"%{normalizedName}%"));

        var count = await query.LongCountAsync(cancellationToken);

        var roles = await query
            .OrderByDescending(role => role.CreatedAt)
            .Page(page, size)
            .ProjectToType<RoleInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<RoleInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Data = roles
        };
    }

    /// <summary>
    ///     根据角色标识获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    public Task<RoleInfo?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return RoleStore.FindMapEntityAsync<RoleInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    public async Task<(StoreResult result, RoleInfo? role)> CreateRoleAsync(
        RolePackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedName = NormalizeKey(pack.Name);

        var exists = await RoleStore.EntityQuery
            .AnyAsync(role => role.NormalizedName == normalizedName, cancellationToken);

        if (exists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisRole), pack.Name), default);

        var role = Instance.CreateInstance<ArtemisRole>();

        pack.Adapt(role);

        role.NormalizedName = normalizedName;

        var result = await RoleStore.CreateAsync(role, cancellationToken);

        return (result, role.Adapt<RoleInfo>());
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, RoleInfo? role)> UpdateRoleAsync(
        Guid id,
        RolePackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role is not null)
        {
            pack.Adapt(role);

            role.NormalizedName = NormalizeKey(pack.Name);

            var result = await RoleStore.UpdateAsync(role, cancellationToken);

            return (result, role.Adapt<RoleInfo>());
        }

        return (StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString()), default);
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, RoleInfo? role)> CreateOrUpdateRoleAsync(
        Guid id,
        RolePackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
            return await UpdateRoleAsync(id, pack, cancellationToken);

        return await CreateRoleAsync(pack, cancellationToken);
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role != null)
            return await RoleStore.DeleteAsync(role, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        Guid id,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            userNameSearch ??= string.Empty;
            emailSearch ??= string.Empty;
            phoneSearch ??= string.Empty;

            var query = RoleStore
                .KeyMatchQuery(id)
                .SelectMany(artemisRole => artemisRole.Users);

            var total = await query.LongCountAsync(cancellationToken);

            var normalizedNameSearch = NormalizeKey(userNameSearch);

            query = query.WhereIf(
                userNameSearch != string.Empty,
                user => EF.Functions.Like(
                    user.NormalizedUserName,
                    $"%{normalizedNameSearch}%"));

            var normalizedEmailSearch = NormalizeKey(emailSearch);

            query = query.WhereIf(
                emailSearch != string.Empty,
                user => EF.Functions.Like(
                    user.NormalizedEmail,
                    $"%{normalizedEmailSearch}%"));

            query = query.WhereIf(
                phoneSearch != string.Empty,
                user => EF.Functions.Like(
                    user.PhoneNumber,
                    $"%{phoneSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var users = await query
                .OrderBy(user => user.CreatedAt)
                .Page(page, size)
                .ProjectToType<UserInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = users
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString("D"));
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> AddRoleUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());

        var userExists = await UserStore.ExistsAsync(userId, cancellationToken);

        if (!userExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), userId.ToString());

        var userRoleExists = await UserRoleStore.EntityQuery
            .Where(userRole => userRole.RoleId == id)
            .Where(userRole => userRole.UserId == userId)
            .AnyAsync(cancellationToken);

        if (userRoleExists)
            return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), $"userId:{userId},roleId:{id}");

        var userRole = Instance.CreateInstance<ArtemisUserRole>();

        userRole.UserId = userId;
        userRole.RoleId = id;

        return await UserRoleStore.CreateAsync(userRole, cancellationToken);
    }

    /// <summary>
    /// 删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> RemoveRoleUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());

        var userExists = await UserStore.ExistsAsync(userId, cancellationToken);

        if (!userExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), userId.ToString());

        var userRole = await UserRoleStore.EntityQuery
            .Where(userRole => userRole.RoleId == id)
            .Where(userRole => userRole.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userRole == null)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), $"userId:{userId},roleId:{id}");

        return await UserRoleStore.DeleteAsync(userRole, cancellationToken);
    }

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<PageResult<RoleClaimInfo>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            claimTypeSearch ??= string.Empty;

            var query = RoleStore
                .KeyMatchQuery(id)
                .SelectMany(role => role.RoleClaims);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                claimTypeSearch != string.Empty,
                claim => EF.Functions.Like(
                    claim.ClaimType,
                    $"%{claimTypeSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var artemisRoles = await query
                .OrderBy(role => role.CreatedAt)
                .ProjectToType<RoleClaimInfo>()
                .Page(page, size)
                .ToListAsync(cancellationToken);

            return new PageResult<RoleClaimInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = artemisRoles
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<RoleClaimInfo?> GetRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists) return await RoleClaimStore.FindMapEntityAsync<RoleClaimInfo>(claimId, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, RoleClaimInfo? roleClaim)> CreateRoleClaimAsync(
        Guid id,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return (StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString()), default);

        var claimExists = await RoleClaimStore.EntityQuery
            .Where(claim => claim.RoleId == id)
            .Where(claim => claim.CheckStamp == pack.GenerateCheckStamp)
            .AnyAsync(cancellationToken);

        if (claimExists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisRoleClaim), pack.GenerateFlag), default);

        var roleClaim = Instance.CreateInstance<ArtemisRoleClaim>();

        pack.Adapt(roleClaim);
        roleClaim.RoleId = id;

        var result = await RoleClaimStore.CreateAsync(roleClaim, cancellationToken);

        return (result, roleClaim.Adapt<RoleClaimInfo>());
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, RoleClaimInfo? roleClaim)> UpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return (StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString()), default);

        var hasBeenSet = await RoleClaimStore.EntityQuery
            .Where(claim => claim.Id != claimId)
            .Where(claim => claim.CheckStamp != pack.CheckStamp)
            .AnyAsync(cancellationToken);

        if (hasBeenSet)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisRoleClaim), pack.GenerateFlag), default);

        var roleClaim = await RoleClaimStore.FindEntityAsync(claimId, cancellationToken);

        if (roleClaim is null)
            return (StoreResult.EntityNotFoundFailed(nameof(ArtemisRoleClaim), claimId.ToString()), default);

        pack.Adapt(roleClaim);

        var result = await RoleClaimStore.UpdateAsync(roleClaim, cancellationToken);

        return (result, roleClaim.Adapt<RoleClaimInfo>());
    }

    /// <summary>
    ///     创建或更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, RoleClaimInfo? roleClaim)> CreateOrUpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await RoleClaimStore.EntityQuery
            .Where(claim => claim.RoleId == id)
            .Where(claim => claim.Id == claimId)
            .AnyAsync(cancellationToken);

        if (exists)
            return
                await UpdateRoleClaimAsync(id, claimId, pack, cancellationToken);

        return await CreateRoleClaimAsync(id, pack, cancellationToken);
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return StoreResult.EntityFoundFailed(nameof(ArtemisRole), id.ToString());

        var roleClaim = await RoleClaimStore.KeyMatchQuery(claimId)
            .Where(claim => claim.RoleId == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (roleClaim == null)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRoleClaim), claimId.ToString());

        return await RoleClaimStore.DeleteAsync(roleClaim, cancellationToken);
    }

    #endregion
}