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
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore { get; }

    /// <summary>
    ///     角色凭据存储访问器
    /// </summary>
    private IArtemisRoleClaimStore RoleClaimStore { get; }

    /// <summary>
    ///     用户角色存储访问器
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
            .OrderBy(role => role.NormalizedName)
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
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    public async Task<(StoreResult result, RoleInfo? role)> CreateRoleAsync(
        RolePackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedName = NormalizeKey(package.Name);

        var exists = await RoleStore.EntityQuery
            .AnyAsync(role => role.NormalizedName == normalizedName, cancellationToken);

        if (exists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisRole), package.Name), default);

        var role = Instance.CreateInstance<ArtemisRole, RolePackage>(package);

        role.NormalizedName = normalizedName;

        var result = await RoleStore.CreateAsync(role, cancellationToken);

        return (result, role.Adapt<RoleInfo>());
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="packages">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public Task<StoreResult> CreateRolesAsync(
        IEnumerable<RolePackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var rolePackages = packages.ToList();

        var packageRoleNames = rolePackages.Select(package => NormalizeKey(package.Name)).ToList();

        var storedRoleNames = RoleStore.EntityQuery
            .Where(item => packageRoleNames.Contains(item.NormalizedName))
            .Select(role => role.NormalizedName)
            .ToList();

        var notSetRoleNames = packageRoleNames.Except(storedRoleNames).ToList();

        if (notSetRoleNames.Any())
        {
            var roles = rolePackages
                .Where(package => notSetRoleNames.Contains(NormalizeKey(package.Name)))
                .Select(package =>
                {
                    var role = Instance.CreateInstance<ArtemisRole, RolePackage>(package);

                    role.NormalizedName = NormalizeKey(package.Name);

                    return role;
                }).ToList();

            return RoleStore.CreateAsync(roles, cancellationToken);
        }

        var flag = string.Join(',', packageRoleNames);

        return Task.FromResult(StoreResult.EntityFoundFailed(nameof(ArtemisRole), flag));
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<(StoreResult result, RoleInfo? role)> UpdateRoleAsync(
        Guid id,
        RolePackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role is not null)
        {
            package.Adapt(role);

            role.NormalizedName = NormalizeKey(package.Name);

            var result = await RoleStore.UpdateAsync(role, cancellationToken);

            return (result, role.Adapt<RoleInfo>());
        }

        return (StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString()), default);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="packages">更新角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateRolesAsync(
        IEnumerable<KeyValuePair<Guid, RolePackage>> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyValuePairs = packages.ToList();

        var dictionary = keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value);

        var ids = dictionary.Keys;

        var roles = await RoleStore.FindEntitiesAsync(ids, cancellationToken);

        var roleList = roles.ToList();

        if (roleList.Any())
        {
            roles = roleList.Select(role =>
            {
                var package = dictionary[role.Id];

                package.Adapt(role);

                role.NormalizedName = NormalizeKey(package.Name);

                return role;
            }).ToList();

            return await RoleStore.UpdateAsync(roles, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(item => item.ToString()));

        return StoreResult.EntityFoundFailed(nameof(ArtemisRole), flag);
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    public async Task<(StoreResult result, RoleInfo? role)> UpdateOrCreateRoleAsync(
        Guid id,
        RolePackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
            return await UpdateRoleAsync(id, package, cancellationToken);

        return await CreateRoleAsync(package, cancellationToken);
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role is not null)
            return await RoleStore.DeleteAsync(role, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="ids">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteRolesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var idList = ids.ToList();

        var roles = await RoleStore.FindEntitiesAsync(idList, cancellationToken);

        var roleList = roles.ToList();

        if (roleList.Any()) 
            return await RoleStore.DeleteAsync(roleList, cancellationToken);

        var flag = string.Join(',', idList.Select(id => id.ToString()));

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), flag);
    }

    /// <summary>
    ///     根据用户信息搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
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
    ///     添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var userExists = await UserStore.ExistsAsync(userId, cancellationToken);

            if (userExists)
            {
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

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), userId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddRoleUsersAsync(
        Guid id,
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var storeUserIds = await UserStore
                .EntityQuery
                .Where(user => userIds.Contains(user.Id))
                .Select(user => user.Id)
                .ToListAsync(cancellationToken);

            string flag;

            if (storeUserIds.Any())
            {
                var beenSetUserIds = await UserRoleStore.EntityQuery
                    .Where(userRole => userRole.RoleId == id)
                    .Where(userRole => storeUserIds.Contains(userRole.UserId))
                    .Select(userRole => userRole.UserId)
                    .ToListAsync(cancellationToken);

                var notSetUserIds = storeUserIds.Except(beenSetUserIds).ToList();

                if (notSetUserIds.Any())
                {
                    var userRoles = notSetUserIds.Select(userId =>
                    {
                        var userRole = Instance.CreateInstance<ArtemisUserRole>();

                        userRole.UserId = userId;
                        userRole.RoleId = id;

                        return userRole;
                    });

                    return await UserRoleStore.CreateAsync(userRoles, cancellationToken);
                }

                flag = string.Join(',', notSetUserIds.Select(userId => userId.ToString()));

                return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), flag);
            }

            flag = string.Join(',', userIds.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var userExists = await UserStore.ExistsAsync(userId, cancellationToken);

            if (userExists)
            {
                var userRole = await UserRoleStore.EntityQuery
                    .Where(userRole => userRole.RoleId == id)
                    .Where(userRole => userRole.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (userRole is not null) return await UserRoleStore.DeleteAsync(userRole, cancellationToken);

                var flag = $"userId:{userId},roleId:{id}";

                return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), flag);
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), userId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveRoleUsersAsync(
        Guid id,
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var userRoles = await UserRoleStore.EntityQuery
                .Where(userRole => userRole.RoleId == id)
                .Where(userRole => userIds.Contains(userRole.UserId))
                .ToListAsync(cancellationToken);

            if (userRoles.Any()) return await UserRoleStore.DeleteAsync(userRoles, cancellationToken);

            var flag = string.Join(',', userIds.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
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
    /// <returns>角色凭据</returns>
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
    ///     添加角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddRoleClaimAsync(
        Guid id,
        ClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var claimExists = await RoleClaimStore.EntityQuery
                .Where(claim => claim.RoleId == id)
                .Where(claim => claim.CheckStamp == package.CheckStamp)
                .AnyAsync(cancellationToken);

            if (claimExists)
            {
                var flag = $"roleId:{id},claim：{package.GenerateFlag}";

                return StoreResult.EntityFoundFailed(nameof(ArtemisRoleClaim), flag);
            }

            var roleClaim = Instance.CreateInstance<ArtemisRoleClaim, ClaimPackage>(package);

            roleClaim.RoleId = id;

            return await RoleClaimStore.CreateAsync(roleClaim, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="packages">凭据</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddRoleClaimsAsync(
        Guid id,
        IEnumerable<ClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var packageList = packages.ToList();

            var checkStamps = packageList.Select(package => package.CheckStamp).ToList();

            var storedClaimsCheckStamp = await RoleClaimStore.EntityQuery
                .Where(claim => claim.RoleId == id)
                .Where(claim => checkStamps.Contains(claim.CheckStamp))
                .Select(claim => claim.CheckStamp)
                .ToListAsync(cancellationToken);

            var notSetClaimsCheckStamp = checkStamps.Except(storedClaimsCheckStamp).ToList();

            if (notSetClaimsCheckStamp.Any())
            {
                var roleClaims = packageList
                    .Where(package => notSetClaimsCheckStamp.Contains(package.CheckStamp))
                    .Select(package =>
                    {
                        var roleClaim = Instance.CreateInstance<ArtemisRoleClaim, ClaimPackage>(package);

                        roleClaim.RoleId = id;

                        return roleClaim;
                    })
                    .ToList();

                return await RoleClaimStore.CreateAsync(roleClaims, cancellationToken);
            }

            var flag = $"roleId:{id},claims:{string.Join(',', packageList.Select(item => item.GenerateFlag))}";

            return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var roleClaim = await RoleClaimStore.KeyMatchQuery(claimId)
                .Where(claim => claim.RoleId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (roleClaim is not null) return await RoleClaimStore.DeleteAsync(roleClaim, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRoleClaim), claimId.ToString());
        }

        return StoreResult.EntityFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimIds">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveRoleClaimsAsync(
        Guid id,
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var claimIdList = claimIds.ToList();

            var roleClaims = await RoleClaimStore.KeyMatchQuery(claimIdList)
                .Where(claim => claim.RoleId == id)
                .ToListAsync(cancellationToken);

            if (roleClaims.Any()) return await RoleClaimStore.DeleteAsync(roleClaims, cancellationToken);

            var flag = string.Join(',', claimIdList.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRoleClaim), flag);
        }

        return StoreResult.EntityFoundFailed(nameof(ArtemisRole), id.ToString());
    }

    #endregion
}