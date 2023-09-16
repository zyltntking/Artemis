using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity;

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
    /// <param name="identityUserStore">用户存储</param>
    /// <param name="roleManager">角色管理器</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userLoginStore"></param>
    /// <param name="userManager"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityManager(
        IArtemisUserStore userStore,
        IArtemisUserClaimStore userClaimStore,
        IArtemisUserLoginStore userLoginStore,
        IArtemisUserTokenStore userTokenStore,
        IArtemisRoleStore roleStore,
        IArtemisRoleClaimStore roleClaimStore,
        UserManager<ArtemisUser> userManager,
        IUserStore<ArtemisUser> identityUserStore,
        RoleManager<ArtemisRole> roleManager,
        ILogger<IManager<ArtemisUser>> logger) : base(userStore, null, null, logger)
    {
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        RoleStore = roleStore;
        RoleClaimStore = roleClaimStore;
        UserManager = userManager;
        IdentityUserStore = identityUserStore;
        RoleManager = roleManager;
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
        UserManager.Dispose();
        IdentityUserStore.Dispose();
        RoleManager.Dispose();
        base.StoreDispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore { get; }

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
    ///     用户管理器
    /// </summary>
    private UserManager<ArtemisUser> UserManager { get; }

    /// <summary>
    ///     用户存储
    /// </summary>
    private IUserStore<ArtemisUser> IdentityUserStore { get; }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private RoleManager<ArtemisRole> RoleManager { get; }

    #endregion

    #region Implementation of IIdentityManager

    /// <summary>
    ///     测试
    /// </summary>
    public async Task<string> Test()
    {
        await FetchRolesAsync();

        var res = await RoleManager
            .Roles
            .Include(item => item.Users)
            .Select(item => item.Users)
            .FirstOrDefaultAsync();

        return res == null ? "success" : res.Count.ToString();
    }

    /// <summary>
    ///     根据角色名获取角色
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <returns></returns>
    public async Task<Role?> GetRoleAsync(string roleName)
    {
        Logger.LogDebug($"查询角色：{roleName}");

        var artemisRole = await RoleManager.FindByNameAsync(roleName);

        if (artemisRole != null) 
            return artemisRole;

        throw new EntityNotFoundException(nameof(ArtemisRole), roleName);

    }

    /// <summary>
    /// 根据角色id获取角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<Role?> GetRoleAsync(Guid roleId)
    {
        Logger.LogDebug($"查询角色：{roleId}");

        var artemisRole = await RoleManager.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Id == roleId);

        if (artemisRole != null) 
            return artemisRole;

        throw new EntityNotFoundException(nameof(ArtemisRole), roleId.ToString());

    }

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        Logger.LogDebug("获取角色列表");

        var artemisRoles = await RoleManager
            .Roles.AsNoTracking()
            .OrderBy(role => role.NormalizedName)
            .ToListAsync();

        return artemisRoles;
    }

    /// <summary>
    ///     根据角色名搜索角色列表
    /// </summary>
    /// <param name="roleNameSearch">搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <returns></returns>
    public async Task<PageResult<Role>> FetchRolesAsync(string? roleNameSearch = null, int page = 1, int size = 20)
    {
        Logger.LogDebug($"查询角色：{roleNameSearch}，页码：{page}，页面大小：{size}");

        roleNameSearch ??= string.Empty;

        var query = RoleManager.Roles.AsNoTracking();

        var total = await query.LongCountAsync();

        long count;

        if (roleNameSearch == string.Empty)
        {
            var normalizedSearch = RoleManager.NormalizeKey(roleNameSearch);

            query = query.Where(artemisRole => artemisRole.NormalizedName.Contains(normalizedSearch));

            count = await query.LongCountAsync();
        }
        else
        {
            count = total;
        }

        var artemisRoles = await query
            .OrderBy(role => role.NormalizedName)
            .Skip((page - 1) * size).Take(size)
            .ToListAsync();

        return new PageResult<Role>
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
    /// <param name="roleName">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, Role? role)> CreateRoleAsync(string roleName,
        CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"创建角色：{roleName}");

        var artemisRole = Instance.CreateInstance<ArtemisRole>();

        artemisRole.Name = roleName;
        artemisRole.NormalizedName = RoleManager.NormalizeKey(roleName);

        var result = await RoleStore.CreateAsync(artemisRole, cancellationToken);

        return (result, artemisRole);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(StoreResult result, Role role)> UpdateRoleAsync(Role role,
        CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"更新角色：{role.Id}");

        var artemisRole = await RoleStore.FindEntityAsync(role.Id, cancellationToken);

        if (artemisRole != null)
        {
            artemisRole.Name = role.Name;
            artemisRole.NormalizedName = RoleManager.NormalizeKey(role.Name);
            artemisRole.Description = role.Description;

            var result = await RoleStore.UpdateAsync(artemisRole, cancellationToken);

            return (result, artemisRole);
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), role.Name);
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"删除角色：{roleId}");

        var artemisRole = await RoleStore.FindEntityAsync(roleId, cancellationToken);

        if (artemisRole != null) 
            return await RoleStore.DeleteAsync(artemisRole, cancellationToken);

        throw new EntityNotFoundException(nameof(ArtemisRole), roleId.ToString());
    }

    /// <summary>
    ///     批量删除角色
    /// </summary>
    /// <param name="roleIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> DeleteRolesAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var enumerable = roleIds.ToList();

        Logger.LogDebug($"批量删除角色：{enumerable}");

        return RoleStore.DeleteAsync(enumerable, cancellationToken);
    }

    /// <summary>
    /// 根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="userNameSearch">用户名</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<User>> FetchRoleUsersAsync(Guid roleId, string? userNameSearch = null, int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"查询角色用户，角色：{roleId}，用户名：{userNameSearch}，页码：{page}，页面大小：{size}");

        var roleExists = await RoleManager.Roles
            .AnyAsync(role => role.Id == roleId, cancellationToken);

        if (roleExists)
        {
            userNameSearch ??= string.Empty;

            var query = RoleManager.Roles.AsNoTracking()
                .Where(artemisRole => artemisRole.Id == roleId)
                .SelectMany(artemisRole => artemisRole.Users);

            var total = await query.LongCountAsync(cancellationToken);

            long count;

            if (userNameSearch == string.Empty)
            {
                var normalizedSearch = UserManager.NormalizeName(userNameSearch);

                query = query.Where(artemisUser => artemisUser.NormalizedUserName.Contains(normalizedSearch));

                count = await query.LongCountAsync(cancellationToken);
            }
            else
            {
                count = total;
            }

            var artemisUsers = await query
                .OrderBy(user => user.NormalizedUserName)
                .Skip((page - 1) * size).Take(size)
                .ToListAsync(cancellationToken);

            return new PageResult<User>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = artemisUsers
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), roleId.ToString());
    }

    /// <summary>
    /// 查询角色的声明
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(Guid roleId, string? claimTypeSearch = null, int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"查询角色凭据，角色：{roleId}，页码：{page}，页面大小：{size}");
        var roleExists = await RoleManager.Roles
            .AnyAsync(role => role.Id == roleId, cancellationToken);

        if (roleExists)
        {
            claimTypeSearch ??= string.Empty;

            var query = RoleManager.Roles.AsNoTracking()
                .Where(artemisRole => artemisRole.Id == roleId)
                .SelectMany(artemisRole => artemisRole.RoleClaims);

            var total = await query.LongCountAsync(cancellationToken);

            long count;

            if (claimTypeSearch == string.Empty)
            {
                query = query.Where(artemisClaim => artemisClaim.ClaimType.Contains(claimTypeSearch));

                count = await query.LongCountAsync(cancellationToken);
            }
            else
            {
                count = total;
            }

            var artemisRoles = await query
                .OrderBy(role => role.ClaimType)
                .Skip((page - 1) * size).Take(size)
                .ToListAsync(cancellationToken);

            return new PageResult<RoleClaim>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Data = artemisRoles
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), roleId.ToString());
    }

    #endregion
}