using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Identity;
using Artemis.Service.Shared.Identity.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Services;

/// <summary>
///     资源服务
/// </summary>
public class ResourceServiceImplement : ResourceService.ResourceServiceBase
{
    /// <summary>
    ///     资源服务
    /// </summary>
    /// <param name="claimManager">认证资源管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    public ResourceServiceImplement(
        IIdentityClaimManager claimManager,
        ILogger<ResourceServiceImplement> logger)
    {
        ClaimManager = claimManager;
        Logger = logger;
    }

    /// <summary>
    ///     资源管理器
    /// </summary>
    private IIdentityClaimManager ClaimManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<ResourceServiceImplement> Logger { get; }

    #region Overrides of ResourceBase

    /// <summary>
    ///     查询凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询凭据信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchClaimInfoResponse> SearchClaimInfo(SearchClaimInfoRequest request,
        ServerCallContext context)
    {
        var claimInfos = await ClaimManager.FetchClaimsAsync(
            request.ClaimType,
            request.ClaimValue,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return claimInfos.PagedResponse<SearchClaimInfoResponse, ClaimInfo>();
    }

    /// <summary>
    ///     读取凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取凭据信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadClaimInfoResponse> ReadClaimInfo(ReadClaimInfoRequest request,
        ServerCallContext context)
    {
        var claimId = Guid.Parse(request.ClaimId);

        var claimInfo = await ClaimManager.GetClaimAsync(claimId, context.CancellationToken);

        return claimInfo.ReadInfoResponse<ReadClaimInfoResponse, ClaimInfo>();
    }

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateClaim(CreateClaimRequest request, ServerCallContext context)
    {
        var package = request.Adapt<ClaimPackage>();

        var result = await ClaimManager.CreateClaimAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateClaim(BatchCreateClaimRequest request,
        ServerCallContext context)
    {
        var claims = request.Batch.Adapt<IEnumerable<ClaimPackage>>();

        var result = await ClaimManager.CreateClaimsAsync(claims, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateClaim(UpdateClaimRequest request, ServerCallContext context)
    {
        var claimId = Guid.Parse(request.ClaimId);

        var claimPackage = request.Claim.Adapt<ClaimPackage>();

        var result = await ClaimManager.UpdateClaimAsync(claimId, claimPackage, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateClaim(BatchUpdateClaimRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.ToDictionary(
            item => Guid.Parse(item.ClaimId),
            item => item.Claim.Adapt<ClaimPackage>());

        var result = await ClaimManager.UpdateClaimsAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteClaim(DeleteClaimRequest request, ServerCallContext context)
    {
        var claimId = Guid.Parse(request.ClaimId);

        var result = await ClaimManager.DeleteClaimAsync(claimId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除凭据")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteClaim(BatchDeleteClaimRequest request,
        ServerCallContext context)
    {
        var claimIds = request.ClaimIds.Select(Guid.Parse);

        var result = await ClaimManager.DeleteClaimsAsync(claimIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}