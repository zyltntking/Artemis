using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Extensions.Web.Identity;
using Artemis.Protos.Identity;
using Artemis.Services.Identity.Managers;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.App.Identity.Services;

/// <summary>
///     用户服务
/// </summary>
public class UserService : User.UserBase
{
    /// <summary>
    ///     用户服务
    /// </summary>
    /// <param name="userManager">用户管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    public UserService(
        IUserManager userManager,
        ILogger<UserService> logger)
    {
        UserManager = userManager;
        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IUserManager UserManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<UserService> Logger { get; }

    #region Overrides of UserBase

    /// <summary>
    ///     获取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取用户信息")]
    [Authorize(IdentityPolicy.Token)]
    public override async Task<FetchUserInfoResponse> FetchUserInfo(FetchUserInfosRequest request,
        ServerCallContext context)
    {
        var pagedUserInfos = await UserManager.FetchUserAsync(
            request.NameSearch,
            request.EmailSearch,
            request.PhoneNumberSearch,
            request.Page ?? 1,
            request.Size ?? 20,
            context.CancellationToken);

        var userReplies = pagedUserInfos.Items.Adapt<List<UserInfoReply>>();

        var response = DataResult.Success(pagedUserInfos).Adapt<FetchUserInfoResponse>();

        response.Data.Items.Add(userReplies);

        return response;
    }

    #endregion
}