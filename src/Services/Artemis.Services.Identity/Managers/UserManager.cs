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
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserManager(
        IArtemisUserStore userStore,
        IArtemisRoleStore roleStore,
        IArtemisUserRoleStore userRoleStore,
        ILogger? logger = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        IDistributedCache? cache = null) : base(userStore, cache, optionsAccessor, logger)
    {
        RoleStore = roleStore;
        UserRoleStore = userRoleStore;
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

    #endregion

    #region Implementation of IUserManager

    /// <summary>
    ///     搜索用户
    /// </summary>
    /// <param name="nameSearch">用户名搜索值</param>
    /// <param name="emailSearch">邮箱搜索值</param>
    /// <param name="phoneNumberSearch">电话号码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
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
                user.NormalizedEmail,
                $"%{normalizedEmail}%"));

        query = query.WhereIf(
            phoneNumberSearch != string.Empty,
            user => EF.Functions.Like(
                user.PhoneNumber,
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
    /// <returns></returns>
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
    /// <param name="pack">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, UserInfo? user)> CreateUserAsync(
        UserPackage pack,
        string password,
        CancellationToken cancellationToken)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedUserName = NormalizeKey(pack.UserName);

        var exists = await UserStore.EntityQuery
            .AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);

        if (exists)
            return (StoreResult.EntityFoundFailed(nameof(ArtemisUser), pack.UserName), default);

        var user = Instance.CreateInstance<ArtemisUser>();

        pack.Adapt(user);

        user.NormalizedUserName = normalizedUserName;

        if (pack.Email is not null) user.NormalizedEmail = NormalizeKey(pack.Email);

        user.PasswordHash = Hash.ArtemisHash(password);

        user.SecurityStamp = pack.GenerateSecurityStamp;

        var result = await UserStore.CreateAsync(user, cancellationToken);

        return (result, user.Adapt<UserInfo>());
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="pack">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, UserInfo? user)> UpdateUserAsync(
        Guid id,
        UserPackage pack,
        string? password = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var user = await UserStore.FindEntityAsync(id, cancellationToken);

        if (user is not null)
        {
            pack.Adapt(user);

            user.NormalizedUserName = NormalizeKey(pack.UserName);

            if (pack.Email is not null) user.NormalizedEmail = NormalizeKey(pack.Email);

            if (password is not null) user.PasswordHash = Hash.ArtemisHash(password);

            user.SecurityStamp = pack.GenerateSecurityStamp;

            var result = await UserStore.UpdateAsync(user, cancellationToken);

            return (result, user.Adapt<UserInfo>());
        }

        return (StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString()), default);
    }

    /// <summary>
    ///     创建或更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="pack">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, UserInfo? user)> CreateOrUpdateUserAsync(
        Guid id,
        UserPackage pack,
        string? password = null,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = await UserStore.ExistsAsync(id, cancellationToken);

        if (exists)
            return await UpdateUserAsync(id, pack, password, cancellationToken);

        if (password is null)
            return (StoreResult.PropertyIsNullFailed(nameof(password)), default);

        return await CreateUserAsync(pack, password, cancellationToken);
    }

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
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
    ///     根据角色名搜索该用户具有的角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleNameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
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
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> AddUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (!userExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());

        var roleExists = await RoleStore.ExistsAsync(roleId, cancellationToken);

        if (!roleExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), roleId.ToString());

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

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> RemoveUserRoleAsync(Guid id, Guid roleId,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var userExists = await UserStore.ExistsAsync(id, cancellationToken);

        if (!userExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUser), id.ToString());

        var roleExists = await RoleStore.ExistsAsync(roleId, cancellationToken);

        if (!roleExists)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisRole), id.ToString());

        var userRole = await UserRoleStore.EntityQuery
            .Where(userRole => userRole.UserId == id)
            .Where(userRole => userRole.RoleId == roleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userRole == null)
            return StoreResult.EntityNotFoundFailed(nameof(ArtemisUserRole), $"userId:{id},roleId:{roleId}");

        return await UserRoleStore.DeleteAsync(userRole, cancellationToken);
    }

    #endregion
}