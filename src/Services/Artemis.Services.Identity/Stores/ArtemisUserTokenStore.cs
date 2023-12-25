using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     IArtemisUserTokenStore接口
/// </summary>
public interface IArtemisUserTokenStore : IStore<ArtemisUserToken, int>
{
}

/// <summary>
///     ArtemisUserTokenStore
/// </summary>
public class ArtemisUserTokenStore : Store<ArtemisUserToken, int>, IArtemisUserTokenStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserTokenStore(
        ArtemisIdentityContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null) : base(context, storeOptions, cache)
    {
    }
}