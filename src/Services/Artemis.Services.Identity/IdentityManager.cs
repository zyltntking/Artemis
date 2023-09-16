using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Models;
using Mapster;
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
    /// <param name="userRoleStore">用户管理器</param>
    /// <param name="identityUserStore">用户存储</param>
    /// <param name="roleManager">角色管理器</param>
    /// <param name="identityRoleStore">角色存储</param>
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
        IArtemisUserRoleStore userRoleStore,
        UserManager<ArtemisUser> userManager,
        IUserStore<ArtemisUser> identityUserStore,
        RoleManager<ArtemisRole> roleManager,
        IRoleStore<ArtemisRole> identityRoleStore,
        ILogger<IManager<ArtemisUser>> logger) : base(userStore, null, null, logger)
    {
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        UserLoginStore = userLoginStore;
        UserTokenStore = userTokenStore;
        RoleStore = roleStore;
        RoleClaimStore = roleClaimStore;
        UserRoleStore = userRoleStore;
        UserManager = userManager;
        IdentityUserStore = identityUserStore;
        RoleManager = roleManager;
        IdentityRoleStore = identityRoleStore;
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
        UserRoleStore.Dispose();
        UserManager.Dispose();
        IdentityUserStore.Dispose();
        RoleManager.Dispose();
        IdentityRoleStore.Dispose();
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
    ///     用户角色映射存储访问器
    /// </summary>
    private IArtemisUserRoleStore UserRoleStore { get; }

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

    /// <summary>
    ///     角色存储
    /// </summary>
    private IRoleStore<ArtemisRole> IdentityRoleStore { get; }

    #endregion

    #region Implementation of IIdentityManager

    /// <summary>
    ///     测试
    /// </summary>
    public async Task<string> Test()
    {
        var res = await RoleManager
            .Roles
            .Include(item => item.Users)
            .Select(item => item.Users)
            .FirstOrDefaultAsync();

        return "success";
    }

    /// <summary>
    ///     根据角色名获取角色
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <returns></returns>
    public async Task<Role?> GetRoleAsync(string roleName)
    {
        Logger.LogDebug($"查询角色：{roleName}");

        var role = await RoleManager.FindByNameAsync(roleName);

        return role?.Adapt<ArtemisRole>();
    }

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        var roles = await RoleManager.Roles.AsNoTracking().ToListAsync();

        return roles.Adapt<List<Role>>();
    }

    /// <summary>
    ///     根据角色名搜索角色列表
    /// </summary>
    /// <param name="roleNameSearch">搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <returns></returns>
    public async Task<PageResult<Role>> GetRolesAsync(string roleNameSearch, int page = 1, int size = 20)
    {
        Logger.LogDebug($"查询角色：{roleNameSearch}，页码：{page}，页面大小：{size}");

        var normalizedSearch = RoleManager.NormalizeKey(roleNameSearch);

        var total = await RoleManager.Roles.LongCountAsync();

        long count;

        var query = RoleManager.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(x => x.NormalizedName!.Contains(normalizedSearch));

            count = await query.LongCountAsync();
        }
        else
        {
            count = total;
        }

        var roles = await query.Skip((page - 1) * size).Take(size).ToListAsync();

        return new PageResult<Role>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Data = roles.Adapt<List<Role>>()
        };
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="roleName">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(IdentityResult result, Role? role)> CreateRoleAsync(string roleName,
        CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"创建角色：{roleName}");

        var role = Instance.CreateInstance<ArtemisRole>();

        role.Name = roleName;
        role.NormalizedName = RoleManager.NormalizeKey(roleName);

        var result = await RoleManager.CreateAsync(role);

        return (result, role.Adapt<Role>());
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<(IdentityResult result, Role role)> UpdateRoleAsync(Role role,
        CancellationToken cancellationToken)
    {
        Logger.LogDebug($"更新角色：{role.Id}");

        var artemisRole = await RoleStore.FindEntityAsync(role.Id, cancellationToken);

        if (artemisRole != null)
        {
            artemisRole.Name = role.Name;
            artemisRole.NormalizedName = RoleManager.NormalizeKey(role.Name);

            var result = await RoleManager.UpdateAsync(artemisRole);

            return (result, artemisRole.Adapt<Role>());
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), role.Id.ToString());
    }

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<IdentityResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"删除角色：{roleId}");

        var artemisRole = await RoleStore.FindEntityAsync(roleId, cancellationToken);

        if (artemisRole != null) return await RoleManager.DeleteAsync(artemisRole);

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
    ///     获取角色用户
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="usernameSearch">用户名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <returns></returns>
    public async Task<PageResult<User>> GetRoleUsersAsync(Guid roleId, string usernameSearch, int page = 1,
        int size = 20)
    {
        Logger.LogDebug($"获取角色用户：{roleId}，搜索值：{usernameSearch}，页码：{page}，页面大小：{size}");

        var normalizedSearch = UserManager.NormalizeName(usernameSearch);

        var total = await UserRoleStore.EntityQuery.Where(entity => entity.RoleId == roleId).LongCountAsync();


        throw new NotImplementedException();
    }

    #endregion
}