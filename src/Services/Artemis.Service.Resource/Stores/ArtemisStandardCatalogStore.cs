﻿using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Resource.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Resource.Stores;

#region Interface

/// <summary>
///     标准目录存储接口
/// </summary>
public interface IArtemisStandardCatalogStore : IStore<ArtemisStandardCatalog>;

#endregion

/// <summary>
/// 标准目录存储
/// </summary>
public class ArtemisStandardCatalogStore : Store<ArtemisStandardCatalog>, IArtemisStandardCatalogStore
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
    public ArtemisStandardCatalogStore(
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