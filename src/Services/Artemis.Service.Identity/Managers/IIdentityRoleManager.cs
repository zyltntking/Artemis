using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证角色管理接口
/// </summary>
public interface IIdentityRoleManager : IManager<IdentityRole>
{
    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<RoleInfo>> FetchRolesAsync(
        string? nameSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色标识获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<RoleInfo?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    Task<(StoreResult result, RoleInfo? role)> CreateRoleAsync(
        RolePackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="packages">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateRolesAsync(
        IEnumerable<RolePackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<(StoreResult result, RoleInfo? role)> UpdateRoleAsync(
        Guid id,
        RolePackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="dictionary">更新角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<StoreResult> UpdateRolesAsync(
        IDictionary<Guid, RolePackage> dictionary,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建或更新结果</returns>
    Task<(StoreResult result, RoleInfo? role)> CreateOrUpdateRoleAsync(
        Guid id,
        RolePackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="ids">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteRolesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据用户信息搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        Guid id,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户信息</returns>
    Task<UserInfo?> GetRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddRoleUsersAsync(
        Guid id,
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userIds">用户标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveRoleUsersAsync(
        Guid id,
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<RoleClaimInfo>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>角色凭据</returns>
    Task<RoleClaimInfo?> GetRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddRoleClaimAsync(
        Guid id,
        RoleClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="packages">凭据</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddRoleClaimsAsync(
        Guid id,
        IEnumerable<RoleClaimPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="dictionary">凭据更新字典</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleClaimsAsync(
        Guid id,
        IDictionary<int, RoleClaimPackage> dictionary,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimIds">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveRoleClaimsAsync(
        Guid id,
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken = default);
}