using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Protos.Identity;
using Artemis.Service.Identity.Managers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.App.Identity.Services;

/// <summary>
///     账户服务
/// </summary>
public class AccountService : Account.AccountBase
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器</param>
    /// <param name="userManager">用户管理器</param>
    /// <param name="logger">日志记录器</param>
    public AccountService(
        IIdentityAccountManager accountManager,
        IIdentityUserManager userManager,
        ILogger<AccountService> logger)
    {
        AccountManager = accountManager;
        UserManager = userManager;
        Logger = logger;
    }

    /// <summary>
    ///     账户管理器
    /// </summary>
    private IIdentityAccountManager AccountManager { get; }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IIdentityUserManager UserManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<AccountService> Logger { get; }

    #region Overrides of AccountBase

    /// <summary>
    ///     签到
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Authorize(IdentityPolicy.Admin)]
    public override Task<TokenResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        Logger.LogInformation("用户 {0} 正在尝试登录", request.UserSign);

        return Task.FromResult(new TokenResponse());
    }

    #endregion
}