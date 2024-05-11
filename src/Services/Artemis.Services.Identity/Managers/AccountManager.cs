using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Services.Identity.Data;
using Artemis.Services.Identity.Stores;
using Artemis.Shared.Identity.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     账号管理器
/// </summary>
public class AccountManager : KeyWithManager<ArtemisUser>, IAccountManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="store">存储访问器依赖</param>
    /// <param name="userClaimStore">用户凭据存储依赖</param>
    /// <param name="roleClaimStore">角色凭据存储依赖</param>
    /// <param name="cache">缓存管理器</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AccountManager(
        IArtemisUserStore store,
        IArtemisUserClaimStore userClaimStore,
        IArtemisRoleClaimStore roleClaimStore,
        IDistributedCache? cache = null,
        IKeyWithStoreManagerOptions? options = null,
        ILogger? logger = null) : base(store, cache, options, null, logger)
    {
        UserClaimStore = userClaimStore;
        RoleClaimStore = roleClaimStore;
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
    ///     用户存储访问器
    /// </summary>
    private IArtemisUserStore UserStore => (IArtemisUserStore)Store;

    /// <summary>
    ///     用户凭据存储依赖
    /// </summary>
    private IArtemisUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     角色凭据存储依赖
    /// </summary>
    private IArtemisRoleClaimStore RoleClaimStore { get; }

    #endregion

    #region Implementation of IAccountManager

    /// <summary>
    ///     签到/登录
    /// </summary>
    /// <param name="userSign">用户签名</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>登录结果和登录后的Token信息</returns>
    public async Task<(SignResult result, TokenDocument? token)> SignInAsync(
        string userSign,
        string password,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizeSign = UserStore.NormalizeKey(userSign);

        var userNameQuery = UserStore.EntityQuery
            .Where(item => item.NormalizedUserName == normalizeSign);

        var emailQuery = UserStore.EntityQuery
            .Where(item => item.NormalizedEmail == normalizeSign);

        var phoneNumberQuery = UserStore.EntityQuery
            .Where(item => item.NormalizedPhoneNumber == normalizeSign);

        var userDocument = await UserStore.EntityQuery
            .Union(userNameQuery)
            .Union(emailQuery)
            .Union(phoneNumberQuery)
            .ProjectToType<UserDocument>()
            .FirstOrDefaultAsync(cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "认证失败"
        };

        if (userDocument is null)
        {
            result.Message = "未找到匹配签名的用户，请检查输入";

            return (result, default);
        }

        var passwordValid = Hash.ArtemisHashVerify(userDocument.PasswordHash, password);

        if (!passwordValid)
        {
            result.Message = "密码错误";

            return (result, default);
        }

        result.Succeeded = true;
        result.Message = "认证成功";

        var userClaims = await UserClaimStore.EntityQuery
            .Where(item => item.UserId == userDocument.Id)
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        var userRoles = await UserStore.KeyMatchQuery(userDocument.Id)
            .SelectMany(item => item.Roles)
            .ProjectToType<RoleInfo>()
            .ToListAsync(cancellationToken);

        var roleIds = userRoles.Select(item => item.Id);

        var roleClaims = await RoleClaimStore.EntityQuery
            .Where(item => roleIds.Contains(item.RoleId))
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        var token = new TokenDocument
        {
            UserId = userDocument.Id,
            UserName = userDocument.UserName,
            UserClaims = userClaims,
            Roles = userRoles,
            RoleClaims = roleClaims
        };

        return (result, token);
    }

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="package">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>注册结果和登录后的Token信息</returns>
    public async Task<(SignResult result, TokenDocument? token)> SignUpAsync(
        UserPackage package,
        string password,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedUserName = NormalizeKey(package.UserName);

        var exists = await UserStore.EntityQuery
            .AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "注册失败"
        };

        if (exists)
        {
            result.Message = "用户已存在";

            return (result, default);
        }

        var user = Instance.CreateInstance<ArtemisUser, UserPackage>(package);

        user.NormalizedUserName = normalizedUserName;

        user.NormalizedEmail = package.Email is not null ? NormalizeKey(package.Email) : string.Empty;

        user.NormalizedPhoneNumber = package.PhoneNumber is not null ? NormalizeKey(package.PhoneNumber) : string.Empty;

        user.PasswordHash = Hash.ArtemisHash(password);

        user.SecurityStamp = package.GenerateSecurityStamp;

        var storeResult = await UserStore.CreateAsync(user, cancellationToken);

        if (!storeResult.Succeeded)
        {
            result.Message = "用户创建失败";
            return (result, default);
        }

        result.Succeeded = true;
        result.Message = "用户创建成功";

        var userClaims = await UserClaimStore.EntityQuery
            .Where(item => item.UserId == user.Id)
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        var userRoles = await UserStore.KeyMatchQuery(user.Id)
            .SelectMany(item => item.Roles)
            .ProjectToType<RoleInfo>()
            .ToListAsync(cancellationToken);

        var roleIds = userRoles.Select(item => item.Id);

        var roleClaims = await RoleClaimStore.EntityQuery
            .Where(item => roleIds.Contains(item.RoleId))
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        var token = new TokenDocument
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserClaims = userClaims,
            Roles = userRoles,
            RoleClaims = roleClaims
        };

        return (result, token);
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>修改结果</returns>
    public async Task<SignResult> ChangePasswordAsync(
        Guid userId,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "修改失败"
        };

        if (oldPassword == newPassword)
        {
            result.Message = "新密码不能与旧密码相同";

            return result;
        }

        var user = await UserStore.KeyMatchQuery(userId).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            result.Message = "未找到匹配签名的用户，请检查输入";

            return result;
        }

        var passwordValid = Hash.ArtemisHashVerify(user.PasswordHash, oldPassword);

        if (!passwordValid)
        {
            result.Message = "原密码错误";

            return result;
        }

        user.PasswordHash = Hash.ArtemisHash(newPassword);

        var storeResult = await UserStore.UpdateAsync(user, cancellationToken);

        if (storeResult.Succeeded)
        {
            result.Succeeded = true;
            result.Message = "修改成功";
        }

        return result;
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="password">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>重置结果</returns>
    public async Task<SignResult> ResetPasswordAsync(
        Guid userId,
        string password,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var user = await UserStore
            .KeyMatchQuery(userId)
            .FirstOrDefaultAsync(cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "重置失败"
        };

        if (user is null)
        {
            result.Message = "未找到匹配用户";

            return result;
        }

        user.PasswordHash = Hash.ArtemisHash(password);

        var storeResult = await UserStore.UpdateAsync(user, cancellationToken);

        result.Message = storeResult.DescribeError;

        if (storeResult.Succeeded)
        {
            result.Succeeded = true;
            result.Message = "重置成功";
        }

        return result;
    }

    /// <summary>
    ///     批量重置密码
    /// </summary>
    /// <param name="packages">重置密码信息包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>批量重置结果</returns>
    public async Task<SignResult> ResetPasswordsAsync(
        IEnumerable<KeyValuePair<Guid, string>> packages,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var keyValuePairs = packages.ToList();

        var ids = keyValuePairs.Select(item => item.Key);

        var matchedUsers = await UserStore.KeyMatchQuery(ids)
            .ToListAsync(cancellationToken);

        var users = matchedUsers.Join(
            keyValuePairs,
            user => user.Id,
            pairs => pairs.Key, (user, pairs) =>
            {
                var (_, password) = pairs;
                user.PasswordHash = Hash.ArtemisHash(password);
                return user;
            });

        var result = new SignResult
        {
            Succeeded = false,
            Message = "重置失败"
        };

        var artemisUsers = users as ArtemisUser[] ?? users.ToArray();

        if (!artemisUsers.Any())
        {
            result.Message = "未找到匹配用户";

            return result;
        }

        var storeResult = await UserStore.UpdateAsync(artemisUsers, cancellationToken);

        if (storeResult.Succeeded)
        {
            result.Succeeded = true;

            result.Message = "重置成功";
        }

        return result;
    }

    #endregion
}