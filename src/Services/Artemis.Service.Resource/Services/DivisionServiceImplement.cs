using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Resource;
using Artemis.Service.Resource.Managers;
using Artemis.Service.Shared.Resource.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Resource.Services;

/// <summary>
///     行政区划服务实现
/// </summary>
public class DivisionServiceImplement : DivisionService.DivisionServiceBase
{
    /// <summary>
    ///     行政区划服务实现
    /// </summary>
    /// <param name="divisionTreeManager"></param>
    /// <param name="logger"></param>
    public DivisionServiceImplement(
        IDivisionTreeManager divisionTreeManager,
        ILogger<DivisionServiceImplement> logger)
    {
        DivisionTreeManager = divisionTreeManager;
        Logger = logger;
    }

    /// <summary>
    ///     任务树管理器
    /// </summary>
    private IDivisionTreeManager DivisionTreeManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<DivisionServiceImplement> Logger { get; }

    #region Overrides of DivisionServiceBase

    /// <summary>
    ///     查询行政区划信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询行政区划信息")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<SearchDivisionInfoResponse> SearchDivisionInfo(SearchDivisionInfoRequest request,
        ServerCallContext context)
    {
        var info = await DivisionTreeManager.FetchDivisionsAsync(
            request.Name,
            request.Code,
            request.Level,
            request.Type,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchDivisionInfoResponse, DivisionInfo>();
    }

    /// <summary>
    ///     读取行政区划信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取行政区划信息")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadDivisionInfoResponse> ReadDivisionInfo(ReadDivisionInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var info = await DivisionTreeManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadDivisionInfoResponse, DivisionInfo>();
    }

    /// <summary>
    ///     读取行政区划信息树
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取行政区划信息树")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<ReadDivisionInfoTreeResponse> ReadDivisionInfoTree(ReadDivisionInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var info = await DivisionTreeManager.GetEntityInfoTreeAsync(id, context.CancellationToken);

        var response = info.ReadInfoResponse<ReadDivisionInfoResponse, DivisionInfo>();

        return info.ReadInfoResponse<ReadDivisionInfoTreeResponse, DivisionInfoTree>();
    }

    /// <summary>
    ///     创建行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateDivision(CreateDivisionRequest request,
        ServerCallContext context)
    {
        var package = request.Adapt<DivisionPackage>();

        var result = await DivisionTreeManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateDivision(BatchCreateDivisionRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<DivisionPackage>>();

        var result = await DivisionTreeManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateDivision(UpdateDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var package = request.Adapt<DivisionPackage>();

        var result = await DivisionTreeManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateDivision(BatchUpdateDivisionRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.DivisionId),
            item => item.Adapt<DivisionPackage>());

        var result = await DivisionTreeManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteDivision(DeleteDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var result = await DivisionTreeManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteDivision(BatchDeleteDivisionRequest request,
        ServerCallContext context)
    {
        var ids = request.DivisionIds.Select(Guid.Parse);

        var result = await DivisionTreeManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     创建下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateChildDivision(CreateChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var package = request.Adapt<DivisionPackage>();

        var result = await DivisionTreeManager.CreateChildEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateChildDivision(BatchCreateChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var packages = request.Batch.Adapt<IEnumerable<DivisionPackage>>();

        var result = await DivisionTreeManager.BatchCreateChildEntityAsync(id, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     添加下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddChildDivision(AddChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childId = Guid.Parse(request.ChildDivision.ChildDivisionId);

        var result = await DivisionTreeManager.AddChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量添加下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddChildDivision(BatchAddChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childIds = request.ChildDivisionIds.Select(Guid.Parse);

        var result = await DivisionTreeManager.BatchAddChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteChildDivision(DeleteChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childId = Guid.Parse(request.ChildDivisionId);

        var result = await DivisionTreeManager.DeleteChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }


    /// <summary>
    ///     批量删除下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteChildDivision(BatchDeleteChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childIds = request.ChildDivisionIds.Select(Guid.Parse);

        var result = await DivisionTreeManager.BatchDeleteChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     移除下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> RemoveChildDivision(RemoveChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childId = Guid.Parse(request.ChildDivision.ChildDivisionId);

        var result = await DivisionTreeManager.RemoveChildEntityAsync(id, childId);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量移除下级行政区划
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除下级行政区划")]
    //[Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchRemoveChildDivision(BatchRemoveChildDivisionRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DivisionId);

        var childIds = request.ChildDivisionIds.Select(Guid.Parse);

        var result = await DivisionTreeManager.BatchRemoveChildEntityAsync(id, childIds);

        return result.AffectedResponse();
    }

    #endregion
}