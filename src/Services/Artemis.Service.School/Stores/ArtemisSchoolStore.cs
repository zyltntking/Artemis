using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.School.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Stores;

#region Interface

/// <summary>
///     学校存储接口
/// </summary>
public interface IArtemisSchoolStore : IStore<ArtemisSchool, Guid, Guid>;

#endregion

/// <summary>
///     学校存储
/// </summary>
public sealed class ArtemisSchoolStore : Store<ArtemisSchool, Guid, Guid>, IArtemisSchoolStore
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
    public ArtemisSchoolStore(
        SchoolContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy<Guid>? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }
}