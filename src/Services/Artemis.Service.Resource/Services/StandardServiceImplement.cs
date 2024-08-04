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
///     标准服务实现
/// </summary>
public class StandardServiceImplement : StandardService.StandardServiceBase
{
    /// <summary>
    ///     服务实现
    /// </summary>
    /// <param name="standardManager"></param>
    /// <param name="logger"></param>
    public StandardServiceImplement(
        IStandardManager standardManager,
        ILogger<StandardServiceImplement> logger)
    {
        StandardManager = standardManager;
        Logger = logger;
    }

    /// <summary>
    ///     数据字典管理器
    /// </summary>
    private IStandardManager StandardManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<StandardServiceImplement> Logger { get; }

    #region Overrides of StandardServiceBase

    /// <summary>
    /// 查询标准目录信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询标准目录信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchStandardCatalogInfoResponse> SearchStandardCatalogInfo(SearchStandardCatalogInfoRequest request, ServerCallContext context)
    {
        var info = await StandardManager.FetchStandardCatalogsAsync(
            request.Name,
            request.Code,
            request.Type,
            request.State,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchStandardCatalogInfoResponse, StandardCatalogInfo>();
    }

    /// <summary>
    /// 读取标准目录信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取标准目录信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadStandardCatalogInfoResponse> ReadStandardCatalogInfo(ReadStandardCatalogInfoRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);

        var info = await StandardManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadStandardCatalogInfoResponse, StandardCatalogInfo>();
    }

    /// <summary>
    /// 创建标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateStandardCatalog(CreateStandardCatalogRequest request, ServerCallContext context)
    {
        var package = request.Adapt<StandardCatalogPackage>();

        var result = await StandardManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateStandardCatalog(BatchCreateStandardCatalogRequest request, ServerCallContext context)
    {
        var package = request.Adapt<IEnumerable<StandardCatalogPackage>>();

        var result = await StandardManager.BatchCreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateStandardCatalog(UpdateStandardCatalogRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);

        var package = request.Adapt<StandardCatalogPackage>();

        var result = await StandardManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateStandardCatalog(BatchUpdateStandardCatalogRequest request, ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.StandardCatalogId),
            item => item.StandardCatalog.Adapt<StandardCatalogPackage>());

        var result = await StandardManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteStandardCatalog(DeleteStandardCatalogRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);

        var result = await StandardManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除标准目录
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除标准目录")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteStandardCatalog(BatchDeleteStandardCatalogRequest request, ServerCallContext context)
    {
        var ids = request.StandardCatalogIds.Select(Guid.Parse);

        var result = await StandardManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 查询标准项目信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询标准项目信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchStandardItemInfoResponse> SearchStandardItemInfo(SearchStandardItemInfoRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);

        var info = await StandardManager.FetchStandardItemsAsync(
            id,
            request.Name,
            request.Code,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchStandardItemInfoResponse, StandardItemInfo>();
    }

    /// <summary>
    /// 读取标准项目信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取标准项目信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadStandardItemInfoResponse> ReadStandardItemInfo(ReadStandardItemInfoRequest request, ServerCallContext context)
    {
        var catalogId = Guid.Parse(request.StandardCatalogId);
        var itemId = Guid.Parse(request.StandardItemId);

        var info = await StandardManager.ReadSubEntityInfoAsync(catalogId, itemId, context.CancellationToken);

        return info.ReadInfoResponse<ReadStandardItemInfoResponse, StandardItemInfo>();
    }

    /// <summary>
    /// 创建标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateStandardItem(CreateStandardItemRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);
        var package = request.Adapt<StandardItemPackage>();

        var result = await StandardManager.CreateSubEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateStandardItem(BatchCreateStandardItemRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);
        var package = request.Batch.Select(item => item.Adapt<StandardItemPackage>());

        var result = await StandardManager.BatchCreateSubEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateStandardItem(UpdateStandardItemRequest request, ServerCallContext context)
    {
        var catalogId = Guid.Parse(request.StandardCatalogId);
        var itemId = Guid.Parse(request.StandardItemId);

        var package = request.Adapt<StandardItemPackage>();

        var result = await StandardManager.UpdateSubEntityAsync(catalogId, itemId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateStandardItem(BatchUpdateStandardItemRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StandardCatalogId);

        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.StandardItemId),
            item => item.StandardItem.Adapt<StandardItemPackage>());

        var result = await StandardManager.BatchUpdateSubEntityAsync(id, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteStandardItem(DeleteStandardItemRequest request, ServerCallContext context)
    {
        var catalogId = Guid.Parse(request.StandardCatalogId);
        var itemId = Guid.Parse(request.StandardItemId);

        var result = await StandardManager.DeleteSubEntityAsync(catalogId, itemId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除标准项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除标准项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteStandardItem(BatchDeleteStandardItemRequest request, ServerCallContext context)
    {
        var catalogId = Guid.Parse(request.StandardCatalogId);

        var itemIds = request.StandardItemIds.Select(Guid.Parse);

        var result = await StandardManager.BatchDeleteSubEntityAsync(catalogId, itemIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}