using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserStore接口
/// </summary>
public interface IArtemisRoleStore : IStore<ArtemisRole>
{
    /// <summary>
    /// 根据角色名获取角色
    /// </summary>
    /// <typeparam name="TMap">映射目标类型</typeparam>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<TMap?> FindMapRoleByNameAsync<TMap>(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <typeparam name="TMap">映射目标类型</typeparam>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<TMap>> GetMapRolesAsync<TMap>(CancellationToken cancellationToken = default);
}

/// <summary>
///     ArtemisUserStore
/// </summary>
public class ArtemisRoleStore : Store<ArtemisRole>, IArtemisRoleStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisRoleStore(
        ArtemisIdentityContext context,
        IDistributedCache? cache = null) : base(context, cache)
    {
    }

    #region Implementation of IArtemisRoleStore

    /// <summary>
    /// 根据角色名获取角色
    /// </summary>
    /// <typeparam name="TMap">映射目标类型</typeparam>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<TMap?> FindMapRoleByNameAsync<TMap>(string name, CancellationToken cancellationToken = default)
    {
        return EntityQuery
            .Where(role => role.NormalizedName == name)
            .ProjectToType<TMap>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <typeparam name="TMap">映射目标类型</typeparam>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public async Task<IEnumerable<TMap>> GetMapRolesAsync<TMap>(CancellationToken cancellationToken = default)
    {
        return await EntityQuery
            .ProjectToType<TMap>()
            .ToListAsync(cancellationToken);
    }

    #endregion

}