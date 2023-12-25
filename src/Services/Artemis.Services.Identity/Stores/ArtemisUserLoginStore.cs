using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserLogin接口
/// </summary>
public interface IArtemisUserLoginStore : IStore<ArtemisUserLogin, int>
{
}

/// <summary>
///     ArtemisUserLogin
/// </summary>
public class ArtemisUserLoginStore : Store<ArtemisUserLogin, int>, IArtemisUserLoginStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserLoginStore(
        ArtemisIdentityContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null) : base(context, storeOptions, cache)
    {
    }
}