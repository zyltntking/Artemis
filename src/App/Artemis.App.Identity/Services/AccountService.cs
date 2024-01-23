using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Extensions.Rpc;
using Artemis.Extensions.Web.Identity;
using Artemis.Protos.Identity;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Services;

/// <summary>
///     账户服务
/// </summary>
public class AccountService : Account.AccountBase
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <param name="options">认证配置依赖</param>
    /// <param name="logger">日志依赖</param>
    public AccountService(
        IAccountManager accountManager,
        IDistributedCache cache,
        IOptions<InternalAuthorizationOptions> options,
        ILogger<AccountService> logger)
    {
        AccountManager = accountManager;
        Cache = cache;
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IAccountManager AccountManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     认证配置依赖
    /// </summary>
    private InternalAuthorizationOptions Options { get; }

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
    [Description("签到")]
    [Authorize(IdentityPolicy.Anonymous)]
    public override async Task<TokenResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        var (result, token) =
            await AccountManager.SignInAsync(request.UserSign, request.Password, context.CancellationToken);

        if (result.Succeeded && token is not null)
        {
            var replyToken = token.GenerateTokenKey();

            var cacheTokenKey = $"{Options.CacheTokenPrefix}:{replyToken}";

            await Cache.CacheTokenAsync(token, cacheTokenKey, Options.Expire, context.CancellationToken);

            // 不允许多终端登录处理
            if (!Options.EnableMultiEnd)
            {
                var userMapTokenKey = $"{Options.UserMapTokenPrefix}:{token.UserId}";

                var oldToken = await Cache.FetchUserMapTokenAsync(userMapTokenKey, false, context.CancellationToken);

                if (oldToken is not null)
                {
                    var cacheOldTokenKey = $"{Options.CacheTokenPrefix}:{oldToken}";

                    await Cache.RemoveAsync(cacheOldTokenKey, context.CancellationToken);
                }

                await Cache.CacheUserMapTokenAsync(userMapTokenKey, replyToken, Options.Expire, context.CancellationToken);
            }

            return RpcResultAdapter.Success<TokenResponse, TokenReply>(new TokenReply
            {
                Token = replyToken,
                Expire = DateTime.Now.AddSeconds(Options.Expire).ToUnixTimeStamp()
            });
        }

        return RpcResultAdapter.Fail<TokenResponse>(result.Message);
    }

    /// <summary>
    ///     签入
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签入")]
    [Authorize(IdentityPolicy.Anonymous)]
    public override async Task<TokenResponse> SignUp(SignUpRequest request, ServerCallContext context)
    {
        var (result, token) = await AccountManager
            .SignUpAsync(new UserPackage
            {
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            }, request.Password, context.CancellationToken);

        if (result.Succeeded && token is not null)
        {
            var replyToken = token.GenerateTokenKey();

            var cacheTokenKey = $"{Options.CacheTokenPrefix}:{replyToken}";

            await Cache.CacheTokenAsync(token, cacheTokenKey, Options.Expire, context.CancellationToken);

            // 不允许多终端登录处理
            if (!Options.EnableMultiEnd)
            {
                var userMapTokenKey = $"{Options.UserMapTokenPrefix}:{token.UserId}";

                await Cache.CacheUserMapTokenAsync(userMapTokenKey, replyToken, Options.Expire, context.CancellationToken);
            }

            return RpcResultAdapter.Success<TokenResponse, TokenReply>(new TokenReply
            {
                Token = replyToken,
                Expire = DateTime.Now.AddSeconds(Options.Expire).ToUnixTimeStamp()
            });
        }

        return RpcResultAdapter.Fail<TokenResponse>(result.Message);
    }

    /// <summary>
    ///     签出
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签出")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> SignOut(EmptyPackage request, ServerCallContext context)
    {
        var token = context.RequestHeaders.Get(Options.HeaderTokenKey)?.Value;

        if (token is not null)
        {
            var cacheTokenKey = $"{Options.CacheTokenPrefix}:{token}";

            // 不允许多终端登录处理
            if (!Options.EnableMultiEnd)
            {
                var tokenDocument = await Cache.FetchTokenAsync(cacheTokenKey, false, context.CancellationToken);

                if (tokenDocument is not null)
                {
                    var userMapTokenKey = $"{Options.UserMapTokenPrefix}:{tokenDocument.UserId}";

                    await Cache.RemoveAsync(userMapTokenKey, context.CancellationToken);
                }
            }

            await Cache.RemoveAsync(cacheTokenKey, context.CancellationToken);


            return RpcResultAdapter.Success<EmptyResponse>();
        }

        return RpcResultAdapter.Fail<EmptyResponse>();
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("修改密码")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
    {
        var token = context.RequestHeaders.Get(Options.HeaderTokenKey)?.Value;

        if (token is not null)
        {
            var cacheTokenKey = $"{Options.CacheTokenPrefix}:{token}";

            var tokenDocument = await Cache.FetchTokenAsync(cacheTokenKey, false, context.CancellationToken);

            if (tokenDocument is not null)
            {
                var result = await AccountManager.ChangePasswordAsync(tokenDocument.UserId, request.OldPassword, request.NewPassword, context.CancellationToken);

                return result.Succeeded ? 
                    RpcResultAdapter.Success<EmptyResponse>() : 
                    RpcResultAdapter.Fail<EmptyResponse>(result.Message);
            }

        }

        return RpcResultAdapter.Fail<EmptyResponse>();
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("重置密码")]
    [Authorize(IdentityPolicy.Admin)]
    public override async Task<EmptyResponse> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
    {
        var userId = request.UserId.GuidFromString();

        var result = await AccountManager.ResetPasswordAsync(userId, request.Password, context.CancellationToken);

        return result.Succeeded ? 
            RpcResultAdapter.Success<EmptyResponse>() : 
            RpcResultAdapter.Fail<EmptyResponse>(result.Message);
    }

    /// <summary>
    /// 批量重置密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("重置密码")]
    [Authorize(IdentityPolicy.Admin)]
    public override async Task<EmptyResponse> BatchResetPasswords(BatchResetPasswordRequest request, ServerCallContext context)
    {
        var packages = new List<KeyValuePair<Guid, string>>();

        foreach (var passwordPackage in request.ResetPasswordPackages)
        {
            var userId = passwordPackage.UserId.GuidFromString();

            packages.Add(new KeyValuePair<Guid, string>(userId, passwordPackage.Password));
        }

        var result = await AccountManager.ResetPasswordsAsync(packages, context.CancellationToken);

        return result.Succeeded ?
            RpcResultAdapter.Success<EmptyResponse>() :
            RpcResultAdapter.Fail<EmptyResponse>(result.Message);
    }

    #endregion
}