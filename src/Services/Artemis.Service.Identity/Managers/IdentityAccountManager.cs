using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Stores;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证账号管理
/// </summary>
public class IdentityAccountManager : Manager<IdentityUser, Guid, Guid>, IIdentityAccountManager
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
        UserStore = userStore;
        UserClaimStore = userClaimStore;
        RoleClaimStore = roleClaimStore;
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

        var userDocument = await UserStore.EntityQuery
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

        if (userDocument is null)
        {
            result.Message = "未找到匹配签名的用户，请检查输入";
            return (result, default);
        }

        var passwordValid = Hash.PasswordVerify(userDocument.PasswordHash, password);

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
            .SelectMany(item => item.Roles!)
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
            RoleClaims = roleClaims,
            EndType = EndType.SignInitial
        };

        return (result, token);
    }

    #endregion
}