using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     用户管理器接口
/// </summary>
public interface IUserManager : IManager<ArtemisUser>
{
    /// <summary>
    ///     搜索用户
    /// </summary>
    /// <param name="nameSearch">用户名搜索值</param>
    /// <param name="emailSearch">邮箱搜索值</param>
    /// <param name="phoneNumberSearch">电话号码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<UserInfo>> FetchUserAsync(
        string? nameSearch,
        string? emailSearch,
        string? phoneNumberSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据用户标识获取用户
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<UserInfo?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="package">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, UserInfo? user)> CreateUserAsync(
        UserPackage package,
        string password,
        CancellationToken cancellationToken);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="pack">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, UserInfo? user)> UpdateUserAsync(
        Guid id,
        UserPackage pack,
        string? password = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="pack">用户信息</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<(StoreResult result, UserInfo? user)> CreateOrUpdateUserAsync(
        Guid id,
        UserPackage pack,
        string? password = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色名搜索该用户具有的角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleNameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<RoleInfo>> FetchUserRolesAsync(
        Guid id,
        string? roleNameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> AddUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="roleId">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> RemoveUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default);
}