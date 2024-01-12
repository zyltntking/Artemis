using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisClaimStore接口
/// </summary>
public interface IArtemisClaimStore : IKeyWithStore<ArtemisClaim>
{
}

/// <summary>
///     ArtemisClaimStore
/// </summary>
public class ArtemisClaimStore : KeyWithStore<ArtemisClaim>, IArtemisClaimStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions">存储依赖</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisClaimStore(
        ArtemisIdentityContext context,
        IKeyWithStoreOptions? storeOptions = null,
        IDistributedCache? cache = null) : base(context, cache, storeOptions)
    {
    }
}