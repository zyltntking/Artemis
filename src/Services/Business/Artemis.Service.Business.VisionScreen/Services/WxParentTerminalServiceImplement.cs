using Artemis.Data.Core;
using Artemis.Data.Store.Extensions;
using Artemis.Extensions.Identity;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Business.VisionScreen.Stores;
using Artemis.Service.Identity.Stores;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Artemis.Service.School.Stores;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Services;

/// <summary>
/// 家长端服务实现
/// </summary>
public class WxParentTerminalServiceImplement : WxParentTerminalService.WxParentTerminalServiceBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userStore"></param>
    /// <param name="userClaimStore"></param>
    /// <param name="studentStore"></param>
    /// <param name="studentRelationBindingStore"></param>
    public WxParentTerminalServiceImplement(
        IIdentityUserStore userStore, 
        IIdentityClaimStore userClaimStore,
        IArtemisStudentStore studentStore,
        IArtemisStudentRelationBindingStore studentRelationBindingStore)
    {
        UserStore = userStore;
        StudentStore = studentStore;
        StudentRelationBindingStore = studentRelationBindingStore;
    }

    /// <summary>
    /// 用户存储
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    /// 学生存储
    /// </summary>
    private IArtemisStudentStore StudentStore { get; }

    /// <summary>
    /// 用户学生亲属关系绑定存储
    /// </summary>
    private IArtemisStudentRelationBindingStore StudentRelationBindingStore { get; }

    #region Overrides of WxParentTerminalServiceBase

    /// <summary>
    /// 绑定孩子关系
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BindChildRelation(BindChildRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户不存在");
        }

        var studentId = Guid.Parse(request.StudentId);

        var studentExists = await StudentStore.ExistsAsync(studentId, context.CancellationToken);

        if (!studentExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("学生不存在");
        }

        var bindingExists = await StudentRelationBindingStore
            .EntityQuery
            .Where(item => item.StudentId == studentId && item.Relation == request.Relation)
            .AnyAsync(context.CancellationToken);

        if (bindingExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>($"该学生已经绑定过关系{request.Relation}");
        }

        var binding = await StudentRelationBindingStore.EntityQuery
            .Where(binding => binding.UserId == userId && binding.StudentId == studentId)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (binding is not null)
        {
            binding.Relation = request.Relation;
            var updateResult = await StudentRelationBindingStore.UpdateAsync(binding, context.CancellationToken);
            return updateResult.AffectedResponse();
        }

        binding = Instance.CreateInstance<ArtemisStudentRelationBinding>();
        binding.UserId = userId;
        binding.StudentId = studentId;
        binding.Relation = request.Relation;
        var createResult = await StudentRelationBindingStore.CreateAsync(binding, context.CancellationToken);
        return createResult.AffectedResponse();
    }

    #endregion
}