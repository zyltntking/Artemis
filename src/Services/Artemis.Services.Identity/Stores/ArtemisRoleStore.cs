﻿using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisUserStore接口
/// </summary>
public interface IArtemisRoleStore : IKeyWithStore<ArtemisRole>
{
}

/// <summary>
///     ArtemisUserStore
/// </summary>
public class ArtemisRoleStore : KeyWithStore<ArtemisRole>, IArtemisRoleStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="storeOptions"></param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisRoleStore(
        ArtemisIdentityContext context,
        IKeyWithStoreOptions? storeOptions = null,
        IDistributedCache? cache = null) : base(context, cache, storeOptions)
    {
    }
}