using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Transfer;
using Artemis.Shared.Identity.Transfer.Base;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///    用户管理器接口
/// </summary>
public interface IUserManager : IManager<ArtemisUser>
{
    /// <summary>
    /// 搜索用户
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
    /// 根据用户标识获取用户
    /// </summary>
    /// <param name="id">标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<UserInfo?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="pack">用户信息</param>
    /// <param name="password">用户密码</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateUserAsync(
        UserBase pack, 
        string password,
        CancellationToken cancellationToken);
}