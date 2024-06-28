using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证用户管理
/// </summary>
public sealed class IdentityUserManager : Manager<IdentityUser, Guid, Guid>, IIdentityUserManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">存储访问器依赖</param>
    /// <param name="userTokenStore"></param>
    /// <param name="userRoleStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="roleStore"></param>
    /// <param name="userClaimStore"></param>
    /// <param name="userLoginStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityUserManager(
        IIdentityUserStore userStore,
        IIdentityRoleStore roleStore,
        IIdentityUserClaimStore userClaimStore,
        IIdentityUserLoginStore userLoginStore,
        IIdentityUserTokenStore userTokenStore,
        IIdentityUserRoleStore userRoleStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(userStore, options, logger)
    {
        UserStore = userStore;
        RoleStore = roleStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        UserRoleStore = userRoleStore;
    }

    #region Dispose

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        RoleStore.Dispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
        UserRoleStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IIdentityRoleStore RoleStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     用户登录存储访问器
    /// </summary>
    private IIdentityUserLoginStore UserLoginStore { get; }

    /// <summary>
    ///     用户令牌存储访问器
    /// </summary>
    private IIdentityUserTokenStore UserTokenStore { get; }

    /// <summary>
    ///     用户角色关系存储访问器
    /// </summary>
    private IIdentityUserRoleStore UserRoleStore { get; }

    #endregion

    #region Implementation of IIdentityUserManager

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

        var normalizedName = nameSearch.StringNormalize();

        var normalizedEmail = emailSearch.StringNormalize();

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
    /// <param name="userSign">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的用户实例</returns>
    public async Task<StoreResult> CreateUserAsync(
        UserSign userSign,
        string password,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedUserName = userSign.UserName.StringNormalize();

        var exists = await UserStore.EntityQuery
            .AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);

        if (exists)
            return StoreResult.EntityFoundFailed(nameof(IdentityUser), userSign.UserName);

        var user = Instance.CreateInstance<IdentityUser, UserSign>(userSign);

        user.NormalizedUserName = normalizedUserName;

        user.NormalizedEmail = userSign.Email is not null ? userSign.Email.StringNormalize() : string.Empty;

        user.PasswordHash = Hash.PasswordHash(password);

        user.SecurityStamp = Generator.SecurityStamp;

        var result = await UserStore.CreateAsync(user, cancellationToken);

        return result;
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="dictionary">批量创建用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    public async Task<StoreResult> CreateUsersAsync(
        IDictionary<UserSign, string> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userPackages = dictionary.ToList();

        var packageUserNames = userPackages.Select(item => item.Key.UserName.StringNormalize()).ToList();

        var storedUserNames = await UserStore.EntityQuery
            .Where(user => packageUserNames.Contains(user.NormalizedUserName))
            .Select(user => user.NormalizedUserName)
            .ToListAsync(cancellationToken);

        var notSetUserNames = packageUserNames.Except(storedUserNames).ToList();

        if (notSetUserNames.Any())
        {
            var users = userPackages
                .Where(item =>
                    notSetUserNames.Contains(item.Key.UserName.StringNormalize())
                )
                .Select(item =>
                {
                    var (package, password) = item;

                    var user = Instance.CreateInstance<IdentityUser, UserSign>(package);

                    user.NormalizedUserName = package.UserName.StringNormalize();

                    user.NormalizedEmail = package.Email is not null ? package.Email.StringNormalize() : string.Empty;

                    user.PasswordHash = Hash.PasswordHash(password);

                    user.SecurityStamp = Generator.SecurityStamp;

                    return user;
                }).ToList();

            return await UserStore.CreateAsync(users, cancellationToken);
        }

        var flag = string.Join(',', packageUserNames);

        return StoreResult.EntityFoundFailed(nameof(IdentityUser), flag);
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新后的实体</returns>
    public async Task<StoreResult> UpdateUserAsync(
        Guid id,
        UserPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var user = await UserStore.FindEntityAsync(id, cancellationToken);

        if (user is not null)
        {
            package.Adapt(user);

            user.NormalizedUserName = package.UserName.StringNormalize();

            user.NormalizedEmail = package.Email is not null ? package.Email.StringNormalize() : string.Empty;

            user.SecurityStamp = Generator.SecurityStamp;

            var result = await UserStore.UpdateAsync(user, cancellationToken);

            return result;
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="dictionary">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    public async Task<StoreResult> UpdateUsersAsync(
        IDictionary<Guid, UserPackage> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var ids = dictionary.Keys;

        var users = await UserStore.FindEntitiesAsync(ids, cancellationToken);

        var userList = users.ToList();

        if (userList.Any())
        {
            users = userList.Select(user =>
            {
                var package = dictionary[user.Id];

                package.Adapt(user);

                user.NormalizedUserName = package.UserName.StringNormalize();

                user.NormalizedEmail = package.Email is not null ? package.Email.StringNormalize() : string.Empty;

                user.SecurityStamp = Generator.SecurityStamp;

                return user;
            }).ToList();

            return await UserStore.UpdateAsync(users, cancellationToken);
        }

        var flag = string.Join(',', ids.Select(id => id.GuidToString()));

        return StoreResult.EntityFoundFailed(nameof(IdentityUser), flag);
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

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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

        var flag = string.Join(',', idList.Select(id => id.GuidToString()));

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), flag);
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
                .SelectMany(artemisUser => artemisUser.Roles!);

            var total = await query.LongCountAsync(cancellationToken);

            var normalizedNameSearch = roleNameSearch.StringNormalize();

            query = query.WhereIf(
                roleNameSearch != string.Empty,
                role => EF.Functions.Like(
                    role.NormalizedName,
                    $"%{normalizedNameSearch}%"));

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

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>查询角色结果</returns>
    public async Task<RoleInfo?> GetUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
            return await UserStore.KeyMatchQuery(id)
                .SelectMany(user => user.Roles!)
                .Where(role => role.Id == roleId)
                .ProjectToType<RoleInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
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
                    return StoreResult.EntityFoundFailed(nameof(IdentityUserRole), $"userId:{id},roleId:{roleId}");

                var userRole = Instance.CreateInstance<IdentityUserRole>();

                userRole.UserId = id;
                userRole.RoleId = roleId;

                return await UserRoleStore.CreateAsync(userRole, cancellationToken);
            }

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), roleId.GuidToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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
                        var userRole = Instance.CreateInstance<IdentityUserRole>();

                        userRole.UserId = id;
                        userRole.RoleId = roleId;

                        return userRole;
                    });

                    return await UserRoleStore.CreateAsync(userRoles, cancellationToken);
                }

                flag = string.Join(',', notSetRoleIds.Select(userId => userId.GuidToString()));

                return StoreResult.EntityFoundFailed(nameof(IdentityUserRole), flag);
            }

            flag = string.Join(',', roleIds.Select(item => item.GuidToString()));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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

                return StoreResult.EntityNotFoundFailed(nameof(IdentityUserRole), flag);
            }

            return StoreResult.EntityNotFoundFailed(nameof(IdentityRole), id.GuidToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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

            var flag = string.Join(',', roleIds.Select(item => item.GuidToString()));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserRole), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     查询用户的凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="claimValueSearch"></param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    public async Task<PageResult<UserClaimInfo>> FetchUserClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        string? claimValueSearch = null,
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
                .SelectMany(artemisUser => artemisUser.UserClaims!);

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

            query = query
                .OrderBy(claim => claim.ClaimType)
                .ThenBy(claim => claim.ClaimValue);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var userClaims = await query
                .ProjectToType<UserClaimInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserClaimInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = userClaims
            };
        }

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
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

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
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

            var summary = Generator.PairSummary(package.ClaimType, package.ClaimValue);

            if (claimExists)
            {
                var flag = $"userId:{id},claim：{summary}";

                return StoreResult.EntityFoundFailed(nameof(IdentityUserClaim), flag);
            }

            var userClaim = Instance.CreateInstance<IdentityUserClaim, UserClaimPackage>(package);

            userClaim.UserId = id;
            userClaim.CheckStamp = Generator.CheckStamp(summary);

            return await UserClaimStore.CreateAsync(userClaim, cancellationToken);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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
                        var summery = Generator.PairSummary(package.ClaimType, package.ClaimValue);

                        var userClaim = Instance.CreateInstance<IdentityUserClaim, UserClaimPackage>(package);

                        userClaim.UserId = id;
                        userClaim.CheckStamp = Generator.CheckStamp(summery);

                        return userClaim;
                    })
                    .ToList();

                return await UserClaimStore.CreateAsync(userClaims, cancellationToken);
            }

            var flag =
                $"userId:{id},claims:{string.Join(',', packageList.Select(item => Generator.PairSummary(item.ClaimType, item.ClaimValue)))}";

            return StoreResult.EntityFoundFailed(nameof(IdentityUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     更新用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateUserClaimAsync(
        Guid id,
        int claimId,
        UserClaimPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userClaim = await UserClaimStore.KeyMatchQuery(claimId)
                .Where(userClaim => userClaim.UserId == id)
                .FirstOrDefaultAsync(cancellationToken);

            var summary = Generator.PairSummary(package.ClaimType, package.ClaimValue);

            if (userClaim is not null)
            {
                userClaim.ClaimType = package.ClaimType;
                userClaim.ClaimValue = package.ClaimValue;
                userClaim.CheckStamp = Generator.CheckStamp(summary);

                return await UserClaimStore.UpdateAsync(userClaim, cancellationToken);
            }

            var flag = $"userId:{id},claim：{summary}";

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     更新用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="dictionary">凭据更新字典</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateUserClaimsAsync(
        Guid id,
        IDictionary<int, UserClaimPackage> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var ids = dictionary.Keys;

            var userClaims = await UserClaimStore.KeyMatchQuery(ids)
                .Where(userClaim => userClaim.UserId == id)
                .ToListAsync(cancellationToken);

            if (userClaims.Any())
            {
                userClaims = userClaims.Select(userClaim =>
                {
                    var package = dictionary[userClaim.Id];

                    var summary = Generator.PairSummary(package.ClaimType, package.ClaimValue);

                    userClaim.ClaimType = package.ClaimType;
                    userClaim.ClaimValue = package.ClaimValue;
                    userClaim.CheckStamp = Generator.CheckStamp(summary);

                    return userClaim;
                }).ToList();

                return await UserClaimStore.UpdateAsync(userClaims, cancellationToken);
            }

            var flag = $"userId:{id},claims:{string.Join(',', ids)}";

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserClaim), claimId.ToString());
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserClaim), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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
                .SelectMany(artemisUser => artemisUser.UserLogins!);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                loginProviderSearch != string.Empty,
                userLogin => EF.Functions.Like(
                    userLogin.LoginProvider,
                    $"%{loginProviderSearch}%"));

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(userLogin => userLogin.LoginProvider).ThenBy(userLogin => userLogin.ProviderKey);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var userLogins = await query.ProjectToType<UserLoginInfo>().ToListAsync(cancellationToken);

            return new PageResult<UserLoginInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = userLogins
            };
        }

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="provider">登录信息标识</param>
    /// <param name="providerKey">登录信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    public async Task<UserLoginInfo?> GetUserLoginAsync(
        Guid id,
        string provider,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
            return await UserLoginStore.EntityQuery
                .Where(login => login.LoginProvider == provider)
                .Where(login => login.ProviderKey == providerKey)
                .ProjectToType<UserLoginInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     添加或更新用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加或更新结果</returns>
    public async Task<StoreResult> AddOrUpdateUserLoginAsync(
        Guid id,
        UserLoginPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var login = await UserLoginStore.EntityQuery
            .Where(login => login.UserId == id)
            .Where(login => login.LoginProvider == package.LoginProvider)
            .Where(login => login.ProviderKey == package.ProviderKey)
            .FirstOrDefaultAsync(cancellationToken);

        if (login == null)
        {
            login = Instance.CreateInstance<IdentityUserLogin, UserLoginPackage>(package);

            login.UserId = id;

            return await UserLoginStore.CreateAsync(login, cancellationToken);
        }

        if (login.ProviderDisplayName == package.ProviderDisplayName) return StoreResult.Success(0);

        login.ProviderDisplayName = package.ProviderDisplayName;

        return await UserLoginStore.UpdateAsync(login, cancellationToken);
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="provider">登录信息标识</param>
    /// <param name="providerKey">登录信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserLoginAsync(
        Guid id,
        string provider,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userLogin = await UserLoginStore.EntityQuery
                .Where(login => login.LoginProvider == provider)
                .Where(login => login.ProviderKey == providerKey)
                .FirstOrDefaultAsync(cancellationToken);

            if (userLogin is not null) return await UserLoginStore.DeleteAsync(userLogin, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserLogin), $"{provider}:{providerKey}");
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="providerAndKeys">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserLoginsAsync(
        Guid id,
        IEnumerable<KeyValuePair<string, string>> providerAndKeys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var query = UserLoginStore.EntityQuery
                .Where(userLogin => userLogin.UserId == id);

            var init = UserLoginStore.EntityQuery
                .Where(userLogin => userLogin.UserId == id);

            var keyValuePairs = providerAndKeys.ToList();

            foreach (var (provider, providerKey) in keyValuePairs)
            {
                var segment = init
                    .Where(userLogin => userLogin.LoginProvider == provider)
                    .Where(userLogin => userLogin.ProviderKey == providerKey);

                query = query.Union(segment);
            }

            var userLogins = await query.ToListAsync(cancellationToken);

            if (userLogins.Any())
                return await UserLoginStore.DeleteAsync(userLogins, cancellationToken);

            var flag = string.Join(',', keyValuePairs.Select(item => Generator.PairSummary(item.Key, item.Value)));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserLogin), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
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
                .SelectMany(artemisUser => artemisUser.UserTokens!);

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

            query = query
                .OrderBy(userToken => userToken.LoginProvider)
                .ThenBy(userToken => userToken.Name);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var userTokens = await query
                .ProjectToType<UserTokenInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<UserTokenInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = userTokens
            };
        }

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProvider">登录提供程序</param>
    /// <param name="name">登录提供程序</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    public async Task<UserTokenInfo?> GetUserTokenAsync(
        Guid id,
        string loginProvider,
        string name,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
            return await UserTokenStore.EntityQuery
                .Where(token => token.LoginProvider == loginProvider)
                .Where(token => token.Name == name)
                .ProjectToType<UserTokenInfo>()
                .FirstOrDefaultAsync(cancellationToken);

        throw new EntityNotFoundException(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     添加或更新用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">令牌信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加或更新结果</returns>
    public async Task<StoreResult> AddOrUpdateUserTokenAsync(
        Guid id,
        UserTokenPackage package,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var token = await UserTokenStore.EntityQuery
            .Where(userToken => userToken.UserId == id)
            .Where(userToken => userToken.LoginProvider == package.LoginProvider)
            .Where(userToken => userToken.Name == package.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (token == null)
        {
            token = Instance.CreateInstance<IdentityUserToken, UserTokenPackage>(package);

            token.UserId = id;

            return await UserTokenStore.CreateAsync(token, cancellationToken);
        }

        if (token.Value == package.Value) return StoreResult.Success(0);

        token.Value = package.Value;

        return await UserTokenStore.UpdateAsync(token, cancellationToken);
    }

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProvider">登录提供程序</param>
    /// <param name="name">令牌名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserTokenAsync(
        Guid id,
        string loginProvider,
        string name,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var userToken = await UserTokenStore.EntityQuery
                .Where(userToken => userToken.UserId == id)
                .Where(userToken => userToken.LoginProvider == loginProvider)
                .Where(userToken => userToken.Name == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (userToken is not null) return await UserTokenStore.DeleteAsync(userToken, cancellationToken);

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserToken), $"{id}:{loginProvider}:{name}");
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="providerAndKeys">令牌标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    public async Task<StoreResult> RemoveUserTokensAsync(
        Guid id,
        IEnumerable<KeyValuePair<string, string>> providerAndKeys,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = id != default && await UserStore.ExistsAsync(id, cancellationToken);

        if (userExists)
        {
            var query = UserTokenStore.EntityQuery
                .Where(userLogin => userLogin.UserId == id);

            var init = UserTokenStore.EntityQuery
                .Where(userLogin => userLogin.UserId == id);

            var keyValuePairs = providerAndKeys.ToList();

            foreach (var (provider, name) in keyValuePairs)
            {
                var segment = init
                    .Where(userLogin => userLogin.LoginProvider == provider)
                    .Where(userLogin => userLogin.Name == name);

                query = query.Union(segment);
            }

            var userTokens = await query.ToListAsync(cancellationToken);

            if (userTokens.Any()) return await UserTokenStore.DeleteAsync(userTokens, cancellationToken);

            var flag = string.Join(',', keyValuePairs.Select(item => $"{id}:{item.Key}:{item.Value}"));

            return StoreResult.EntityNotFoundFailed(nameof(IdentityUserToken), flag);
        }

        return StoreResult.EntityNotFoundFailed(nameof(IdentityUser), id.GuidToString());
    }

    #endregion
}