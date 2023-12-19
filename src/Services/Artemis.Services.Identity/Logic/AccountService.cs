using Artemis.Data.Core;
using Artemis.Data.Grpc;
using Artemis.Extensions.Web.Identity;
using Artemis.Services.Identity.Managers;
using Artemis.Services.Rpc;
using Artemis.Shared.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Artemis.Services.Identity.Logic;

/// <summary>
///     账户服务
/// </summary>
public class AccountService : RpcServiceBase, IAccountService
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器依赖</param>
    /// <param name="cache">分布式缓存依赖</param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="options"></param>
    public AccountService(
        IAccountManager accountManager,
        IDistributedCache cache,
        IHttpContextAccessor httpContextAccessor,
        IOptions<InternalAuthorizationOptions> options) : base(httpContextAccessor, options)
    {
        AccountManager = accountManager;
        Cache = cache;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IAccountManager AccountManager { get; }

    /// <summary>
    ///     分布式缓存依赖
    /// </summary>
    private IDistributedCache Cache { get; }

    #region Implementation of IAccountService

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize(IdentityPolicy.Anonymous)]
    public async Task<TokenResponse> SignInAsync(SignInRequest request)
    {
        var (result, token) = await AccountManager
            .SignInAsync(request, CancellationToken);

        if (result.Succeeded)
            if (token is not null)
            {
                var replyToken = token.GenerateTokenKey();

                var cacheKey = $"{Options.CacheTokenPrefix}:{replyToken}";

                Cache.CacheToken(token, cacheKey, Options.Expire);

                return new TokenResponse
                {
                    Result = GrpcResponse.SuccessResult(),
                    Data = new TokenResult
                    {
                        Token = replyToken,
                        Expire = DateTime.Now.AddDays(30).ToUnixTimeStamp()
                    }
                };
            }

        return new TokenResponse
        {
            Result = GrpcResponse.FailResult(result.Message),
            Data = null
        };
    }

    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize(IdentityPolicy.Anonymous)]
    public async Task<TokenResponse> SignUpAsync(SignUpRequest request)
    {
        var (result, token) = await AccountManager
            .SignUpAsync(request, request.Password, CancellationToken);

        if (result.Succeeded)
            if (token is not null)
            {
                var replyToken = token.GenerateTokenKey();

                var cacheKey = $"{Options.CacheTokenPrefix}:{replyToken}";

                Cache.CacheToken(token, cacheKey, Options.Expire);

                return new TokenResponse
                {
                    Result = GrpcResponse.SuccessResult(),
                    Data = new TokenResult
                    {
                        Token = replyToken,
                        Expire = DateTime.Now.AddDays(30).ToUnixTimeStamp()
                    }
                };
            }

        return new TokenResponse
        {
            Result = GrpcResponse.FailResult(result.Message),
            Data = null
        };
    }

    /// <summary>
    ///     登出
    /// </summary>
    /// <returns></returns>
    [Authorize(IdentityPolicy.ActionName)]
    public Task<GrpcEmptyResponse> SignOutAsync()
    {
        var token = CurrentToken;

        throw new NotImplementedException();
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize(IdentityPolicy.Token)]
    public async Task<GrpcEmptyResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var token = CurrentToken;

        var result = await AccountManager.ChangePasswordAsync(
            request.UserSign,
            request.OldPassword,
            request.NewPassword, 
            CancellationToken);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize(IdentityPolicy.Admin)]
    public async Task<GrpcEmptyResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var result = await AccountManager.ResetPasswordAsync(
            request.UserId,
            request.Password,
            CancellationToken);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize(IdentityPolicy.Admin)]
    public async Task<GrpcEmptyResponse> ResetPasswordsAsync(ResetPasswordsRequest request)
    {
        var result = await AccountManager.ResetPasswordsAsync(
            request.UserIds,
            request.Password,
            CancellationToken);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    #endregion
}