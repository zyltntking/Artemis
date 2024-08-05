﻿using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Shared.Identity.Transfer;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证资源管理接口
/// </summary>
public interface IIdentityClaimManager : IManager
{
    #region IdentityClaim

    /// <summary>
    ///     根据凭据信息搜索凭据
    /// </summary>
    /// <param name="claimTypeSearch">凭据类型搜索值</param>
    /// <param name="claimValueSearch">凭据值搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<ClaimInfo>> FetchClaimsAsync(
        string? claimTypeSearch,
        string? claimValueSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据凭据标识获取凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>凭据信息</returns>
    Task<ClaimInfo?> GetClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="package">凭据</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateClaimAsync(
        ClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateClaimsAsync(
        IEnumerable<ClaimPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新成功的凭据信息</returns>
    Task<StoreResult> UpdateClaimAsync(
        Guid id,
        ClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新凭据
    /// </summary>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<StoreResult> UpdateClaimsAsync(
        IDictionary<Guid, ClaimPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    Task<StoreResult> CreateOrUpdateClaimAsync(
        Guid id,
        ClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除凭据
    /// </summary>
    /// <param name="ids">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteClaimsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    #endregion
}