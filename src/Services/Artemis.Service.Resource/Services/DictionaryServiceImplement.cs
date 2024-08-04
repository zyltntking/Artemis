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
///     字典服务实现
/// </summary>
public class DictionaryServiceImplement : DictionaryService.DictionaryServiceBase
{
    /// <summary>
    ///     服务实现
    /// </summary>
    /// <param name="dataDictionaryManager"></param>
    /// <param name="logger"></param>
    public DictionaryServiceImplement(
        IDataDictionaryManager dataDictionaryManager,
        ILogger<DictionaryServiceImplement> logger)
    {
        DataDictionaryManager = dataDictionaryManager;
        Logger = logger;
    }

    /// <summary>
    ///     数据字典管理器
    /// </summary>
    private IDataDictionaryManager DataDictionaryManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<DictionaryServiceImplement> Logger { get; }

    #region Overrides of DictionaryServiceBase

    /// <summary>
    ///     查询字典信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询字典信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchDictionaryInfoResponse> SearchDictionaryInfo(
        SearchDictionaryInfoRequest request,
        ServerCallContext context)
    {
        var info = await DataDictionaryManager.FetchDictionariesAsync(
            request.Name,
            request.Type,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchDictionaryInfoResponse, DataDictionaryInfo>();
    }

    /// <summary>
    ///     读取数据字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取数据字典")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadDictionaryInfoResponse> ReadDictionaryInfo(ReadDictionaryInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var info = await DataDictionaryManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadDictionaryInfoResponse, DataDictionaryInfo>();
    }

    /// <summary>
    ///     创建数据字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建数据字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateDictionary(CreateDictionaryRequest request,
        ServerCallContext context)
    {
        var package = request.Adapt<DataDictionaryPackage>();

        var result = await DataDictionaryManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建数据字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建数据字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateDictionary(BatchCreateDictionariyRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<DataDictionaryPackage>>();

        var result = await DataDictionaryManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新数据字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新数据字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateDictionary(UpdateDictionaryRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var package = request.Dictionary.Adapt<DataDictionaryPackage>();

        var result = await DataDictionaryManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新数据字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新数据字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateDictionary(BatchUpdateDictionaryRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.DictionaryId),
            item => item.Dictionary.Adapt<DataDictionaryPackage>());

        var result = await DataDictionaryManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteDictionary(DeleteDictionaryRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var result = await DataDictionaryManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除字典
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除字典")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteDictionary(BatchDeleteDictionaryRequest request,
        ServerCallContext context)
    {
        var ids = request.DictionaryIds.Select(Guid.Parse);

        var result = await DataDictionaryManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     查询字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询字典项目")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchDictionaryItemInfoResponse> SearchDictionaryItemInfo(
        SearchDictionaryItemInfoRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var info = await DataDictionaryManager
            .FetchDictionaryItemsAsync(id, request.Key, request.Value, request.Page ?? 0, request.Size ?? 0,
                context.CancellationToken);

        return info.PagedResponse<SearchDictionaryItemInfoResponse, DataDictionaryItemInfo>();
    }

    /// <summary>
    ///     读取字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取字典项目")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadDictionaryItemInfoResponse> ReadDictionaryItemInfo(
        ReadDictionaryItemInfoRequest request, ServerCallContext context)
    {
        var dictionaryId = Guid.Parse(request.DictionaryId);

        var dictionaryItemId = Guid.Parse(request.DictionaryItemId);

        var info = await DataDictionaryManager.ReadSubEntityInfoAsync(
            dictionaryId,
            dictionaryItemId,
            context.CancellationToken);

        return info.ReadInfoResponse<ReadDictionaryItemInfoResponse, DataDictionaryItemInfo>();
    }

    /// <summary>
    ///     创建字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateDictionaryItem(CreateDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var package = request.Adapt<DataDictionaryItemPackage>();

        var result = await DataDictionaryManager.CreateSubEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateDictionaryItem(BatchCreateDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var packages = request.Batch.Adapt<IEnumerable<DataDictionaryItemPackage>>();

        var result = await DataDictionaryManager.BatchCreateSubEntityAsync(id, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateDictionaryItem(UpdateDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var itemId = Guid.Parse(request.DictionaryItemId);

        var package = request.DictionaryItem.Adapt<DataDictionaryItemPackage>();

        var result = await DataDictionaryManager.UpdateSubEntityAsync(id, itemId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateDictionaryItem(BatchUpdateDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.DictionaryItemId),
            item => item.DictionaryItem.Adapt<DataDictionaryItemPackage>());

        var result = await DataDictionaryManager.BatchUpdateSubEntityAsync(id, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteDictionaryItem(DeleteDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var itemId = Guid.Parse(request.DictionaryItemId);

        var result = await DataDictionaryManager.DeleteSubEntityAsync(id, itemId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除字典项目
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除字典项目")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteDictionaryItem(BatchDeleteDictionaryItemRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.DictionaryId);

        var itemIds = request.DictionaryItemIds.Select(Guid.Parse);

        var result = await DataDictionaryManager.BatchDeleteSubEntityAsync(id, itemIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}