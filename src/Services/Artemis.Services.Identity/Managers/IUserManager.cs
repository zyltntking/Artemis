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
    ///     根据用户信息搜索用户
    /// </summary>
    /// <param name="nameSearch">用户名搜索值</param>
    /// <param name="emailSearch">邮箱搜索值</param>
    /// <param name="phoneNumberSearch">电话号码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
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
    /// <returns>查询不到用户实例时返回空</returns>
    Task<UserInfo?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="package">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的用户实例</returns>
    Task<(StoreResult result, UserInfo? user)> CreateUserAsync(
        UserPackage package,
        string password,
        CancellationToken cancellationToken);

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="packages">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>创建结果</returns>
    Task<StoreResult> CreateUsersAsync(
        IEnumerable<KeyValuePair<UserPackage, string>> packages,
        CancellationToken cancellationToken);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果和更新后的实体</returns>
    Task<(StoreResult result, UserInfo? user)> UpdateUserAsync(
        Guid id,
        UserPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="packages">用户信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>更新结果</returns>
    Task<StoreResult> UpdateUsersAsync(
        IEnumerable<KeyValuePair<Guid, UserPackage>> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户
    /// </summary>
    /// <param name="ids">用户标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteUsersAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色名搜索该用户具有的角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleNameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<RoleInfo>> FetchUserRolesAsync(
        Guid id,
        string? roleNameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>查询角色结果</returns>
    Task<RoleInfo?> GetUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleIds">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserRolesAsync(
        Guid id,
        IEnumerable<Guid> roleIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleId">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserRoleAsync(
        Guid id,
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户角色
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="roleIds">角色标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserRolesAsync(
        Guid id,
        IEnumerable<Guid> roleIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询用户的凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<UserClaimInfo>> FetchUserClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    Task<UserClaimInfo?> GetUserClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserClaimAsync(
        Guid id,
        UserClaimPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="packages">凭据信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserClaimsAsync(
        Guid id,
        IEnumerable<UserClaimPackage> packages,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="claimId">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserClaimAsync(
        Guid id,
        int claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户凭据
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="claimIds">凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserClaimsAsync(
        Guid id,
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProviderSearch">登录提供程序搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到登录信息实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<UserLoginInfo>> FetchUserLoginsAsync(
        Guid id,
        string? loginProviderSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    Task<UserLoginInfo?> GetUserLoginAsync(
        Guid id,
        int loginId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserLoginAsync(
        Guid id,
        UserLoginPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     替换用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录信息标识</param>
    /// <param name="package">登录信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>替换结果</returns>
    Task<StoreResult> ReplaceUserLoginAsync(
        Guid id,
        int loginId,
        UserLoginPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginId">登录标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserLoginAsync(
        Guid id,
        int loginId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="loginIds">登录标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserLoginsAsync(
        Guid id,
        IEnumerable<int> loginIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="loginProviderSearch">登录提供程序搜索值</param>
    /// <param name="nameSearch">令牌名</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>当查询不到登录信息实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<UserTokenInfo>> FetchUserTokensAsync(
        Guid id,
        string? loginProviderSearch = null,
        string? nameSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌信息标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>用户凭据信息</returns>
    Task<UserTokenInfo?> GetUserTokenAsync(
        Guid id,
        int tokenId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     添加用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="package">令牌信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>添加结果</returns>
    Task<StoreResult> AddUserTokenAsync(
        Guid id,
        UserTokenPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     替换用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌信息标识</param>
    /// <param name="package">令牌信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>替换结果</returns>
    Task<StoreResult> ReplaceUserTokenAsync(
        Guid id,
        int tokenId,
        UserTokenPackage package,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户令牌信息
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <param name="tokenId">令牌标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserTokenAsync(
        Guid id,
        int tokenId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除用户登录信息
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="tokenIds">令牌标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>删除结果</returns>
    Task<StoreResult> RemoveUserTokensAsync(
        Guid id,
        IEnumerable<int> tokenIds,
        CancellationToken cancellationToken = default);
}