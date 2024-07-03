using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.School.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Stores;

#region Interface

/// <summary>
///     班级存储接口
/// </summary>
public interface IArtemisClassStore : IStore<ArtemisClass>;

#endregion

/// <summary>
///     班级存储
/// </summary>
public sealed class ArtemisClassStore : Store<ArtemisClass>, IArtemisClassStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public ArtemisClassStore(
        SchoolContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }
}