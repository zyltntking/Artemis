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
///     组织服务实现
/// </summary>
public class OrganizationServiceImplement : OrganizationService.OrganizationServiceBase
{
    /// <summary>
    ///     组织机构树服务实现
    /// </summary>
    /// <param name="organizationTreeManager"></param>
    /// <param name="logger"></param>
    public OrganizationServiceImplement(
        IOrganizationTreeManager organizationTreeManager,
        ILogger<DivisionServiceImplement> logger)
    {
        OrganizationTreeManager = organizationTreeManager;
        Logger = logger;
    }

    /// <summary>
    ///     任务树管理器
    /// </summary>
    private IOrganizationTreeManager OrganizationTreeManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<DivisionServiceImplement> Logger { get; }

    #region Overrides of OrganizationServiceBase

    /// <summary>
    ///     查找组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查找组织机构信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchOrganizationInfoResponse> SearchOrganizationInfo(
        SearchOrganizationInfoRequest request,
        ServerCallContext context)
    {
        var info = await OrganizationTreeManager.FetchOrganizationsAsync(
            request.Name,
            request.Code,
            request.Type,
            request.Status,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchOrganizationInfoResponse, OrganizationInfo>();
    }

    /// <summary>
    ///     读取组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取组织机构信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadOrganizationInfoResponse> ReadOrganizationInfo(ReadOrganizationInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var info = await OrganizationTreeManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadOrganizationInfoResponse, OrganizationInfo>();
    }

    /// <summary>
    ///     读取组织机构信息树
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取组织机构信息树")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadOrganizationInfoTreeResponse> ReadOrganizationInfoTree(
        ReadOrganizationInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var info = await OrganizationTreeManager.GetEntityInfoTreeAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadOrganizationInfoTreeResponse, OrganizationInfoTree>();
    }

    /// <summary>
    ///     创建组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建组织机构信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateOrganization(CreateOrganizationRequest request,
        ServerCallContext context)
    {
        var package = request.Adapt<OrganizationPackage>();

        var result = await OrganizationTreeManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建组织机构信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateOrganization(BatchCreateOrganizationRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<OrganizationPackage>>();

        var result = await OrganizationTreeManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新组织机构信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateOrganization(UpdateOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var package = request.Adapt<OrganizationPackage>();

        var result = await OrganizationTreeManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新组织机构信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新组织机构信息")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateOrganization(BatchUpdateOrganizationRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.OrganizationId),
            item => item.Organization.Adapt<OrganizationPackage>());

        var result = await OrganizationTreeManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteOrganization(DeleteOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var result = await OrganizationTreeManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteOrganization(BatchDeleteOrganizationRequest request,
        ServerCallContext context)
    {
        var ids = request.OrganizationIds.Select(Guid.Parse);

        var result = await OrganizationTreeManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     创建子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateChildOrganization(CreateChildOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var package = request.Adapt<OrganizationPackage>();

        var result = await OrganizationTreeManager.CreateChildEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateChildOrganization(
        BatchCreateChildOrganizationRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var packages = request.Batch.Adapt<IEnumerable<OrganizationPackage>>();

        var result = await OrganizationTreeManager.BatchCreateChildEntityAsync(id, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     添加子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> AddChildOrganization(AddChildOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childId = Guid.Parse(request.ChildOrganization.ChildOrganizationId);

        var result = await OrganizationTreeManager.AddChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量添加子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchAddChildOrganization(BatchAddChildOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childIds = request.ChildOrganizationIds.Select(Guid.Parse);

        var result = await OrganizationTreeManager.BatchAddChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteChildOrganization(DeleteChildOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childId = Guid.Parse(request.ChildOrganizationId);

        var result = await OrganizationTreeManager.DeleteChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteChildOrganization(
        BatchDeleteChildOrganizationRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childIds = request.ChildOrganizationIds.Select(Guid.Parse);

        var result = await OrganizationTreeManager.BatchDeleteChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     移除子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> RemoveChildOrganization(RemoveChildOrganizationRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childId = Guid.Parse(request.ChildOrganization.ChildOrganizationId);

        var result = await OrganizationTreeManager.RemoveChildEntityAsync(id, childId);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量移除子组织机构
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除子组织机构")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchRemoveChildOrganization(
        BatchRemoveChildOrganizationRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.OrganizationId);

        var childIds = request.ChildOrganizationIds.Select(Guid.Parse);

        var result = await OrganizationTreeManager.BatchRemoveChildEntityAsync(id, childIds);

        return result.AffectedResponse();
    }

    #endregion
}