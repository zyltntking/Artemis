using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Models;

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
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<Role> GetRoleAsync(string roleName);

    /// <summary>
    /// 根据角色id获取角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<Role> GetRoleAsync(Guid roleId);

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Role>> GetRolesAsync();

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="roleNameSearch">搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <returns></returns>
    Task<PageResult<Role>> FetchRolesAsync(string? roleNameSearch, int page = 1, int size = 20);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="roleName">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, Role? role)> CreateRoleAsync(string roleName,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, Role role)> UpdateRoleAsync(Role role, CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除角色
    /// </summary>
    /// <param name="roleIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteRolesAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="userNameSearch">用户名</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<User>> FetchRoleUsersAsync(Guid roleId, string? userNameSearch = null, int page = 1, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取角色凭据列表
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<RoleClaim>> GetRoleClaimAsync(Guid roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询角色的声明
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(Guid roleId, string? claimTypeSearch = null, int page = 1, int size = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, IEnumerable<ArtemisRoleClaim> roleClaims)> CreateRoleClaimsAsync(IEnumerable<ArtemisRoleClaim> roleClaims, CancellationToken cancellationToken = default);
}