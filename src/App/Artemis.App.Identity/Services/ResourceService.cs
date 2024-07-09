using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Identity.Resource;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Artemis.App.Identity.Services;

/// <summary>
///     资源服务
/// </summary>
public class ResourceService : Resource.ResourceBase
{
    /// <summary>
    ///     资源服务
    /// </summary>
    /// <param name="resourceManager">认证资源管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    public ResourceService(
        IIdentityResourceManager resourceManager,
        ILogger<ResourceService> logger)
    {
        ResourceManager = resourceManager;
        Logger = logger;
    }

    /// <summary>
    ///     资源管理器
    /// </summary>
    private IIdentityResourceManager ResourceManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<ResourceService> Logger { get; }

    #region Overrides of ResourceBase

    /// <summary>
    ///     查询凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Authorize(IdentityPolicy.Token)]
    public override async Task<SearchClaimInfoResponse> SearchClaimInfo(SearchClaimInfoRequest request,
        ServerCallContext context)
    {
        var claimInfos = await ResourceManager.FetchClaimsAsync(
            request.ClaimType,
            request.ClaimValue,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        var claimPackets = claimInfos.Items!.Select(item => item.Adapt<ClaimInfoPacket>());

        var response = DataResult.Success(claimInfos).Adapt<SearchClaimInfoResponse>();

        response.Data.Items.Add(claimPackets);

        return response;
    }

    /// <summary>
    ///     读取凭据信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Authorize(IdentityPolicy.Token)]
    public override async Task<ReadClaimInfoResponse> ReadClaimInfo(ReadClaimInfoRequest request,
        ServerCallContext context)
    {
        var claimId = Guid.Parse(request.ClaimId);

        var claimInfo = await ResourceManager.GetClaimAsync(claimId, context.CancellationToken);

        if (claimInfo is null) return DataResult.Fail<ClaimInfoPacket>().Adapt<ReadClaimInfoResponse>();

        return DataResult.Success(claimInfo).Adapt<ReadClaimInfoResponse>();
    }

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Authorize(IdentityPolicy.Token)]
    public override async Task<AffectedResponse> CreateClaim(CreateClaimRequest request, ServerCallContext context)
    {
        var result = await ResourceManager.CreateClaimAsync(
            new ClaimPackage
            {
                ClaimType = request.ClaimType,
                ClaimValue = request.ClaimValue,
                Description = request.Description
            },
            context.CancellationToken);

        if (result.Succeeded)
        {
            var response = DataResult.EmptySuccess().Adapt<AffectedResponse>();
            response.Data = result.AffectRows;
            return response;
        }

        return await base.CreateClaim(request, context);
    }

    #endregion
}