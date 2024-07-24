using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Extensions.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Identity;
using Artemis.Service.Shared.Identity.Transfer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Service.Identity.Services;

/// <summary>
///     账户服务
/// </summary>
public class AccountServiceImplement : AccountService.AccountServiceBase
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器</param>
    /// <param name="userManager">用户管理器</param>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    /// <param name="logger">日志记录器</param>
    public AccountServiceImplement(
        IIdentityAccountManager accountManager,
        IIdentityUserManager userManager,
        IDistributedCache cache,
        IOptions<ArtemisIdentityOptions> options,
        ILogger<AccountServiceImplement> logger)
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
    private IIdentityAccountManager AccountManager { get; }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IIdentityUserManager UserManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    /// <summary>
    ///     授权配置项
    /// </summary>
    private ArtemisIdentityOptions Options { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<AccountServiceImplement> Logger { get; }

    #region Overrides of AccountBase

    /// <summary>
    ///     签到
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签到")]
    [Authorize(AuthorizePolicy.Anonymous)]
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
            token.EndType = request.EndType.ToString("G");

            // 记录TokenDocument
            var identityToken = await RecordTokenDocument(token, context.CancellationToken);

            var reply = new TokenReply
            {
                Token = identityToken,
                Expire = DateTime.Now.AddSeconds(Options.CacheTokenExpire).ToUnixTimeStamp()
            };

            return ResultAdapter.AdaptSuccess<TokenResponse, TokenReply>(reply);
        }

        return ResultAdapter.AdaptEmptyFail<TokenResponse>();
    }

    /// <summary>
    ///     签入
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签入")]
    [Authorize(AuthorizePolicy.Anonymous)]
    public override async Task<TokenResponse> SignUp(SignUpRequest request, ServerCallContext context)
    {
        var (result, token) = await AccountManager
            .SignUpAsync(new UserPackage
            {
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.Phone
            }, request.Password, context.CancellationToken);

        if (result.Succeeded && token is not null)
        {
            token.EndType = EndType.SignUpEnd.Name;

            // 记录TokenDocument
            var identityToken = await RecordTokenDocument(token, context.CancellationToken);

            var reply = new TokenReply
            {
                Token = identityToken,
                Expire = DateTime.Now.AddSeconds(Options.CacheTokenExpire).ToUnixTimeStamp()
            };

            return ResultAdapter.AdaptSuccess<TokenResponse, TokenReply>(reply);
        }

        return ResultAdapter.AdaptEmptyFail<TokenResponse>();
    }

    /// <summary>
    ///     签出
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("签出")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<EmptyResponse> SignOut(Empty request, ServerCallContext context)
    {
        var authorizationToken = context
            .GetHttpContext()
            .User.Claims
            .Where(claim => claim.Type == ArtemisClaimTypes.Authorization.Name)
            .Select(claim => claim.Value)
            .FirstOrDefault();

        if (authorizationToken is not null)
        {
            await EraseTokenDocument(authorizationToken, context.CancellationToken);

            return ResultAdapter.AdaptEmptySuccess<EmptyResponse>();
        }

        return ResultAdapter.AdaptEmptyFail<EmptyResponse>();
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("修改密码")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<EmptyResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
    {
        var userIdString = context
            .GetHttpContext()
            .User.Claims
            .Where(claim => claim.Type == ArtemisClaimTypes.UserId.Name)
            .Select(claim => claim.Value)
            .FirstOrDefault();

        var valid = Guid.TryParse(userIdString, out var userId);

        if (valid)
        {
            var result = await AccountManager.ChangePasswordAsync(
                userId,
                request.OldPassword,
                request.NewPassword,
                context.CancellationToken);

            return result.Succeeded
                ? ResultAdapter.AdaptEmptySuccess<EmptyResponse>()
                : ResultAdapter.AdaptEmptyFail<EmptyResponse>(result.Message);
        }

        return ResultAdapter.AdaptEmptySuccess<EmptyResponse>();
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("重置密码")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<EmptyResponse> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
    {
        var userId = request.UserId.GuidFromString();

        var result = await AccountManager.ResetPasswordAsync(
            userId,
            request.Password,
            context.CancellationToken);

        return result.Succeeded
            ? ResultAdapter.AdaptEmptySuccess<EmptyResponse>()
            : ResultAdapter.AdaptEmptyFail<EmptyResponse>(result.Message);
    }

    /// <summary>
    ///     批量重置密码
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("重置密码")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<EmptyResponse> BatchResetPasswords(BatchResetPasswordRequest request,
        ServerCallContext context)
    {
        var dictionary = new Dictionary<Guid, string>();

        foreach (var resetPassword in request.Batch)
        {
            var userId = resetPassword.UserId.GuidFromString();
            var password = resetPassword.Password;

            dictionary.TryAdd(userId, password);
        }

        var result = await AccountManager.ResetPasswordsAsync(dictionary, context.CancellationToken);

        return result.Succeeded
            ? ResultAdapter.AdaptEmptySuccess<EmptyResponse>()
            : ResultAdapter.AdaptEmptyFail<EmptyResponse>(result.Message);
    }

    #endregion

    #region InternalLogic

    /// <summary>
    ///     记录TokenDocument
    /// </summary>
    /// <param name="record">Token记录</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>认证Token</returns>
    private async Task<string> RecordTokenDocument(
        TokenRecord record,
        CancellationToken cancellationToken = default)
    {
        var tokenSymbol = record.TokenSymbol();

        Logger.LogInformation($"生成认证Token：{tokenSymbol}");

        Logger.LogInformation("添加UserLogin记录");

        // 记录UserLogin
        var loginPackage = new UserLoginPackage
        {
            LoginProvider = Options.IdentityServiceProvider,
            ProviderKey = TokenKeyGenerator.LoginProviderKey(record.UserId, record.EndType),
            ProviderDisplayName = record.UserName
        };

        await UserManager.AddOrUpdateUserLoginAsync(record.UserId, loginPackage, cancellationToken);

        Logger.LogInformation("添加UserToken记录");

        // 记录UserToken
        var userToken = new UserTokenPackage
        {
            LoginProvider = Options.IdentityServiceProvider,
            Name = TokenKeyGenerator.ProviderTokenName(record.EndType, Options.IdentityServiceTokenNameSuffix),
            Value = tokenSymbol
        };

        await UserManager.AddOrUpdateUserTokenAsync(record.UserId, userToken, cancellationToken);

        // 缓存Token
        var cacheTokenKey = TokenKeyGenerator.CacheTokenKey(Options.CacheTokenPrefix, tokenSymbol);

        await Cache.CacheTokenRecordAsync(cacheTokenKey, record, cancellationToken);

        // 不允许同终端多客户端登录处理
        if (!Options.EnableMultiEnd)
        {
            //缓存映射Token
            var userMapTokenKey =
                TokenKeyGenerator.CacheUserMapTokenKey(Options.CacheUserMapTokenPrefix, record.EndType, record.UserId);

            await Cache.CacheUserMapTokenSymbolAsync(userMapTokenKey, tokenSymbol, Options.CacheTokenExpire,
                cancellationToken);
        }

        return tokenSymbol;
    }

    /// <summary>
    ///     擦除TokenDocument
    /// </summary>
    /// <param name="identityToken">认证token</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    private async Task EraseTokenDocument(string identityToken, CancellationToken cancellationToken = default)
    {
        var cacheTokenKey = TokenKeyGenerator.CacheTokenKey(Options.CacheTokenPrefix, identityToken);

        var tokenDocument = await Cache.FetchTokenRecordAsync(cacheTokenKey, false, cancellationToken);

        if (tokenDocument is not null)
        {
            var providerTokenName =
                TokenKeyGenerator.ProviderTokenName(tokenDocument.EndType, Options.IdentityServiceTokenNameSuffix);

            await UserManager.RemoveUserTokenAsync(
                tokenDocument.UserId,
                Options.IdentityServiceProvider,
                providerTokenName,
                cancellationToken);

            var loginProviderKey = TokenKeyGenerator.LoginProviderKey(tokenDocument.UserId, tokenDocument.EndType);

            await UserManager.RemoveUserLoginAsync(
                tokenDocument.UserId,
                Options.IdentityServiceProvider,
                loginProviderKey,
                cancellationToken);

            // 不允许同终端多客户端登录处理
            if (!Options.EnableMultiEnd)
            {
                //缓存映射Token
                var userMapTokenKey = TokenKeyGenerator.CacheUserMapTokenKey(
                    Options.CacheUserMapTokenPrefix,
                    tokenDocument.EndType,
                    tokenDocument.UserId);

                await Cache.RemoveAsync(userMapTokenKey, cancellationToken);
            }
        }

        await Cache.RemoveAsync(cacheTokenKey, cancellationToken);
    }

    #endregion
}