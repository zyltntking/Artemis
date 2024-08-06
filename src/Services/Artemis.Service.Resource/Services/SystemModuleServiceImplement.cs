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
/// 系统模块服务实现
/// </summary>
public class SystemModuleServiceImplement : SystemModuleService.SystemModuleServiceBase
{
    /// <summary>
    ///     服务实现
    /// </summary>
    /// <param name="systemModuleTerrManager"></param>
    /// <param name="logger"></param>
    public SystemModuleServiceImplement(
        ISystemModuleTreeManager systemModuleTerrManager,
        ILogger<SystemModuleServiceImplement> logger)
    {
        SystemModuleTreeManager = systemModuleTerrManager;
        Logger = logger;
    }

    /// <summary>
    ///     数据字典管理器
    /// </summary>
    private ISystemModuleTreeManager SystemModuleTreeManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<SystemModuleServiceImplement> Logger { get; }


    #region Overrides of SystemModuleServiceBase

    /// <summary>
    /// 查询系统模块信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询系统模块信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchSystemModuleInfoResponse> SearchSystemModuleInfo(SearchSystemModuleInfoRequest request, ServerCallContext context)
    {
        var info = await SystemModuleTreeManager.FetchSystemModulesAsync(
            request.Name,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchSystemModuleInfoResponse, SystemModuleInfo>();
    }

    /// <summary>
    /// 读取系统模块信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取系统模块信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadSystemModuleInfoResponse> ReadSystemModuleInfo(ReadSystemModuleInfoRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var info = await SystemModuleTreeManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadSystemModuleInfoResponse, SystemModuleInfo>();
    }

    /// <summary>
    /// 读取系统模块信息树
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取系统模块信息树")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadSystemModuleInfoTreeResponse> ReadSystemModuleInfoTree(ReadSystemModuleInfoRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var info = await SystemModuleTreeManager.GetEntityInfoTreeAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadSystemModuleInfoTreeResponse, SystemModuleInfo>();
    }

    /// <summary>
    /// 创建系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateSystemModule(CreateSystemModuleRequest request, ServerCallContext context)
    {
        var package = request.Adapt<SystemModulePackage>();

        var result = await SystemModuleTreeManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateSystemModule(BatchCreateSystemModuleRequest request, ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<SystemModulePackage>>();

        var result = await SystemModuleTreeManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 更新系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateSystemModule(UpdateSystemModulRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var package = request.SystemModule.Adapt<SystemModulePackage>();

        var result = await SystemModuleTreeManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量更新系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateSystemModule(BatchUpdateSystemModuleRequest request, ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.SystemModuleId),
            item => item.SystemModule.Adapt<SystemModulePackage>());

        var result = await SystemModuleTreeManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteSystemModule(DeleteSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var result = await SystemModuleTreeManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteSystemModule(BatchDeleteSystemModuleRequest request, ServerCallContext context)
    {
        var ids = request.SystemModuleIds.Select(Guid.Parse);

        var result = await SystemModuleTreeManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 创建下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateChildSystemModule(CreateChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var package = request.ChildSubSystemModule.Adapt<SystemModulePackage>();

        var result = await SystemModuleTreeManager.CreateChildEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量创建下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateChildSystemModule(BatchCreateChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var packages = request.Batch.Adapt<IEnumerable<SystemModulePackage>>();

        var result = await SystemModuleTreeManager.BatchCreateChildEntityAsync(id, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 添加下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("添加下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> AddChildSystemModule(AddChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childId = Guid.Parse(request.ChildSystemModule.ChildSystemModuleId);

        var result = await SystemModuleTreeManager.AddChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量添加下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量添加下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchAddChildSystemModule(BatchAddChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childIds = request.ChildSystemModuleIds.Select(Guid.Parse);

        var result = await SystemModuleTreeManager.BatchAddChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 删除下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteChildSystemModule(DeleteChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childId = Guid.Parse(request.ChildSystemModuleId);

        var result = await SystemModuleTreeManager.DeleteChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量删除下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteChildSystemModule(BatchDeleteChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childIds = request.ChildSystemModuleIds.Select(Guid.Parse);

        var result = await SystemModuleTreeManager.BatchDeleteChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 移除下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("移除下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> RemoveChildSystemModule(RemoveChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childId = Guid.Parse(request.ChildSystemModule.ChildSystemModuleId);

        var result = await SystemModuleTreeManager.RemoveChildEntityAsync(id, childId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 批量移除下级系统模块
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量移除下级系统模块")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchRemoveChildSystemModule(BatchRemoveChildSystemModuleRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SystemModuleId);

        var childIds = request.ChildSystemModuleIds.Select(Guid.Parse);

        var result = await SystemModuleTreeManager.BatchRemoveChildEntityAsync(id, childIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}