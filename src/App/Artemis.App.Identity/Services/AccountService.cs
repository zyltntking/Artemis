using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Extensions.Rpc;
using Artemis.Extensions.Web.Identity;
using Artemis.Protos.Identity;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Transfer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
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
    /// <param name="userManager">用户管理器依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <param name="options">认证配置依赖</param>
    /// <param name="logger">日志依赖</param>
    public AccountService(
        IAccountManager accountManager,
        IUserManager userManager,
        IDistributedCache cache,
        IOptions<SharedIdentityOptions> options,
        ILogger<AccountService> logger)
    {
        AccountManager = accountManager;
        UserManager = userManager;
        Cache = cache;
        Options = options.Value;
        Logger = logger;
    }

    /// <summary>
    ///     账户管理器
    /// </summary>
    private IAccountManager AccountManager { get; }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IUserManager UserManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     认证配置依赖
    /// </summary>
    private SharedIdentityOptions Options { get; }

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
        // 签到
        var (result, token) =
            await AccountManager.SignInAsync(request.UserSign, request.Password, context.CancellationToken);

        if (result.Succeeded && token is not null)
        {
            token.EndType = request.EndType;

            // 记录TokenDocument
            var identityToken = await RecordTokenDocument(token, context.CancellationToken);

            return DataResult.Success(new TokenReply
            {
                Token = identityToken,
                Expire = DateTime.Now.AddSeconds(Options.CacheIdentityTokenExpire).ToUnixTimeStamp()
            }).Adapt<TokenResponse>();
        }

        return RpcResultAdapter.EmptyFail<TokenResponse>(result.Message);
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
            token.EndType = Constants.EndForSignUp;

            // 记录TokenDocument
            var identityToken = await RecordTokenDocument(token, context.CancellationToken);

            return DataResult.Success(new TokenReply
            {
                Token = identityToken,
                Expire = DateTime.Now.AddSeconds(Options.CacheIdentityTokenExpire).ToUnixTimeStamp()
            }).Adapt<TokenResponse>();
        }

        return RpcResultAdapter.EmptyFail<TokenResponse>(result.Message);
    }

    /// <summary>
    ///     签出
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签出")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> SignOut(Empty request, ServerCallContext context)
    {
        var token = context.RequestHeaders.Get(Options.HeaderIdentityTokenKey)?.Value;

        if (token is not null)
        {
            await EraseTokenDocument(token,context.CancellationToken);

            return RpcResultAdapter.EmptySuccess<EmptyResponse>();
        }

        return RpcResultAdapter.EmptyFail<EmptyResponse>();
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("修改密码")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<EmptyResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
    {
        var token = context.RequestHeaders.Get(Options.HeaderIdentityTokenKey)?.Value;

        if (token is not null)
        {
            var cacheTokenKey = $"{Options.CacheIdentityTokenPrefix}:{token}";

            var tokenDocument = await Cache.FetchTokenAsync(cacheTokenKey, false, context.CancellationToken);

            if (tokenDocument is not null)
            {
                var result = await AccountManager.ChangePasswordAsync(tokenDocument.UserId, request.OldPassword,
                    request.NewPassword, context.CancellationToken);

                return result.Succeeded
                    ? RpcResultAdapter.EmptySuccess<EmptyResponse>()
                    : RpcResultAdapter.EmptyFail<EmptyResponse>(result.Message);
            }
        }

        return RpcResultAdapter.EmptyFail<EmptyResponse>();
    }

    /// <summary>
    ///     重置密码
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

        return result.Succeeded
            ? RpcResultAdapter.EmptySuccess<EmptyResponse>()
            : RpcResultAdapter.EmptyFail<EmptyResponse>(result.Message);
    }

    /// <summary>
    ///     批量重置密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("重置密码")]
    [Authorize(IdentityPolicy.Admin)]
    public override async Task<EmptyResponse> BatchResetPasswords(BatchResetPasswordRequest request,
        ServerCallContext context)
    {
        var packages = new List<KeyValuePair<Guid, string>>();

        foreach (var passwordPackage in request.ResetPasswordPackages)
        {
            var userId = passwordPackage.UserId.GuidFromString();

            packages.Add(new KeyValuePair<Guid, string>(userId, passwordPackage.Password));
        }

        var result = await AccountManager.ResetPasswordsAsync(packages, context.CancellationToken);

        return result.Succeeded
            ? RpcResultAdapter.EmptySuccess<EmptyResponse>()
            : RpcResultAdapter.EmptyFail<EmptyResponse>(result.Message);
    }

    #endregion

    #region Logic

    /// <summary>
    ///     记录TokenDocument
    /// </summary>
    /// <param name="tokenDocument">tokenDocument</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>认证Token</returns>
    private async Task<string> RecordTokenDocument(TokenDocument tokenDocument,
        CancellationToken cancellationToken = default)
    {
        var identityToken = tokenDocument.GenerateTokenKey();

        Logger.LogInformation($"生成认证Token：{identityToken}");

        Logger.LogInformation("添加UserLogin记录");

        // 记录UserLogin
        var loginPackage = new UserLoginPackage
        {
            LoginProvider = Options.IdentityServiceProvider,
            ProviderKey = $"{tokenDocument.EndType}:{tokenDocument.UserId}",
            ProviderDisplayName = tokenDocument.UserName
        };

        await UserManager.AddOrUpdateUserLoginAsync(tokenDocument.UserId, loginPackage, cancellationToken);

        Logger.LogInformation("添加UserToken记录");

        // 记录UserToken
        var userToken = new UserTokenPackage
        {
            LoginProvider = Options.IdentityServiceProvider,
            Name = tokenDocument.EndType,
            Value = identityToken
        };

        await UserManager.AddOrUpdateUserTokenAsync(tokenDocument.UserId, userToken, cancellationToken);

        // 缓存Token
        var cacheTokenKey = $"{Options.CacheIdentityTokenPrefix}:{identityToken}";

        await Cache.CacheTokenAsync(tokenDocument, cacheTokenKey, Options.CacheIdentityTokenExpire, cancellationToken);

        // 不允许同终端多客户端登录处理
        if (!Options.EnableMultiEnd)
        {
            //缓存映射Token
            var userMapTokenKey = $"{Options.UserMapTokenPrefix}:{tokenDocument.EndType}:{tokenDocument.UserId}";

            await Cache.CacheUserMapTokenAsync(userMapTokenKey, identityToken, Options.CacheIdentityTokenExpire,
                cancellationToken);
        }

        return identityToken;
    }

    /// <summary>
    ///     擦除TokenDocument
    /// </summary>
    /// <param name="identityToken">认证token</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task EraseTokenDocument(string identityToken, CancellationToken cancellationToken = default)
    {
        var cacheTokenKey = $"{Options.CacheIdentityTokenPrefix}:{identityToken}";

        var tokenDocument = await Cache.FetchTokenAsync(cacheTokenKey, false, cancellationToken);

        if (tokenDocument is not null)
        {
            await UserManager.RemoveUserTokenAsync(tokenDocument.UserId, Options.IdentityServiceProvider, tokenDocument.EndType,
                cancellationToken);

            await UserManager.RemoveUserLoginAsync(tokenDocument.UserId, Options.IdentityServiceProvider, tokenDocument.EndType, cancellationToken);

            // 不允许同终端多客户端登录处理
            if (!Options.EnableMultiEnd)
            {
                var userMapTokenKey = $"{Options.UserMapTokenPrefix}:{tokenDocument.EndType}:{tokenDocument.UserId}";

                await Cache.RemoveAsync(userMapTokenKey, cancellationToken);
            }
        }

        await Cache.RemoveAsync(cacheTokenKey, cancellationToken);
    }

    #endregion
}