using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Grpc.Core;

namespace Artemis.App.Business.VisionScreen.Services;

/// <summary>
/// 微信家长终端服务实现
/// </summary>
public class WxParentTerminalServiceImplement : WxParentTerminalService.WxParentTerminalServiceBase
{
    #region Overrides of WxParentTerminalServiceBase

    /// <summary>
    /// 绑定孩子关系
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<EmptyResponse> BindChildRelation(BindChildRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EmptyResponse());
    }

    /// <summary>
    /// 反馈筛查记录后续处理
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<EmptyResponse> FeedbackRecord(FeedbackRecordRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EmptyResponse());
    }

    /// <summary>
    /// 获取个人中心孩子信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<FetchChildInfoResponse> FetchChildrenInfo(EmptyRequest request, ServerCallContext context)
    {
        var response = new FetchChildInfoResponse();
        var childInfo = Activator.CreateInstance<ChildInfo>();
        response.Data.Add(childInfo);
        response.Data.Add(childInfo);
        response.Data.Add(childInfo);

        return Task.FromResult(response);
    }

    /// <summary>
    /// 获取个人中心消息信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<FetchParentUserMessageInfoResponse> FetchUserMessageInfo(EmptyRequest request, ServerCallContext context)
    {
        var response = new FetchParentUserMessageInfoResponse();

        var not = Activator.CreateInstance<NotFeedbackRecordPacket>();

        response.Data.NotFeedbackRecords.Add(new NotFeedbackRecordPacket());
        response.Data.NotFeedbackRecords.Add(new NotFeedbackRecordPacket());
        response.Data.NotFeedbackRecords.Add(new NotFeedbackRecordPacket());

        response.Data.Notifications.Add(new NotificationPacket());
        response.Data.Notifications.Add(new NotificationPacket());
        response.Data.Notifications.Add(new NotificationPacket());

        return Task.FromResult(response);
    }

    /// <summary>
    /// 查询孩子信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<QueryChildResponse> QueryChild(QueryChildRequest request, ServerCallContext context)
    {
        var response = new QueryChildResponse();

        response.Data = new ChildInfoPacket();

        return Task.FromResult(response);
    }

    /// <summary>
    /// 读取筛查结果
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<ReadScreenRecordResponse> ReadScreenRecord(ReadScreenRecordRequest request, ServerCallContext context)
    {
        var response = new ReadScreenRecordResponse();
        response.Data = new ScreenRecordPacket();

        return Task.FromResult(response);
    }

    /// <summary>
    /// 上传孩子眼部照片
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    public override Task<EmptyResponse> UploadChildEyePhoto(UploadChildEyePhotoRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EmptyResponse());
    }


    #endregion
}