using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
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
///     认证角色管理器
/// </summary>
public sealed class IdentityRoleManager : Manager, IIdentityRoleManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="roleStore">存储访问器依赖</param>
    /// <param name="userStore"></param>
    /// <param name="roleClaimStore"></param>
    /// <param name="userRoleStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityRoleManager(
        IIdentityRoleStore roleStore,
        IIdentityUserStore userStore,
        IIdentityRoleClaimStore roleClaimStore,
        IIdentityUserRoleStore userRoleStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        RoleStore = roleStore ?? throw new ArgumentNullException(nameof(roleStore));
        UserStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        RoleClaimStore = roleClaimStore ?? throw new ArgumentNullException(nameof(roleClaimStore));
        UserRoleStore = userRoleStore ?? throw new ArgumentNullException(nameof(userRoleStore));
    }

    #region Overrides of KeyLessManager<IdentityRole,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        RoleStore.Dispose();
        UserStore.Dispose();
        RoleClaimStore.Dispose();
        UserRoleStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IIdentityRoleStore RoleStore { get; }

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    ///     角色凭据存储访问器
    /// </summary>
    private IIdentityRoleClaimStore RoleClaimStore { get; }

    /// <summary>
    ///     用户角色关系存储访问器
    /// </summary>
    private IIdentityUserRoleStore UserRoleStore { get; }

    #endregion

    #region Implementation of IIdentityRoleManager

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<RoleInfo>> FetchRolesAsync(
        string? nameSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        nameSearch ??= string.Empty;

        var query = RoleStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedName = nameSearch.StringNormalize();

        query = query.WhereIf(
            nameSearch != string.Empty,
            role => EF.Functions.Like(role.NormalizedName, $"%{normalizedName}%"));

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(role => role.NormalizedName);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var roles = await query
            .ProjectToType<RoleInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<RoleInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = roles
        };
    }

    /// <summary>
    ///     根据角色标识获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
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
    /// <returns>存储结果</returns>
    public async Task<StoreResult> CreateRoleAsync(
        RolePackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedName = package.Name.StringNormalize();

        var exists = await RoleStore.EntityQuery
            .AnyAsync(role => role.NormalizedName == normalizedName, cancellationToken);

        if (exists)
            return StoreResult.EntityFoundFailed(nameof(IdentityRole), package.Name);

        var role = Instance.CreateInstance<IdentityRole, RolePackage>(package);

        role.NormalizedName = normalizedName;

        return await RoleStore.CreateAsync(role, cancellationToken);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="packages">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateRolesAsync(
        IEnumerable<RolePackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var rolePackages = packages.ToList();

        var packageRoleNames = rolePackages.Select(package => package.Name.StringNormalize()).ToList();

        var storedRoleNames = await RoleStore.EntityQuery
            .Where(item => packageRoleNames.Contains(item.NormalizedName))
            .Select(role => role.NormalizedName)
            .ToListAsync(cancellationToken);

        var notSetRoleNames = packageRoleNames.Except(storedRoleNames).ToList();

        if (notSetRoleNames.Any())
        {
            var roles = rolePackages
                .Where(package => notSetRoleNames.Contains(package.Name.StringNormalize()))
                .Select(package =>
                {
                    var role = Instance.CreateInstance<IdentityRole, RolePackage>(package);

                    role.NormalizedName = package.Name.StringNormalize();

                    return role;
                }).ToList();

            return await RoleStore.CreateAsync(roles, cancellationToken);
        }

        var flag = string.Join(',', packageRoleNames);

        return StoreResult.EntityFoundFailed(nameof(IdentityRole), flag);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateRoleAsync(
        Guid id,
        RolePackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role is not null)
        {
            package.Adapt(role);

            role.NormalizedName = package.Name.StringNormalize();

            return await RoleStore.UpdateAsync(role, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="dictionary">更新角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateRolesAsync(
        IDictionary<Guid, RolePackage> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var ids = dictionary.Keys;

        var roles = await RoleStore.FindEntitiesAsync(ids, cancellationToken);

        var roleList = roles.ToList();

        if (roleList.Any())
        {
            roles = roleList.Select(role =>
            {
                var package = dictionary[role.Id];

                package.Adapt(role);

                role.NormalizedName = package.Name.StringNormalize();

                return role;
            }).ToList();

            return await RoleStore.UpdateAsync(roles, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(item => item.IdToString()));

        return StoreResult.EntityFoundFailed(nameof(IdentityRole), flag);
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    public async Task<StoreResult> CreateOrUpdateRoleAsync(
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

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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

        var flag = string.Join(',', idList.Select(id => id.IdToString()));

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), flag);
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
                .SelectMany(artemisRole => artemisRole.Users!);

            var total = await query.LongCountAsync(cancellationToken);

            var normalizedNameSearch = userNameSearch.StringNormalize();

            query = query.WhereIf(
                userNameSearch != string.Empty,
                user => EF.Functions.Like(
                    user.NormalizedUserName,
                    $"%{normalizedNameSearch}%"));

            var normalizedEmailSearch = emailSearch.StringNormalize();

            query = query.WhereIf(
                emailSearch != string.Empty,
                user => EF.Functions.Like(
                    user.NormalizedEmail!,
                    $"%{normalizedEmailSearch}%"));

            query = query.WhereIf(
                phoneSearch != string.Empty,
                user => EF.Functions.Like(
                    user.PhoneNumber!,
                    $"%{phoneSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(user => user.NormalizedUserName);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var users = await query
                .ProjectToType<UserInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = users
            };
        }

        throw new EntityNotFoundException(nameof(IdentityRole), id.IdToString()!);
    }

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户信息</returns>
    public async Task<UserInfo?> GetRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
            return await RoleStore.KeyMatchQuery(id)
                .SelectMany(role => role.Users!)
                .Where(user => user.Id == userId)
                .ProjectToType<UserInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(nameof(IdentityRole), id.IdToString()!);
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
                    return StoreResult.EntityFoundFailed(nameof(IdentityUserRole), $"userId:{userId},roleId:{id}");

                var userRole = Instance.CreateInstance<IdentityUserRole>();

                userRole.UserId = userId;
                userRole.RoleId = id;

                return await UserRoleStore.CreateAsync(userRole, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), userId.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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
                        var userRole = Instance.CreateInstance<IdentityUserRole>();

                        userRole.UserId = userId;
                        userRole.RoleId = id;

                        return userRole;
                    });

                    return await UserRoleStore.CreateAsync(userRoles, cancellationToken);
                }

                flag = string.Join(',', notSetUserIds.Select(userId => userId.IdToString()));

                return StoreResult.EntityFoundFailed(nameof(IdentityUserRole), flag);
            }

            flag = string.Join(',', userIds.Select(item => item.IdToString()));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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

                return StoreResult.EntityNotFoundFailed(nameof(IdentityUserRole), flag);
            }

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), userId.IdToString()!);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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

            var flag = string.Join(',', userIds.Select(item => item.IdToString()));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
    }

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="claimValueSearch"></param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<RoleClaimInfo>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        string? claimValueSearch = null,
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
                .SelectMany(role => role.RoleClaims!);

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

            query = query.OrderBy(role => role.ClaimType).ThenBy(role => role.ClaimValue);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var roleClaims = await query
                .ProjectToType<RoleClaimInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<RoleClaimInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = roleClaims
            };
        }

        throw new EntityNotFoundException(nameof(IdentityRole), id.IdToString()!);
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

        throw new EntityNotFoundException(nameof(IdentityRole), id.IdToString()!);
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
        RoleClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var claimExists = await RoleClaimStore.EntityQuery
                .Where(claim => claim.RoleId == id)
                .Where(claim => claim.ClaimType == package.ClaimType)
                .Where(claim => claim.ClaimValue == package.ClaimValue)
                .AnyAsync(cancellationToken);

            var summary = Normalize.KeyValuePairSummary(package.ClaimType, package.ClaimValue);

            if (claimExists)
            {
                var flag = $"roleId:{id},claim：{summary}";

                return StoreResult.EntityFoundFailed(nameof(IdentityRoleClaim), flag);
            }

            var roleClaim = Instance.CreateInstance<IdentityRoleClaim, ClaimPackage>(package);

            roleClaim.RoleId = id;
            roleClaim.CheckStamp = Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue);

            return await RoleClaimStore.CreateAsync(roleClaim, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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
        IEnumerable<RoleClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var packageList = packages.ToList();

            var checkStamps = packageList.Select(package =>
                    Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue))
                .ToList();

            var storedClaimsCheckStamp = await RoleClaimStore.EntityQuery
                .Where(claim => claim.RoleId == id)
                .Where(claim => checkStamps.Contains(claim.CheckStamp))
                .Select(claim => claim.CheckStamp)
                .ToListAsync(cancellationToken);

            var notSetClaimsCheckStamp = checkStamps.Except(storedClaimsCheckStamp).ToList();

            if (notSetClaimsCheckStamp.Any())
            {
                var roleClaims = packageList
                    .Where(package =>
                        notSetClaimsCheckStamp.Contains(Normalize.KeyValuePairStamp(package.ClaimType,
                            package.ClaimValue)))
                    .Select(package =>
                    {
                        var roleClaim = Instance.CreateInstance<IdentityRoleClaim, ClaimPackage>(package);
                        roleClaim.RoleId = id;
                        roleClaim.CheckStamp = Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue);

                        return roleClaim;
                    })
                    .ToList();

                return await RoleClaimStore.CreateAsync(roleClaims, cancellationToken);
            }

            var flag =
                $"roleId:{id},claims:{string.Join(',', packageList.Select(item => Normalize.KeyValuePairSummary(item.ClaimType, item.ClaimValue)))}";

            return StoreResult.EntityFoundFailed(nameof(IdentityRoleClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleClaimAsync(Guid id, int claimId, RoleClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var roleClaim = await RoleClaimStore.KeyMatchQuery(claimId)
                .Where(roleClaim => roleClaim.RoleId == id)
                .FirstOrDefaultAsync(cancellationToken);

            var summary = Normalize.KeyValuePairSummary(package.ClaimType, package.ClaimValue);

            if (roleClaim is not null)
            {
                roleClaim.ClaimType = package.ClaimType;
                roleClaim.ClaimValue = package.ClaimValue;
                roleClaim.CheckStamp = Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue);

                return await RoleClaimStore.UpdateAsync(roleClaim, cancellationToken);
            }

            var flag = $"roleId:{id},claim：{summary}";

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRoleClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="dictionary">凭据更新字典</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleClaimsAsync(Guid id, IDictionary<int, RoleClaimPackage> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var roleExists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (roleExists)
        {
            var ids = dictionary.Keys;

            var roleClaims = await RoleClaimStore.KeyMatchQuery(ids)
                .Where(roleClaim => roleClaim.RoleId == id)
                .ToListAsync(cancellationToken);

            if (roleClaims.Any())
            {
                roleClaims = roleClaims.Select(roleClaim =>
                {
                    var package = dictionary[roleClaim.Id];
                    roleClaim.ClaimType = package.ClaimType;
                    roleClaim.ClaimValue = package.ClaimValue;
                    roleClaim.CheckStamp = Normalize.KeyValuePairStamp(package.ClaimType, package.ClaimValue);

                    return roleClaim;
                }).ToList();

                return await RoleClaimStore.UpdateAsync(roleClaims, cancellationToken);
            }

            var flag = $"roleId:{id},claims:{string.Join(',', ids)}";

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRoleClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.IdToString()!);
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

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRoleClaim), claimId.IdToString()!);
        }

        return StoreResult.EntityFoundFailed(nameof(IdentityRole), id.IdToString()!);
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

            var flag = string.Join(',', claimIdList.Select(item => item.IdToString()));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRoleClaim), flag);
        }

        return StoreResult.EntityFoundFailed(nameof(IdentityRole), id.IdToString()!);
    }

    #endregion
}