using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证资源管理接口
/// </summary>
public interface IIdentityResourceManager : IManager<IdentityClaim>
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
    ///     根据凭据类型获取凭据列表
    /// </summary>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<List<ClaimInfo>> GetClaimsAsync(
        string? claimTypeSearch,
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
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果和创建成功的凭据实例</returns>
    Task<(StoreResult result, ClaimInfo? claim)> CreateClaimAsync(
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
    Task<(StoreResult result, ClaimInfo? claim)> UpdateClaimAsync(
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
        IEnumerable<KeyValuePair<Guid, ClaimPackage>> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新凭据
    /// </summary>
    /// <param name="id">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    Task<(StoreResult result, ClaimInfo? role)> CreateOrUpdateClaimAsync(
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