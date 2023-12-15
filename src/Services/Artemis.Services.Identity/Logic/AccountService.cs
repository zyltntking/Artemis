using Artemis.Data.Core;
using Artemis.Data.Grpc;
using Artemis.Data.Store.Extensions;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Logic;

/// <summary>
///     账户服务
/// </summary>
public class AccountService : IAccountService
{
    /// <summary>
    ///     账户服务
    /// </summary>
    /// <param name="accountManager">账户管理器依赖</param>
    /// <param name="cache">分布式缓存依赖</param>
    public AccountService(
        IAccountManager accountManager,
        IDistributedCache cache)
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
    public async Task<TokenResponse> SignInAsync(SignInRequest request)
    {
        var (result, token) = await AccountManager.SignInAsync(request);

        if (result.Succeeded)
        {
            // todo cache token
            var json = token!.Serialize();

            var replyToken = Hash.Md5Hash(json);

            await Cache.SetAsync(replyToken, token!);

            var tokenResult = new TokenResult
            {
                Token = replyToken,
                Expire = DateTime.Now.AddDays(30).ToUnixTimeStamp()
            };

            return new TokenResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = tokenResult
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
    public async Task<TokenResponse> SignUpAsync(SignUpRequest request)
    {
        var (result, token) = await AccountManager.SignUpAsync(request, request.Password);

        if (result.Succeeded)
        {
            // todo cache token

            var json = token!.Serialize();

            var replyToken = Hash.Md5Hash(json);

            var tokenResult = new TokenResult
            {
                Token = replyToken,
                Expire = DateTime.Now.AddDays(30).ToUnixTimeStamp()
            };

            return new TokenResponse
            {
                Result = GrpcResponse.SuccessResult(),
                Data = tokenResult
            };
        }

        return new TokenResponse
        {
            Result = GrpcResponse.FailResult(result.Message),
            Data = null
        };
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [Authorize("Token")]
    public async Task<GrpcEmptyResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var result = await AccountManager.ChangePasswordAsync(
            request.UserSign,
            request.OldPassword,
            request.NewPassword);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var result = await AccountManager.ResetPasswordAsync(
            request.UserId,
            request.Password);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    public async Task<GrpcEmptyResponse> ResetPasswordsAsync(ResetPasswordsRequest request)
    {
        var result = await AccountManager.ResetPasswordsAsync(
            request.UserIds,
            request.Password);

        return result.Succeeded
            ? GrpcResponse.EmptySuccess()
            : GrpcResponse.EmptyFail(result.Message);
    }

    #endregion
}