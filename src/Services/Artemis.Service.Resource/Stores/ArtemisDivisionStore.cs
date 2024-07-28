﻿using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Resource.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Resource.Stores;

#region Interface

/// <summary>
///     行政区划存储接口
/// </summary>
public interface IArtemisDivisionStore : IStore<ArtemisDivision>;

#endregion

/// <summary>
///     行政区划存储
/// </summary>
public class ArtemisDivisionStore : Store<ArtemisDivision>, IArtemisDivisionStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    public ArtemisDivisionStore(
        ResourceContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }
}