using Artemis.Data.Store;
using Artemis.Services.Identity.Data;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserLogin接口
/// </summary>
public interface IArtemisUserLoginStore : IKeyLessStore<ArtemisUserLogin>
{
}

/// <summary>
///     ArtemisUserLogin
/// </summary>
public class ArtemisUserLoginStore : KeyLessStore<ArtemisUserLogin>, IArtemisUserLoginStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserLoginStore(
        ArtemisIdentityContext context,
        IKeyLessStoreOptions? storeOptions = null) : base(context, storeOptions)
    {
    }
}