using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;
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
        base.StoreDispose();
        UserClaimStore.Dispose();
        UserLoginStore.Dispose();
        UserTokenStore.Dispose();
        RoleStore.Dispose();
        RoleClaimStore.Dispose();
        UserManager.Dispose();
        IdentityUserStore.Dispose();
        RoleManager.Dispose();
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
    ///     存储错误描述器
    /// </summary>
    private IStoreErrorDescriber Describer { get; } = new StoreErrorDescriber();

    #endregion

    #region Implementation of IIdentityManager

    /// <summary>
    ///     测试
    /// </summary>
    public async Task<string> Test()
    {
        var provider = new HashProvider();

        var cc = await UpdateRoleClaimsAsync(new List<RoleClaim>
        {
            new()
            {
                Id = 1,
                RoleId = Guid.NewGuid(),
                CheckStamp = provider.Md5Hash("test"),
                ClaimType = "test",
                ClaimValue = "test",
                Description = "test"
            }
        });

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
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    public async Task<Role?> GetRoleAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{name}");

        return await RoleManager.FindByNameAsync(name);
    }

    /// <summary>
    ///     根据角色id获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    public async Task<Role?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{id}");

        return await RoleManager.Roles
            .AsNoTracking()
            .ProjectToType<Role>()
            .FirstOrDefaultAsync(role => role.Id == id, cancellationToken);
    }

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns>角色列表</returns>
    /// <remarks>当查询不到角色实例时返回空列表</remarks>
    public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        SetDebugLog("获取角色列表");

        return await RoleManager.Roles
            .AsNoTracking()
            .OrderBy(role => role.NormalizedName)
            .ProjectToType<Role>()
            .ToListAsync(cancellationToken);
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
    public async Task<PageResult<Role>> FetchRolesAsync(
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{nameSearch}，页码：{page}，页面大小：{size}");

        nameSearch ??= string.Empty;

        var query = RoleManager.Roles.AsNoTracking();

        var total = await query.LongCountAsync(cancellationToken);

        long count;

        if (nameSearch != string.Empty)
        {
            var normalizedSearch = RoleManager.NormalizeKey(nameSearch);

            query = query.Where(artemisRole => EF.Functions.Like(artemisRole.NormalizedName, $"%{normalizedSearch}%"));

            count = await query.LongCountAsync(cancellationToken);
        }
        else
        {
            count = total;
        }

        var artemisRoles = await query
            .OrderBy(role => role.NormalizedName)
            .Skip((page - 1) * size).Take(size)
            .ProjectToType<Role>()
            .ToListAsync(cancellationToken);

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
    /// <param name="name">角色名</param>
    /// <param name="description">角色描述</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    public async Task<AttachResult<StoreResult, Role>> CreateRoleAsync(
        string name,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{name}");

        var role = Instance.CreateInstance<Role>();

        role.Name = name;
        role.NormalizedName = RoleManager.NormalizeKey(name);
        role.Description = description;

        var result = await RoleStore.CreateNewAsync(role, cancellationToken: cancellationToken);

        return result.Attach(role);
    }

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleAsync(
        Role role,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{role.Id}");

        var roleExists = await RoleStore.ExistsAsync(role.Id, cancellationToken);

        if (roleExists)
        {
            role.NormalizedName = RoleManager.NormalizeKey(role.Name);

            return await RoleStore.OverAsync(role, cancellationToken: cancellationToken);
        }

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), role.Id.ToString()));
    }

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> CreateOrUpdateRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色：{role.Name}");

        role.NormalizedName = RoleManager.NormalizeKey(role.Name);

        var exists = await RoleStore.ExistsAsync(role.Id, cancellationToken);

        if (exists) return await RoleStore.OverAsync(role, cancellationToken: cancellationToken);

        return await RoleStore.CreateNewAsync(role, cancellationToken: cancellationToken);
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
        SetDebugLog($"查询角色：{id}");

        var artemisRole = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (artemisRole != null)
            return await RoleStore.DeleteAsync(artemisRole, cancellationToken);

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));
    }

    /// <summary>
    ///     批量删除角色
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<StoreResult> DeleteRolesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var roleIdList = ids.ToList();

        SetDebugLog($"批量删除角色凭据：{string.Join(",", roleIdList)}");

        return RoleStore.DeleteAsync(roleIdList, cancellationToken);
    }

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="nameSearch">用户名</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<PageResult<User>> FetchRoleUsersAsync(
        Guid id,
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色用户，角色：{id}，用户名：{nameSearch}，页码：{page}，页面大小：{size}");

        var roleExists = await RoleManager.Roles
            .AnyAsync(role => role.Id == id, cancellationToken);

        if (roleExists)
        {
            nameSearch ??= string.Empty;

            var query = RoleManager.Roles.AsNoTracking()
                .Where(artemisRole => artemisRole.Id == id)
                .SelectMany(artemisRole => artemisRole.Users);

            var total = await query.LongCountAsync(cancellationToken);

            long count;

            if (nameSearch == string.Empty)
            {
                var normalizedSearch = UserManager.NormalizeName(nameSearch);

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

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<IEnumerable<RoleClaim>> GetRoleClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog("获取角色凭据列表");

        var roleExists = await RoleManager.Roles
            .AnyAsync(role => role.Id == id, cancellationToken);

        if (roleExists)
        {
            var artemisRoleClaims = await RoleManager.Roles
                .Where(role => role.Id == id)
                .SelectMany(role => role.RoleClaims)
                .OrderBy(roleClaim => roleClaim.ClaimType)
                .ToListAsync(cancellationToken);

            return artemisRoleClaims;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     查询角色的声明
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"查询角色凭据，角色：{id}，页码：{page}，页面大小：{size}");

        var roleExists = await RoleManager.Roles
            .AnyAsync(role => role.Id == id, cancellationToken);

        if (roleExists)
        {
            claimTypeSearch ??= string.Empty;

            var query = RoleManager.Roles.AsNoTracking()
                .Where(artemisRole => artemisRole.Id == id)
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

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaim">凭据</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateRoleClaimAsync(
        RoleClaim roleClaim,
        CancellationToken cancellationToken)
    {
        SetDebugLog($"创建角色凭据：roleId: {roleClaim.RoleId}, stamp: {roleClaim.CheckStamp}");

        var exists = await RoleClaimStore.EntityQuery
            .AnyAsync(claim => claim.RoleId == roleClaim.RoleId &&
                               claim.CheckStamp == roleClaim.CheckStamp,
                cancellationToken);

        if (!exists) return await RoleClaimStore.CreateNewAsync(roleClaim, cancellationToken: cancellationToken);

        return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisRole), roleClaim.CheckStamp));
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<AttachResult<StoreResult, IEnumerable<RoleClaim>>> CreateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default)
    {
        var source = roleClaims.ToList();

        SetDebugLog($"创建角色凭据：{string.Join(",", source.Select(item => $"{item.RoleId}:{item.CheckStamp}"))}");

        var checkList = source.Select(claim => claim.CheckStamp);

        var sourceInDb = RoleClaimStore.EntityQuery
            .Where(claim => checkList.Contains(claim.CheckStamp))
            .Select(claim => new { claim.RoleId, claim.CheckStamp })
            .AsEnumerable()
            .Join(source,
                claimInDb => new ClaimKey(claimInDb.RoleId, claimInDb.CheckStamp),
                claimToAdd => new ClaimKey(claimToAdd.RoleId, claimToAdd.CheckStamp),
                (_, claimsToAdd) => claimsToAdd,
                new ClaimKeyEqualityComparer())
            .ToList();

        var sourceToAdd = source.Except(sourceInDb).ToList();

        var result = await RoleClaimStore.CreateNewAsync(sourceToAdd, cancellationToken: cancellationToken);

        return result.Attach(sourceToAdd.AsEnumerable());
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleClaimAsync(RoleClaim roleClaim,
        CancellationToken cancellationToken = default)
    {
        SetDebugLog($"更新角色凭据：roleId: {roleClaim.RoleId}, stamp: {roleClaim.CheckStamp}");

        var exists = await RoleClaimStore.ExistsAsync(roleClaim.Id, cancellationToken);

        if (exists) return await RoleClaimStore.OverAsync(roleClaim, cancellationToken: cancellationToken);

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), roleClaim.CheckStamp));
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<AttachResult<StoreResult, IEnumerable<RoleClaim>>> UpdateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default)
    {
        var source = roleClaims.ToList();

        SetDebugLog($"创建角色凭据：{string.Join(",", source.Select(item => $"{item.RoleId}:{item.CheckStamp}"))}");

        var idList = source.Select(claim => claim.Id);

        var checkList = source.Select(claim => claim.CheckStamp);

        var sourceInDb = RoleClaimStore.EntityQuery
            .Where(claim => idList.Contains(claim.Id) || checkList.Contains(claim.CheckStamp))
            .Select(claim => new { claim.Id, claim.RoleId, claim.CheckStamp })
            .AsEnumerable()
            .ToList();

        var sourceToCheck = sourceInDb.Join(source,
                claimInDb => claimInDb.Id,
                claimToUpdate => claimToUpdate.Id,
                (_, claimToUpdate) => claimToUpdate)
            .ToList();

        var sourceToUpdate = new List<RoleClaim>();

        foreach (var checkItem in sourceToCheck)
        {
            var exists = sourceInDb
                .Where(item => item.Id != checkItem.Id)
                .Where(item => item.RoleId == checkItem.RoleId)
                .Any(item => item.CheckStamp == checkItem.CheckStamp);

            if (!exists) sourceToUpdate.Add(checkItem);
        }

        var result = await RoleClaimStore.OverAsync(sourceToUpdate, cancellationToken: cancellationToken);

        return result.Attach(sourceToUpdate.AsEnumerable());
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimId">角色凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteRoleClaimAsync(int claimId, CancellationToken cancellationToken)
    {
        SetDebugLog($"查询角色：{claimId}");

        return RoleClaimStore.DeleteAsync(claimId, cancellationToken);
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="checkStamp">校验戳</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> DeleteRoleClaimAsync(Guid roleId, string checkStamp,
        CancellationToken cancellationToken)
    {
        SetDebugLog($"删除角色凭据：roleId: {roleId}, stamp: {checkStamp}");

        var roleClaim = await RoleClaimStore.EntityQuery
            .Where(claim => claim.RoleId == roleId)
            .FirstOrDefaultAsync(claim => claim.CheckStamp == checkStamp, cancellationToken);

        if (roleClaim != null) return await RoleClaimStore.DeleteAsync(roleClaim, cancellationToken);

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRoleClaim), $"{roleId:N}:{checkStamp}"));
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimIds">凭据标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteRoleClaimsAsync(IEnumerable<int> claimIds, CancellationToken cancellationToken)
    {
        var list = claimIds.ToList();

        SetDebugLog($"批量删除角色凭据：{string.Join(",", list)}");

        return RoleClaimStore.DeleteAsync(list, cancellationToken);
    }

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimKeys">角色键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<StoreResult> DeleteRoleClaimsAsync(IEnumerable<ClaimKey> claimKeys,
        CancellationToken cancellationToken)
    {
        var list = claimKeys.ToList();

        SetDebugLog($"批量删除角色凭据：{string.Join(",", list.Select(item => $"{item.OuterId:N}:{item.CheckStamp}"))}");

        var idList = new List<Guid>();

        var checkList = new List<string>();

        foreach (var claimKey in list)
        {
            idList.Add(claimKey.OuterId);
            checkList.Add(claimKey.CheckStamp);
        }

        var sourceToDelete = RoleClaimStore.EntityQuery
            .Where(claim => idList.Contains(claim.RoleId))
            .Where(claim => checkList.Contains(claim.CheckStamp))
            .Select(claim => new { claim.Id, claim.RoleId, claim.CheckStamp })
            .AsEnumerable()
            .Join(list,
                claimInDb => new ClaimKey(claimInDb.RoleId, claimInDb.CheckStamp),
                claimToDelete => new ClaimKey(claimToDelete.OuterId, claimToDelete.CheckStamp),
                (claimInDb, _) => claimInDb.Id,
                new ClaimKeyEqualityComparer())
            .ToList();

        return RoleClaimStore.DeleteAsync(sourceToDelete, cancellationToken);
    }

    #endregion
}