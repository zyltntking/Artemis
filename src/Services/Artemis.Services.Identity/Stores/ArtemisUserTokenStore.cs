using Artemis.Data.Store;
using Artemis.Services.Identity.Data;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     IArtemisUserTokenStore接口
/// </summary>
public interface IArtemisUserTokenStore : IKeyLessStore<ArtemisUserToken>
{
}

/// <summary>
///     ArtemisUserTokenStore
/// </summary>
public class ArtemisUserTokenStore : KeyLessStore<ArtemisUserToken>, IArtemisUserTokenStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserTokenStore(
        ArtemisIdentityContext context,
        IKeyLessStoreOptions? storeOptions = null) : base(context, storeOptions)
    {
    }
}