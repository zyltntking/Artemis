using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;

namespace Artemis.Services.Identity;

/// <summary>
///     Artemis用户管理器接口
/// </summary>
public interface IIdentityManager : IManager<ArtemisUser>
{
    /// <summary>
    ///     测试
    /// </summary>
    Task<string> Test();

    /// <summary>
    ///     根据角色名获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<Role?> GetRoleAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色id获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<Role?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>角色列表</returns>
    /// <remarks>当查询不到角色实例时返回空列表</remarks>
    Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken"></param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<Role>> FetchRolesAsync(
        string? nameSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="description">角色描述</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    Task<AttachResult<StoreResult, Role>> CreateRoleAsync(
        string name,
        string? description = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleAsync(
        Role role,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> CreateOrUpdateRoleAsync(
        Role role,
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
    ///     批量删除角色
    /// </summary>
    /// <param name="ids">角色id列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRolesAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="nameSearch">用户名</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<User>> FetchRoleUsersAsync(
        Guid id,
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<RoleClaim>> GetRoleClaimAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询角色的声明
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaim">凭据</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateRoleClaimAsync(
        RoleClaim roleClaim,
        CancellationToken cancellationToken);

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<AttachResult<StoreResult, IEnumerable<RoleClaim>>> CreateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleClaimAsync(
        RoleClaim roleClaim,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<AttachResult<StoreResult, IEnumerable<RoleClaim>>> UpdateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimId">角色凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimAsync(
        int claimId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="checkStamp">校验戳</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimAsync(
        Guid roleId,
        string checkStamp,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimIds">凭据标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimsAsync(
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimKeys">角色键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimsAsync(
        IEnumerable<ClaimKey> claimKeys,
        CancellationToken cancellationToken);
}