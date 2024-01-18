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
    public override async Task<TokenResponse> SignIn(SingInRequest request, ServerCallContext context)
    {
        var (result, token) =
            await AccountManager.SignInAsync(request.UserSign, request.Password, context.CancellationToken);

        if (result.Succeeded && token is not null)
        {
            var replyToken = token.GenerateTokenKey();

            var cacheKey = $"{Options.CacheTokenPrefix}:{replyToken}";

            Cache.CacheToken(token, cacheKey, Options.Expire);

            return RpcResultAdapter.Success<TokenResponse, TokenReply>(new TokenReply
            {
                Token = replyToken,
                Expire = DateTime.Now.AddSeconds(Options.Expire).ToUnixTimeStamp()
            });
        }

        return RpcResultAdapter.Fail<TokenResponse>();
    }

    /// <summary>
    /// 签入
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

            var cacheKey = $"{Options.CacheTokenPrefix}:{replyToken}";

            Cache.CacheToken(token, cacheKey, Options.Expire);

            return RpcResultAdapter.Success<TokenResponse, TokenReply>(new TokenReply
            {
                Token = replyToken,
                Expire = DateTime.Now.AddSeconds(Options.Expire).ToUnixTimeStamp()
            });
        }

        return RpcResultAdapter.Fail<TokenResponse>();
    }

    /// <summary>
    /// 签出
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签出")]
    [Authorize(IdentityPolicy.Token)]
    public override Task<EmptyResponse> SignOut(Empty request, ServerCallContext context)
    {
        throw new NotImplementedException();

        return Task.FromResult(RpcResultAdapter.Success<EmptyResponse>());
    }

    #endregion
}