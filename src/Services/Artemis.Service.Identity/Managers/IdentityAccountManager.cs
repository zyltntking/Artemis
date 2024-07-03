using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
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
///     认证账号管理
/// </summary>
public sealed class IdentityAccountManager : Manager<IdentityUser>, IIdentityAccountManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="userStore">存储访问器依赖</param>
    /// <param name="roleClaimStore">角色凭据依赖</param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="userClaimStore">用户凭据依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityAccountManager(
        IIdentityUserStore userStore,
        IIdentityUserClaimStore userClaimStore,
        IIdentityRoleClaimStore roleClaimStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(userStore, options, logger)
    {
        UserStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        UserClaimStore = userClaimStore ?? throw new ArgumentNullException(nameof(userClaimStore));
        RoleClaimStore = roleClaimStore ?? throw new ArgumentNullException(nameof(roleClaimStore));
    }

    #region Dispose

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        UserStore.Dispose();
        UserClaimStore.Dispose();
        RoleClaimStore.Dispose();
    }

    #endregion

    #region Logic

    /// <summary>
    ///     签发Token文档
    /// </summary>
    /// <param name="authentication">认证用户</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task<TokenDocument> IssuedTokenDocument(UserAuthentication authentication,
        CancellationToken cancellationToken = default)
    {
        var userClaims = await UserClaimStore.EntityQuery
            .Where(claim => claim.UserId == authentication.Id)
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        var userRoles = await UserStore.KeyMatchQuery(authentication.Id)
            .SelectMany(user => user.Roles!)
            .ProjectToType<RoleInfo>()
            .ToListAsync(cancellationToken);

        var roleIds = userRoles.Select(item => item.Id);

        var roleClaims = await RoleClaimStore.EntityQuery
            .Where(claim => roleIds.Contains(claim.RoleId))
            .ProjectToType<ClaimPackage>()
            .ToListAsync(cancellationToken);

        return new TokenDocument
        {
            UserId = authentication.Id,
            UserName = authentication.UserName,
            UserClaims = userClaims,
            Roles = userRoles,
            RoleClaims = roleClaims,
            EndType = InternalEndType.SignInitial
        };
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     用户存储访问器
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityUserClaimStore UserClaimStore { get; }

    /// <summary>
    ///     用户凭据存储访问器
    /// </summary>
    private IIdentityRoleClaimStore RoleClaimStore { get; }

    #endregion

    #region Implementation of IIdentityAccountManager

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

        var normalizeSign = userSign.StringNormalize();

        var userNameQuery = UserStore.EntityQuery
            .Where(item => item.NormalizedUserName == normalizeSign);

        var emailQuery = UserStore.EntityQuery
            .Where(item => item.NormalizedEmail == normalizeSign);

        var phoneNumberQuery = UserStore.EntityQuery
            .Where(item => item.PhoneNumber == normalizeSign);

        var userAuthentication = await UserStore.EntityQuery
            .Union(userNameQuery)
            .Union(emailQuery)
            .Union(phoneNumberQuery)
            .ProjectToType<UserAuthentication>()
            .FirstOrDefaultAsync(cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "认证失败"
        };

        if (userAuthentication is null)
        {
            result.Message = "未找到匹配签名的用户，请检查输入";
            return (result, default);
        }

        var passwordValid = Hash.PasswordVerify(userAuthentication.PasswordHash, password);

        if (!passwordValid)
        {
            result.Message = "密码错误";

            return (result, default);
        }

        result.Succeeded = true;
        result.Message = "认证成功";

        var tokenDocument = await IssuedTokenDocument(userAuthentication, cancellationToken);

        return (result, tokenDocument);
    }

    /// <summary>
    ///     报名/注册
    /// </summary>
    /// <param name="userSign">用户标识信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>注册结果和登录后的Token信息</returns>
    public async Task<(SignResult result, TokenDocument? token)> SignUpAsync(UserSign userSign, string password,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var normalizedUserName = userSign.UserName.StringNormalize();

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

        var user = Instance.CreateInstance<IdentityUser, UserSign>(userSign);

        user.NormalizedUserName = normalizedUserName;

        user.NormalizedEmail = userSign.Email is not null ? userSign.Email.StringNormalize() : string.Empty;

        user.PasswordHash = Hash.PasswordHash(password);

        user.SecurityStamp = Generator.SecurityStamp;

        var storeResult = await UserStore.CreateAsync(user, cancellationToken);

        if (!storeResult.Succeeded)
        {
            result.Message = "用户创建失败";
            return (result, default);
        }

        result.Succeeded = true;
        result.Message = "用户创建成功";

        var tokenDocument = await IssuedTokenDocument(new UserAuthentication
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash
        }, cancellationToken);

        return (result, tokenDocument);
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="userId">用户标识</param>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>修改结果</returns>
    public async Task<SignResult> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword,
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

        var passwordValid = Hash.PasswordVerify(user.PasswordHash, oldPassword);

        if (!passwordValid)
        {
            result.Message = "原密码错误";

            return result;
        }

        user.PasswordHash = Hash.PasswordHash(newPassword);

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
    public async Task<SignResult> ResetPasswordAsync(Guid userId, string password,
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

        user.PasswordHash = Hash.PasswordHash(password);

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
    ///     批量修改密码
    /// </summary>
    /// <param name="dictionary">重置密码信息包</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>批量重置结果</returns>
    public async Task<SignResult> ResetPasswordsAsync(IDictionary<Guid, string> dictionary,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var ids = dictionary.Keys;

        var matchedUsers = await UserStore.KeyMatchQuery(ids)
            .ToListAsync(cancellationToken);

        var result = new SignResult
        {
            Succeeded = false,
            Message = "重置失败"
        };

        if (!matchedUsers.Any())
        {
            result.Message = "未找到匹配用户";

            return result;
        }

        var users = matchedUsers.Select(user =>
        {
            user.PasswordHash = Hash.PasswordHash(dictionary[user.Id]);
            return user;
        }).ToList();

        var storeResult = await UserStore.UpdateAsync(users, cancellationToken);

        if (storeResult.Succeeded)
        {
            result.Succeeded = true;

            result.Message = "重置成功";
        }

        return result;
    }

    #endregion
}