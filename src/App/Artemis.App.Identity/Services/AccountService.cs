using Artemis.Data.Shared.Transfer;
using Artemis.Extensions.ServiceConnect;
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
    public override async Task<TokenResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        Logger.LogInformation($"用户 {request.UserSign} 正在尝试登录...");

        var (result, token) = await AccountManager.SignInAsync(
            request.UserSign,
            request.Password,
            context.CancellationToken);

        Logger.LogInformation($"用户 {request.UserSign} 认证成功，准备编码授权信息...");

        if (result.Succeeded && token is not null)
        {
            token.EndType = request.EndType;

            // 记录TokenDocument
            // var identityToken = await RecordTokenDocument(token, context.CancellationToken);

            // todo ...


            return new TokenResponse();
        }

        return RpcResultAdapter.EmptyFail<TokenResponse>(result.Message);
    }

    #endregion
}