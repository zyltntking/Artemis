using Artemis.Protos.Identity;
using Grpc.Core;

namespace Artemis.App.Identity.Services;

/// <summary>
/// 用户服务
/// </summary>
public class UserService : User.UserBase
{
    #region Overrides of UserBase

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<UserInfosResponse> GetUserInfo(FetchUserInfosRequest request, ServerCallContext context)
    {
        return base.GetUserInfo(request, context);
    }

    #endregion
}