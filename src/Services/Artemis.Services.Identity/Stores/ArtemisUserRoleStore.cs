using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserRoleStore接口
/// </summary>
public interface IArtemisUserRoleStore : IStore<ArtemisUserRole, int>
{
}

/// <summary>
///     ArtemisUserRoleStore
/// </summary>
public class ArtemisUserRoleStore : Store<ArtemisUserRole, int>, IArtemisUserRoleStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserRoleStore(
        ArtemisIdentityContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null) : base(context, storeOptions, cache)
    {
    }
}