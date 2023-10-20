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
///     用户管理器
/// </summary>
public class UserManager : Manager<ArtemisUser>, IUserManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">存储访问器依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="roleStore">角色存储访问器依赖</param>
    /// <param name="userRoleStore">用户角色存储访问器依赖</param>
    /// <param name="userTokenStore">用户令牌存储访问器依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userClaimStore">用户凭据存储访问器依赖</param>
    /// <param name="userLoginStore">用户登录存储访问器依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserManager(
        IArtemisUserStore userStore,
        IArtemisRoleStore roleStore,
        IArtemisUserRoleStore userRoleStore,
        IArtemisUserClaimStore userClaimStore,
        IArtemisUserLoginStore userLoginStore,
        IArtemisUserTokenStore userTokenStore,
        ILogger? logger = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        IDistributedCache? cache = null) : base(userStore, cache, optionsAccessor, logger)
    {
        RoleStore = roleStore;
        UserRoleStore = userRoleStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
    }

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        RoleStore.Dispose();
        UserRoleStore.Dispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore => (IArtemisUserStore)Store;

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IArtemisRoleStore RoleStore { get; }

    /// <summary>
    ///     用户角色存储访问器
    /// </summary>
    private IArtemisUserRoleStore UserRoleStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IArtemisUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     用户登录存储访问器
    /// </summary>
    private IArtemisUserLoginStore UserLoginStore { get; }

    /// <summary>
    ///     用户令牌存储访问器
    /// </summary>
    private IArtemisUserTokenStore UserTokenStore { get; }

    #endregion

    #region Implementation of IUserManager

    /// <summary>
    ///     根据用户信息搜索用户
    /// </summary>
    /// <param name="nameSearch">用户名搜索值</param>
    /// <param name="emailSearch">邮箱搜索值</param>
    /// <param name="phoneNumberSearch">电话号码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<UserInfo>> FetchUserAsync(
        string? nameSearch,
        string? emailSearch,
        string? phoneNumberSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        nameSearch ??= string.Empty;

        emailSearch ??= string.Empty;

        phoneNumberSearch ??= string.Empty;

        var query = UserStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedName = NormalizeKey(nameSearch);

        var normalizedEmail = NormalizeKey(emailSearch);

        query = query.WhereIf(
            normalizedName != string.Empty,
            user => EF.Functions.Like(
                user.NormalizedUserName,
                $"%{normalizedName}%"));

        query = query.WhereIf(
            normalizedEmail != string.Empty,
            user => EF.Functions.Like(
                user.NormalizedEmail!,
                $"%{normalizedEmail}%"));

        query = query.WhereIf(
            phoneNumberSearch != string.Empty,
            user => EF.Functions.Like(
                user.PhoneNumber!,
                $"%{phoneNumberSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        var users = await query
            .OrderByDescending(user => user.CreatedAt)
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

    /// <summary>
    ///     根据用户标识获取用户
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>查询不到用户实例时返回空</returns>
    public Task<UserInfo?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        return UserStore.FindMapEntityAsync<UserInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="package">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的用户实例</returns>
    public async Task<(StoreResult result, UserInfo? user)> CreateUserAsync(
        UserPackage package,
        string password,
        CancellationToken cancellationToken)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedUserName = NormalizeKey(package.UserName);

        var exists = await UserStore.EntityQuery
            .AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);

        if (exists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisUser), package.UserName), default);

        var user = Instance.CreateInstance<ArtemisUser, UserPackage>(package);

        user.NormalizedUserName = normalizedUserName;

        user.NormalizedEmail = package.Email is not null ? NormalizeKey(package.Email) : string.Empty;

        user.PasswordHash = Hash.ArtemisHash(password);

        user.SecurityStamp = package.GenerateSecurityStamp;

        var result = await UserStore.CreateAsync(user, cancellationToken);

        return (result, user.Adapt<UserInfo>());
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="packages">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateUsersAsync(
        IEnumerable<KeyValuePair<UserPackage, string>> packages,
        CancellationToken cancellationToken)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userPackages = packages.ToList();

        var packageUserNames = userPackages.Select(item => NormalizeKey(item.Key.UserName)).ToList();

        var storedUserNames = await UserStore.EntityQuery
            .Where(user => packageUserNames.Contains(user.NormalizedUserName))
            .Select(user => user.NormalizedUserName)
            .ToListAsync(cancellationToken);

        var notSetUserNames = packageUserNames.Except(storedUserNames).ToList();

        if (notSetUserNames.Any())
        {
            var users = userPackages
                .Where(item =>
                    notSetUserNames.Contains(NormalizeKey(item.Key.UserName))
                )
                .Select(item =>
                {
                    var (package, password) = item;

                    var user = Instance.CreateInstance<ArtemisUser, UserPackage>(package);

                    user.NormalizedUserName = NormalizeKey(package.UserName);

                    user.NormalizedEmail = package.Email is not null ? NormalizeKey(package.Email) : string.Empty;

                    user.PasswordHash = Hash.ArtemisHash(password);

                    user.SecurityStamp = package.GenerateSecurityStamp;

                    return user;
                }).ToList();

            return await UserStore.CreateAsync(users, cancellationToken);
        }

        var flag = string.Join(',', packageUserNames);

        return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), flag);
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新后的实体</returns>
    public async Task<(StoreResult result, UserInfo? user)> UpdateUserAsync(
        Guid id,
        UserPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var user = await UserStore.FindEntityAsync(id, cancellationToken);

        if (user is not null)
        {
            package.Adapt(user);

            user.NormalizedUserName = NormalizeKey(package.UserName);

            user.NormalizedEmail = package.Email is not null ? NormalizeKey(package.Email) : string.Empty;

            user.SecurityStamp = package.GenerateSecurityStamp;

            var result = await UserStore.UpdateAsync(user, cancellationToken);

            return (result, user.Adapt<UserInfo>());
        }

        return (StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString()), default);
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="packages">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateUsersAsync(
        IEnumerable<KeyValuePair<Guid, UserPackage>> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyValuePairs = packages.ToList();

        var dictionary = keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value);

        var ids = dictionary.Keys;

        var users = await UserStore.FindEntitiesAsync(ids, cancellationToken);

        var userList = users.ToList();

        if (userList.Any())
        {
            users = userList.Select(user =>
            {
                var package = dictionary[user.Id];

                package.Adapt(user);

                user.NormalizedUserName = NormalizeKey(package.UserName);

                user.NormalizedEmail = package.Email is not null ? NormalizeKey(package.Email) : string.Empty;

                user.SecurityStamp = package.GenerateSecurityStamp;

                return user;
            }).ToList();

            return await UserStore.UpdateAsync(users, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(id => id.ToString()));

        return StoreResult.EntityFoundFailed(nameof(ArtemisRole), flag);
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var user = await UserStore.FindEntityAsync(id, cancellationToken);

        if (user != null)
            return await UserStore.DeleteAsync(user, cancellationToken);

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="ids">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteUsersAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var idList = ids.ToList();

        var users = await UserStore.FindEntitiesAsync(idList, cancellationToken);

        var userList = users.ToList();

        if (userList.Any())
            return await UserStore.DeleteAsync(userList, cancellationToken);

        var flag = string.Join(',', idList.Select(id => id.ToString()));

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), flag);
    }

    /// <summary>
    ///     根据角色名搜索该用户具有的角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleNameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<RoleInfo>> FetchUserRolesAsync(
        Guid id,
        string? roleNameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            roleNameSearch ??= string.Empty;

            var query = UserStore
                .KeyMatchQuery(id)
                .SelectMany(artemisUser => artemisUser.Roles);

            var total = await query.LongCountAsync(cancellationToken);

            var normalizedNameSearch = NormalizeKey(roleNameSearch);

            query = query.WhereIf(
                roleNameSearch != string.Empty,
                role => EF.Functions.Like(
                    role.NormalizedName,
                    $"%{normalizedNameSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var roles = await query
                .OrderBy(role => role.CreatedAt)
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

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString("D"));
    }

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>查询角色结果</returns>
    public async Task<RoleInfo?> GetUserRoleAsync(Guid id, Guid roleId, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
            return await UserStore.KeyMatchQuery(id)
                .SelectMany(user => user.Roles)
                .Where(role => role.Id == roleId)
                .ProjectToType<RoleInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var roleExists = await RoleStore.ExistsAsync(roleId, cancellationToken);

            if (roleExists)
            {
                var userRoleExists = await UserRoleStore.EntityQuery
                    .Where(userRole => userRole.UserId == id)
                    .Where(userRole => userRole.RoleId == roleId)
                    .AnyAsync(cancellationToken);

                if (userRoleExists)
                    return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), $"userId:{id},roleId:{roleId}");

                var userRole = Instance.CreateInstance<ArtemisUserRole>();

                userRole.UserId = id;
                userRole.RoleId = roleId;

                return await UserRoleStore.CreateAsync(userRole, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), roleId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleIds">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserRolesAsync(
        Guid id,
        IEnumerable<Guid> roleIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var storeRoleIds = await RoleStore
                .EntityQuery
                .Where(role => roleIds.Contains(role.Id))
                .Select(role => role.Id)
                .ToListAsync(cancellationToken);

            string flag;

            if (storeRoleIds.Any())
            {
                var beenSetRoleIds = await UserRoleStore.EntityQuery
                    .Where(userRole => userRole.UserId == id)
                    .Where(userRole => storeRoleIds.Contains(userRole.RoleId))
                    .Select(userRole => userRole.RoleId)
                    .ToListAsync(cancellationToken);

                var notSetRoleIds = storeRoleIds.Except(beenSetRoleIds).ToList();

                if (notSetRoleIds.Any())
                {
                    var userRoles = notSetRoleIds.Select(roleId =>
                    {
                        var userRole = Instance.CreateInstance<ArtemisUserRole>();

                        userRole.UserId = id;
                        userRole.RoleId = roleId;

                        return userRole;
                    });

                    return await UserRoleStore.CreateAsync(userRoles, cancellationToken);
                }

                flag = string.Join(',', notSetRoleIds.Select(userId => userId.ToString()));

                return StoreResult.EntityFoundFailed(nameof(ArtemisUserRole), flag);
            }

            flag = string.Join(',', roleIds.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var roleExists = await RoleStore.ExistsAsync(roleId, cancellationToken);

            if (!roleExists)
            {
                var userRole = await UserRoleStore.EntityQuery
                    .Where(userRole => userRole.UserId == id)
                    .Where(userRole => userRole.RoleId == roleId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (userRole is not null) return await UserRoleStore.DeleteAsync(userRole, cancellationToken);

                var flag = $"userId:{id},roleId:{roleId}";

                return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), flag);
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleIds">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserRolesAsync(
        Guid id,
        IEnumerable<Guid> roleIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userRoles = await UserRoleStore.EntityQuery
                .Where(userRole => userRole.UserId == id)
                .Where(userRole => roleIds.Contains(userRole.RoleId))
                .ToListAsync(cancellationToken);

            if (userRoles.Any()) return await UserRoleStore.DeleteAsync(userRoles, cancellationToken);

            var flag = string.Join(',', roleIds.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     查询用户的凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<UserClaimInfo>> FetchUserClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            claimTypeSearch ??= string.Empty;

            var query = UserStore
                .KeyMatchQuery(id)
                .SelectMany(artemisUser => artemisUser.Claims);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                claimTypeSearch != string.Empty,
                claim => EF.Functions.Like(
                    claim.ClaimType,
                    $"%{claimTypeSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var userClaims = await query
                .OrderBy(claim => claim.CreatedAt)
                .Page(page, size)
                .ProjectToType<UserClaimInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserClaimInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = userClaims
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     获取用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    public async Task<UserClaimInfo?> GetUserClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists) return await UserClaimStore.FindMapEntityAsync<UserClaimInfo>(claimId, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserClaimAsync(
        Guid id,
        UserClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var claimExists = await UserClaimStore.EntityQuery
                .Where(userClaim => userClaim.UserId == id)
                .Where(userClaim => userClaim.CheckStamp == package.CheckStamp)
                .AnyAsync(cancellationToken);

            if (claimExists)
            {
                var flag = $"userId:{id},claim：{package.GenerateFlag}";

                return StoreResult.EntityFoundFailed(nameof(ArtemisUserClaim), flag);
            }

            var userClaim = Instance.CreateInstance<ArtemisUserClaim, UserClaimPackage>(package);

            userClaim.UserId = id;

            return await UserClaimStore.CreateAsync(userClaim, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserClaimsAsync(
        Guid id,
        IEnumerable<UserClaimPackage> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var packageList = packages.ToList();

            var checkStamps = packageList.Select(package => package.CheckStamp).ToList();

            var storedClaimCheckStamp = await UserClaimStore.EntityQuery
                .Where(userClaim => userClaim.UserId == id)
                .Where(userClaim => checkStamps.Contains(userClaim.CheckStamp))
                .Select(userClaim => userClaim.CheckStamp)
                .ToListAsync(cancellationToken);

            var notSetCheckStamps = checkStamps.Except(storedClaimCheckStamp).ToList();

            if (notSetCheckStamps.Any())
            {
                var userClaims = packageList
                    .Where(package => notSetCheckStamps.Contains(package.CheckStamp))
                    .Select(package =>
                    {
                        var userClaim = Instance.CreateInstance<ArtemisUserClaim, UserClaimPackage>(package);

                        userClaim.UserId = id;

                        return userClaim;
                    })
                    .ToList();

                return await UserClaimStore.CreateAsync(userClaims, cancellationToken);
            }

            var flag = $"userId:{id},claims:{string.Join(',', packageList.Select(item => item.GenerateFlag))}";

            return StoreResult.EntityFoundFailed(nameof(ArtemisUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userClaim = await UserClaimStore.KeyMatchQuery(claimId)
                .Where(userClaim => userClaim.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userClaim is not null) return await UserClaimStore.DeleteAsync(userClaim, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserClaim), claimId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimIds">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserClaimsAsync(
        Guid id,
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var claimIdList = claimIds.ToList();

            var userClaims = await UserClaimStore.KeyMatchQuery(claimIdList)
                .Where(userClaim => userClaim.UserId == id)
                .ToListAsync(cancellationToken);

            if (userClaims.Any()) return await UserClaimStore.DeleteAsync(userClaims, cancellationToken);

            var flag = string.Join(',', claimIdList.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     查询用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProviderSearch">登录提供程序搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到登录信息实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<UserLoginInfo>> FetchUserLoginsAsync(
        Guid id,
        string? loginProviderSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            loginProviderSearch ??= string.Empty;

            var query = UserStore
                .KeyMatchQuery(id)
                .SelectMany(artemisUser => artemisUser.Logins);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                loginProviderSearch != string.Empty,
                userLogin => EF.Functions.Like(
                    userLogin.LoginProvider,
                    $"%{loginProviderSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var userLogins = await query
                .OrderBy(userLogin => userLogin.CreatedAt)
                .Page(page, size)
                .ProjectToType<UserLoginInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserLoginInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = userLogins
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    public async Task<UserLoginInfo?> GetUserLoginAsync(
        Guid id,
        int loginId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists) return await UserLoginStore.FindMapEntityAsync<UserLoginInfo>(loginId, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserLoginAsync(
        Guid id,
        UserLoginPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var loginExists = await UserLoginStore.EntityQuery
                .Where(userLogin => userLogin.UserId == id)
                .Where(userLogin => userLogin.LoginProvider == package.LoginProvider)
                .Where(userLogin => userLogin.ProviderKey == package.ProviderKey)
                .AnyAsync(cancellationToken);

            if (loginExists)
            {
                var flag = $"userId:{id},login：{package.GenerateFlag}";

                return StoreResult.EntityFoundFailed(nameof(ArtemisUserLogin), flag);
            }

            var userLogin = Instance.CreateInstance<ArtemisUserLogin, UserLoginPackage>(package);

            userLogin.UserId = id;

            return await UserLoginStore.CreateAsync(userLogin, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     替换用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录信息标识</param>
    /// <param name="package">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>替换结果</returns>
    public async Task<StoreResult> ReplaceUserLoginAsync(
        Guid id,
        int loginId,
        UserLoginPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userLogin = await UserLoginStore.KeyMatchQuery(loginId)
                .Where(userLogin => userLogin.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userLogin is not null)
            {
                package.Adapt(userLogin);

                return await UserLoginStore.UpdateAsync(userLogin, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserLogin), loginId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserLoginAsync(
        Guid id,
        int loginId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userLogin = await UserLoginStore.KeyMatchQuery(loginId)
                .Where(userLogin => userLogin.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userLogin is not null) return await UserLoginStore.DeleteAsync(userLogin, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserLogin), loginId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="loginIds">登录标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserLoginsAsync(
        Guid id,
        IEnumerable<int> loginIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var loginIdList = loginIds.ToList();

            var userLogins = await UserLoginStore.KeyMatchQuery(loginIdList)
                .Where(userLogin => userLogin.UserId == id)
                .ToListAsync(cancellationToken);

            if (userLogins.Any()) return await UserLoginStore.DeleteAsync(userLogins, cancellationToken);

            var flag = string.Join(',', loginIdList.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserLogin), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     查询用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProviderSearch">登录提供程序搜索值</param>
    /// <param name="nameSearch">令牌名</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到登录信息实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<UserTokenInfo>> FetchUserTokensAsync(
        Guid id,
        string? loginProviderSearch = null,
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            loginProviderSearch ??= string.Empty;

            nameSearch ??= string.Empty;

            var query = UserStore
                .KeyMatchQuery(id)
                .SelectMany(artemisUser => artemisUser.Tokens);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                loginProviderSearch != string.Empty,
                userToken => EF.Functions.Like(
                    userToken.LoginProvider,
                    $"%{loginProviderSearch}%"));

            query = query.WhereIf(
                nameSearch != string.Empty,
                userToken => EF.Functions.Like(
                    userToken.Name,
                    $"%{nameSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            var userTokens = await query
                .OrderBy(userToken => userToken.CreatedAt)
                .Page(page, size)
                .ProjectToType<UserTokenInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserTokenInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = userTokens
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    public async Task<UserTokenInfo?> GetUserTokenAsync(
        Guid id,
        int tokenId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists) return await UserTokenStore.FindMapEntityAsync<UserTokenInfo>(tokenId, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     添加用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">令牌信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    public async Task<StoreResult> AddUserTokenAsync(
        Guid id,
        UserTokenPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var tokenExists = await UserTokenStore.EntityQuery
                .Where(userToken => userToken.UserId == id)
                .Where(userToken => userToken.LoginProvider == package.LoginProvider)
                .Where(userToken => userToken.Name == package.Name)
                .AnyAsync(cancellationToken);

            if (tokenExists)
            {
                var flag = $"userId:{id},token：{package.GenerateFlag}";

                return StoreResult.EntityFoundFailed(nameof(ArtemisUserToken), flag);
            }

            var userToken = Instance.CreateInstance<ArtemisUserToken, UserTokenPackage>(package);

            userToken.UserId = id;

            return await UserTokenStore.CreateAsync(userToken, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     替换用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌信息标识</param>
    /// <param name="package">令牌信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>替换结果</returns>
    public async Task<StoreResult> ReplaceUserTokenAsync(
        Guid id,
        int tokenId,
        UserTokenPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userToken = await UserTokenStore.KeyMatchQuery(tokenId)
                .Where(userToken => userToken.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userToken is not null)
            {
                package.Adapt(userToken);

                return await UserTokenStore.UpdateAsync(userToken, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserToken), tokenId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserTokenAsync(
        Guid id,
        int tokenId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userToken = await UserTokenStore.KeyMatchQuery(tokenId)
                .Where(userToken => userToken.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userToken is not null) return await UserTokenStore.DeleteAsync(userToken, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserToken), tokenId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="tokenIds">令牌标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserTokensAsync(
        Guid id,
        IEnumerable<int> tokenIds,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var tokenIdList = tokenIds.ToList();

            var userTokens = await UserTokenStore.KeyMatchQuery(tokenIdList)
                .Where(userToken => userToken.UserId == id)
                .ToListAsync(cancellationToken);

            if (userTokens.Any()) return await UserTokenStore.DeleteAsync(userTokens, cancellationToken);

            var flag = string.Join(',', tokenIdList.Select(item => item.ToString()));

            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserToken), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());
    }

    #endregion
}