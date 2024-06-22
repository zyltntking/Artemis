using Artemis.Data.Core;
using Artemis.Protos.Identity;
using Artemis.Service.Identity.Managers;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace Artemis.App.Identity.Services;

/// <summary>
///     用户服务
/// </summary>
[Route("api")]
public class UserService : User.UserBase
{
    /// <summary>
    ///     用户管理器
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="logger"></param>
    public UserService(IIdentityUserManager userManager, ILogger<UserService> logger)
    {
        UserManager = userManager;
        UserManager.HandlerRegister = Guid.NewGuid;

        Logger = logger;
    }

    /// <summary>
    ///     用户管理器
    /// </summary>
    private IIdentityUserManager UserManager { get; }

    /// <summary>
    ///     日志
    /// </summary>
    private ILogger<UserService> Logger { get; }

    #region Overrides of UserBase

    /// <summary>
    ///     获取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override async Task<UserInfoSearchResponse> FetchUserInfo(UserInfoSearchPackage request,
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

        var response = DataResult.Success(pagedUserInfos).Adapt<UserInfoSearchResponse>();

        response.Data.Items.AddRange(userReplies);

        return response;
    }

    #endregion
}