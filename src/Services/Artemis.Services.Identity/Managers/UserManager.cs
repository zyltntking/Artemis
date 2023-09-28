using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Kit;
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
/// 用户管理器
/// </summary>
public class UserManager : Manager<ArtemisUser>, IUserManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="optionsAccessor">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserManager(
        IArtemisUserStore store, 
        ILogger? logger = null,
        IOptions<ArtemisStoreOptions>? optionsAccessor = null,
        IDistributedCache? cache = null) : base(store, cache, optionsAccessor, logger)
    {
    }

    #region Overrides of Manager<ArtemisUser,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     角色存储访问器
    /// </summary>
    private IArtemisUserStore UserStore => (IArtemisUserStore)Store;

    #endregion

    #region Implementation of IUserManager

    /// <summary>
    /// 搜索用户
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
        ThrowIfDisposed();

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
    /// 根据用户标识获取用户
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<UserInfo?> GetUserAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        return UserStore.FindMapEntityAsync<UserInfo>(id, cancellationToken);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="pack">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<StoreResult> CreateUserAsync(
        UserBase pack, 
        string password,
        CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        var normalizedUserName = NormalizeKey(pack.UserName);

        var exists = await UserStore.EntityQuery
            .AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);

        if (exists)
            return StoreResult.Failed(Describer.EntityHasBeenSet(nameof(ArtemisUser), pack.UserName));

        var user = Instance.CreateInstance<ArtemisUser>();

        pack.Adapt(user);

        user.NormalizedUserName = normalizedUserName;

        user.NormalizedEmail = NormalizeKey(user.Email);

        user.PasswordHash = Hash.ArtemisHash(password);

        user.SecurityStamp = Base32.GenerateBase32();

        return await UserStore.CreateAsync(user, cancellationToken);
    }

    #endregion
}