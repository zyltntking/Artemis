using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;

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
    Task<Role?> GetRoleAsync(string roleName);

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Role>> GetAllRolesAsync();

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="roleNameSearchText">搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <returns></returns>
    Task<PageResult<Role>> GetRolesAsync(string? roleNameSearchText = null, int page = 1, int size = 20);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="roleName">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(IdentityResult result, Role? role)> CreateRoleAsync(string roleName,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="role">角色</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(IdentityResult result, Role role)> UpdateRoleAsync(Role role, CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IdentityResult> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     批量删除角色
    /// </summary>
    /// <param name="roleIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> DeleteRolesAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
}