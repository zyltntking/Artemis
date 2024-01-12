using Artemis.Data.Store;
using Artemis.Services.Identity.Data;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserRoleStore接口
/// </summary>
public interface IArtemisUserRoleStore : IKeyLessStore<ArtemisUserRole>
{
}

/// <summary>
///     ArtemisUserRoleStore
/// </summary>
public class ArtemisUserRoleStore : KeyLessStore<ArtemisUserRole>, IArtemisUserRoleStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserRoleStore(
        ArtemisIdentityContext context,
        IKeyLessStoreOptions? storeOptions = null) : base(context, storeOptions)
    {
    }
}