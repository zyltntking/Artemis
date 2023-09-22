using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     Artemis用户管理器
/// </summary>
public class IdentityManager : Manager<ArtemisUser>, IIdentityManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">用户存储访问器</param>
    /// <param name="userClaimStore">用户凭据存储访问器</param>
    /// <param name="userTokenStore"></param>
    /// <param name="roleStore">角色存储访问器</param>
    /// <param name="roleClaimStore">角色凭据存储访问器</param>
    /// <param name="optionsAccessor"></param>
    /// <param name="cache">缓存以来</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userLoginStore">用户登录存储访问器</param>
    public IdentityManager(
        IArtemisUserStore userStore,
        IArtemisUserClaimStore userClaimStore,
        IArtemisUserLoginStore userLoginStore,
        IArtemisUserTokenStore userTokenStore,
        IArtemisRoleStore roleStore,
        IArtemisRoleClaimStore roleClaimStore,
        ILogger<IdentityManager> logger,
        IOptions<StoreOptions>? optionsAccessor = null,
        IDistributedCache? cache = null) : base(userStore, cache, optionsAccessor, null, logger)
    {
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        RoleStore = roleStore;
        RoleClaimStore = roleClaimStore;
    }

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
        RoleStore.Dispose();
        RoleClaimStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore => (IArtemisUserStore)Store;

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

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IArtemisRoleStore RoleStore { get; }

    /// <summary>
    ///     角色凭据存储访问器
    /// </summary>
    private IArtemisRoleClaimStore RoleClaimStore { get; }

    /// <summary>
    ///     存储错误描述器
    /// </summary>
    private IStoreErrorDescriber Describer { get; } = new StoreErrorDescriber();

    #endregion

    /// <summary>
    ///    缓存角色名键
    /// </summary>
    /// <param name="roleName">角色名称</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private Task CacheRoleIdAsync(string roleName, Guid roleId, CancellationToken cancellationToken = default)
    {
        if (CacheAvailable)
        {
            var key = GenerateRoleNameKey(roleName);

            return SetKeyAsync(key, roleId, cancellationToken)!;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 查询角色id
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task<Guid> FetchRoleIdAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var key = GenerateRoleNameKey(roleName);

        Guid id = default;

        if (CacheAvailable)
        {
            id = await GetKeyAsync(key, cancellationToken);
        }

        if (id == default)
        {
            var normalizedName = NormalizeKey(roleName);

            id = RoleStore.EntityQuery
                .Where(role => role.NormalizedName == normalizedName)
                .Select(role => role.Id)
                .FirstOrDefault();

            if (CacheAvailable)
            {
                await SetKeyAsync(key, id, cancellationToken)!;
            }
        }

        return id;
    }

    /// <summary>
    /// 删除角色名键
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task DeleteRolIdAsync(string roleName, CancellationToken cancellationToken = default)
    {
        if (CacheAvailable)
        {
            var key = GenerateRoleNameKey(roleName);

            await RemoveKeyAsync(key, cancellationToken)!;
        }
    }

    /// <summary>
    /// 生成角色名键
    /// </summary>
    /// <param name="roleName">角色名称</param>
    /// <returns></returns>
    private string GenerateRoleNameKey(string roleName)
    {
        var normalizedName = NormalizeKey(roleName);

        return GenerateKey(KeyPrefix, "Role", "Name", normalizedName);
    }

    #region Implementation of IIdentityManager

    /// <summary>
    ///     测试
    /// </summary>
    public void Test()
    {
        SetKey("keyName", Guid.NewGuid());
    }

    /// <summary>
    ///     根据角色名获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    public async Task<RoleInfo?> GetRoleAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var id = await FetchRoleIdAsync(name, cancellationToken);

        if (id == default)
        {
            return default;
        }

        return await GetRoleAsync(id, cancellationToken);
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
        return RoleStore.FindMapEntityAsync<RoleInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns>角色列表</returns>
    /// <remarks>当查询不到角色实例时返回空列表</remarks>
    public Task<IEnumerable<RoleInfo>> GetRolesAsync(
        CancellationToken cancellationToken = default)
    {
        return RoleStore.GetRolesAsync<RoleInfo>(cancellationToken);
    }

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
        nameSearch ??= string.Empty;

        var query = RoleStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        var normalizedSearch = NormalizeKey(nameSearch);

        query = query.WhereIf(
            nameSearch != string.Empty,
            role => EF.Functions.Like(
                role.NormalizedName,
                $"%{normalizedSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        var artemisRoles = await query
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
            Data = artemisRoles
        };
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="description">角色描述</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    public async Task<StoreResult> CreateRoleAsync(
        string name,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await RoleStore.ExistsAsync(name, cancellationToken);

        if (exists) return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisRole), name));

        var role = Instance.CreateInstance<ArtemisRole>();

        var normalizedName = NormalizeKey(name);

        role.Name = name;
        role.NormalizedName = normalizedName;
        role.Description = description;

        var result = await RoleStore.CreateAsync(role, cancellationToken);

        await CacheRoleIdAsync(name, role.Id, cancellationToken);

        return result;
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleAsync(
        string name,
        RoleBase pack,
        CancellationToken cancellationToken = default)
    {
        var id = await FetchRoleIdAsync(name, cancellationToken);

        return await UpdateRoleAsync(id, pack, cancellationToken);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleAsync(
        Guid id,
        RoleBase pack,
        CancellationToken cancellationToken = default)
    {
        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        var cacheUpdate = role?.Name != pack.Name;

        if (cacheUpdate)
        {
            await DeleteRolIdAsync(role?.Name!, cancellationToken);
        }

        if (role is not null)
        {
            role.Name = pack.Name;
            role.NormalizedName = NormalizeKey(pack.Name);
            role.Description = pack.Description;

            var result = await RoleStore.UpdateAsync(role, cancellationToken: cancellationToken);

            if (result.Succeeded)
            {
                if (cacheUpdate)
                {
                    await CacheRoleIdAsync(role.Name, role.Id, cancellationToken);
                }
            }

            return result;
        }

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateOrUpdateRoleAsync(
        string name,
        RoleBase pack,
        CancellationToken cancellationToken = default)
    {
        var id = await FetchRoleIdAsync(name, cancellationToken);

        return await CreateOrUpdateRoleAsync(id, pack, cancellationToken);
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateOrUpdateRoleAsync(
        Guid id,
        RoleBase pack,
        CancellationToken cancellationToken = default)
    {
        if (id != default)
        {
            return await UpdateRoleAsync(id, pack, cancellationToken);
        }

        return await CreateRoleAsync(pack.Name, pack.Description, cancellationToken);
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteRoleAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var id = await FetchRoleIdAsync(name, cancellationToken);

        return await DeleteRoleAsync(id, cancellationToken);
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
        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role != null)
            return await RoleStore.DeleteAsync(role, cancellationToken);

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));
    }

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        string name,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog(
            $"查询角色用户，角色：{name}，用户名称搜索值：{userNameSearch}，用户邮箱搜索值：{emailSearch}，用户手机号码搜索值：{phoneSearch}，页码：{page}，页面大小：{size}");

        var normalizedName = NormalizeKey(name);

        var exists = name != string.Empty && await RoleStore.ExistsAsync(normalizedName, cancellationToken);

        if (exists)
        {
            userNameSearch ??= string.Empty;
            emailSearch ??= string.Empty;
            phoneSearch ??= string.Empty;

            var query = RoleStore
                .NameMatchQuery(normalizedName)
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

        throw new EntityNotFoundException(nameof(ArtemisRole), name);
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
        SetDebugLog(
            $"查询角色用户，角色：{id:D}，用户名称搜索值：{userNameSearch}，用户邮箱搜索值：{emailSearch}，用户手机号码搜索值：{phoneSearch}，页码：{page}，页面大小：{size}");

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
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<IEnumerable<RoleClaimInfo>> GetRoleClaimsAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"获取角色凭据列表, 角色名: {name}");

        var normalizedName = NormalizeKey(name);

        var exists = name != string.Empty && await RoleStore.ExistsAsync(normalizedName, cancellationToken);

        if (exists)
        {
            var claims = await RoleStore
                .NameMatchQuery(normalizedName)
                .SelectMany(role => role.RoleClaims)
                .OrderBy(claim => claim.CreatedAt)
                .ProjectToType<RoleClaimInfo>()
                .ToListAsync(cancellationToken);

            return claims;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), name);
    }

    /// <summary>
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<IEnumerable<RoleClaimInfo>> GetRoleClaimsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"获取角色凭据列表, 角色标识: {id:D}");

        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            var claims = await RoleStore
                .KeyMatchQuery(id)
                .SelectMany(role => role.RoleClaims)
                .OrderBy(claim => claim.CreatedAt)
                .ProjectToType<RoleClaimInfo>()
                .ToListAsync(cancellationToken);

            return claims;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="name">角色名称</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<RoleClaimInfo>> FetchRoleClaimsAsync(
        string name,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色凭据，角色：{name}，页码：{page}，页面大小：{size}");

        var exists = name != string.Empty && await RoleStore.ExistsAsync(name, cancellationToken);

        if (exists)
        {
            claimTypeSearch ??= string.Empty;

            var query = RoleStore
                .NameMatchQuery(name)
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

        throw new EntityNotFoundException(nameof(ArtemisRole), name);
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
        SetDebugLog($"查询角色凭据，角色：{id}，页码：{page}，页面大小：{size}");

        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
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
    /// <param name="name">角色名</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<RoleClaimInfo?> GetRoleClaimAsync(
        string name,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"获取角色凭据，角色：{name}，凭据标识：{claimId}");

        var normalizedName = NormalizeKey(name);

        var exists = name != string.Empty && await RoleStore.ExistsAsync(normalizedName, cancellationToken);

        if (exists)
        {
            var claim = await RoleClaimStore.FindMapEntityAsync<RoleClaimInfo>(claimId, cancellationToken);

            return claim;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), name);
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
        SetDebugLog($"获取角色凭据，角色：{id:D}，凭据标识：{claimId}");

        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            var claim = await RoleClaimStore.FindMapEntityAsync<RoleClaimInfo>(claimId, cancellationToken);

            return claim;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    #endregion
}