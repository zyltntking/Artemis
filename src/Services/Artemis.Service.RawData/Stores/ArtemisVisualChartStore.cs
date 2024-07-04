﻿using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.RawData.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.RawData.Stores;

#region Interface

/// <summary>
///     验光仪存储接口
/// </summary>
public interface IArtemisVisualChartStore : IStore<ArtemisVisualChart>;

#endregion

/// <summary>
///     验光仪存储
/// </summary>
public class ArtemisVisualChartStore : Store<ArtemisVisualChart>, IArtemisVisualChartStore
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
    public ArtemisVisualChartStore(
        RawDataContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }
}