using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Services.Identity.Stores;

/// <summary>
/// ArtemisUserRoleStore接口
/// </summary>
public interface IArtemisUserRoleStore : IStore<ArtemisUserRole, int>
{
}

/// <summary>
/// ArtemisUserRoleStore
/// </summary>
public class ArtemisUserRoleStore : Store<ArtemisUserRole, int>, IArtemisUserRoleStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="describer">操作异常描述者</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisUserRoleStore(DbContext context, IDistributedCache? cache = null, ILogger? logger = null, IStoreErrorDescriber? describer = null) : base(context, cache, logger, describer)
    {
    }
}