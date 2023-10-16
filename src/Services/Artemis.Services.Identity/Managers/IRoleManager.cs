using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     角色管理器接口
/// </summary>
public interface IRoleManager : IManager<ArtemisRole>
{
    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken"></param>
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
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<RoleInfo?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    Task<(StoreResult result, RoleInfo? role)> CreateRoleAsync(
        RolePackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, RoleInfo? role)> UpdateRoleAsync(
        Guid id,
        RolePackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, RoleInfo? role)> CreateOrUpdateRoleAsync(
        Guid id,
        RolePackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleAsync(
        Guid id,
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
    /// <returns></returns>
    Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        Guid id,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> AddRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userId">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> RemoveRoleUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询角色的凭据
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
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
    /// <returns></returns>
    Task<RoleClaimInfo?> GetRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, RoleClaimInfo? roleClaim)> CreateRoleClaimAsync(
        Guid id,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, RoleClaimInfo? roleClaim)> UpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="pack">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, RoleClaimInfo? roleClaim)> CreateOrUpdateRoleClaimAsync(
        Guid id,
        int claimId,
        RoleClaimPackage pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);
}