using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity.Managers;
using Artemis.Shared.Identity.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services;

/// <summary>
///     账户服务
/// </summary>
[Route("api/Account")]
public class AccountService : ApiController, IAccountService
{
    /// <summary>
    /// 账户服务
    /// </summary>
    /// <param name="logger">日志依赖</param>
    /// <param name="accountManager">账户管理器依赖</param>
    public AccountService(
        ILogger<AccountService> logger,
        IAccountManager accountManager) : base(logger)
    {
        AccountManager = accountManager;
    }

    /// <summary>
    ///     角色管理器
    /// </summary>
    private IAccountManager AccountManager { get; }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [HttpPost(nameof(SignIn))]
    public Task<DataResult<TokenResult>> SignIn(SignInRequest request)
    {
        return SignInAsync(request);
    }

    #region Implementation of IAccountService

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<TokenResult>> SignInAsync(
        SignInRequest request, 
        ServerCallContext? context = default)
    {
        var (result, token) = await AccountManager
            .SignInAsync(
                request, 
                context?.CancellationToken ?? default);

        if (result.Succeeded)
        {
            // todo cache token

            var json = token!.Serialize();

            var replyToken = Hash.Md5Hash(json);

            var tokenResult = new TokenResult
            {
                Token = replyToken,
                Expire = DateTime.Now.AddDays(30)
            };

            return DataResult.Success(tokenResult);
        }

        return DataResult.Fail<TokenResult>(result.Message);
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<TokenResult>> SignUpAsync(
        SignUpRequest request, 
        ServerCallContext? context = default)
    {
        var (result, token) = await AccountManager
            .SignUpAsync(
                request, 
                request.Password, 
                context?.CancellationToken ?? default);

        if (result.Succeeded)
        {
            // todo cache token

            var json = token!.Serialize();

            var replyToken = Hash.Md5Hash(json);

            var tokenResult = new TokenResult
            {
                Token = replyToken,
                Expire = DateTime.Now.AddDays(30)
            };

            return DataResult.Success(tokenResult);
        }

        return DataResult.Fail<TokenResult>(result.Message);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> ChangePasswordAsync(
        ChangePasswordRequest request, 
        ServerCallContext? context = default)
    {
        var result = await AccountManager
            .ChangePasswordAsync(
                request.UserSign, 
                request.OldPassword,
                request.NewPassword,
                context?.CancellationToken ?? default);

        return result.Succeeded ? 
            DataResult.Success(new EmptyRecord()) : 
            DataResult.Fail<EmptyRecord>(result.Message);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> ResetPasswordAsync(
        ResetPasswordRequest request, 
        ServerCallContext? context = default)
    {
        var result = await AccountManager
            .ResetPasswordAsync(
                request.UserId, 
                request.Password,
                context?.CancellationToken ?? default);

        return result.Succeeded ? 
            DataResult.Success(new EmptyRecord()) : 
            DataResult.Fail<EmptyRecord>(result.Message);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <param name="context">上下文</param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataResult<EmptyRecord>> ResetPasswordsAsync(
        ResetPasswordsRequest request, 
        ServerCallContext? context = default)
    {
        var result = await AccountManager
            .ResetPasswordsAsync(
                request.UserIds, 
                request.Password,
                context?.CancellationToken ?? default);

        return result.Succeeded ?
            DataResult.Success(new EmptyRecord()) :
            DataResult.Fail<EmptyRecord>(result.Message);
    }

    #endregion
}