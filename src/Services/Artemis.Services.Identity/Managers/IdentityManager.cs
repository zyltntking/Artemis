﻿using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Transfer;
using Artemis.Shared.Identity.Transfer.Base;
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
        IDistributedCache? cache = null) : base(userStore, cache, optionsAccessor, logger)
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

    #endregion

    #region Implementation of IIdentityManager

    /// <summary>
    ///     测试
    /// </summary>
    public void Test()
    {
        var role = new ArtemisRole
        {
            Name = "test",
            NormalizedName = "test",
            Description = "test",
            Id = default
        };

        SetKey("keyName", Guid.NewGuid());
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
        return RoleStore.FindMapEntityAsync<RoleInfo>(id, cancellationToken);
    }

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    public async Task<StoreResult> CreateRoleAsync(
        RoleBase pack,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = NormalizeKey(pack.Name);

        var exists = await RoleStore.EntityQuery
            .AnyAsync(role => role.NormalizedName == normalizedName, cancellationToken);

        if (exists)
            return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisRole), pack.Name));

        var role = Instance.CreateInstance<ArtemisRole>();

        pack.Adapt(role);

        role.NormalizedName = normalizedName;

        return await RoleStore.CreateAsync(role, cancellationToken);
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

        if (role is not null)
        {
            pack.Adapt(role);

            role.NormalizedName = NormalizeKey(pack.Name);

            return await RoleStore.UpdateAsync(role, cancellationToken);
        }

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));
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
        var exists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists) return await UpdateRoleAsync(id, pack, cancellationToken);

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
        var role = await RoleStore.FindEntityAsync(id, cancellationToken);

        if (role != null)
            return await RoleStore.DeleteAsync(role, cancellationToken);

        return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));
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
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<RoleClaimInfo?> GetRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default)
    {
        var exists = id != default && await RoleStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            var claim = await RoleClaimStore.FindMapEntityAsync<RoleClaimInfo>(claimId, cancellationToken);

            return claim;
        }

        throw new EntityNotFoundException(nameof(ArtemisRole), id.ToString());
    }

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateRoleClaimAsync(
        Guid id,
        RoleClaimBase pack,
        CancellationToken cancellationToken = default)
    {
        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));

        var claimExists = await RoleClaimStore.EntityQuery
            .Where(claim => claim.RoleId == id)
            .Where(claim => claim.CheckStamp == pack.GenerateCheckStamp)
            .AnyAsync(cancellationToken);

        if (claimExists)
            return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisRoleClaim), pack.GenerateFlag));

        var claim = Instance.CreateInstance<ArtemisRoleClaim>();

        pack.Adapt(claim);
        claim.RoleId = id;

        return await RoleClaimStore.CreateAsync(claim, cancellationToken);
    }

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> UpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimBase pack,
        CancellationToken cancellationToken = default)
    {
        var roleExists = await RoleStore.ExistsAsync(id, cancellationToken);

        if (!roleExists)
            return StoreResult.Failed(Describer.EntityNotFound(nameof(ArtemisRole), id.ToString()));

        var hasBeenSet = await RoleClaimStore.EntityQuery
            .Where(claim => claim.Id != claimId)
            .Where(claim => claim.CheckStamp != pack.CheckStamp)
            .AnyAsync(cancellationToken);

        if (hasBeenSet)
            return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisRoleClaim), pack.GenerateFlag));

        var roleClaim = await RoleClaimStore.FindEntityAsync(claimId, cancellationToken);

        if (roleClaim is null)
            return StoreResult.Failed(Describer.EntityNotFound(
                nameof(ArtemisRoleClaim), claimId.ToString()));

        pack.Adapt(roleClaim);

        return await RoleClaimStore.UpdateAsync(roleClaim, cancellationToken);
    }

    /// <summary>
    ///     创建或更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateOrUpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimBase pack,
        CancellationToken cancellationToken = default)
    {
        var exists = await RoleClaimStore.EntityQuery
            .Where(claim => claim.RoleId == id)
            .Where(claim => claim.Id == claimId)
            .AnyAsync(cancellationToken);

        if (exists) return await UpdateRoleClaimAsync(id, claimId, pack, cancellationToken);

        return await CreateRoleClaimAsync(id, pack, cancellationToken);
    }

    #endregion
}