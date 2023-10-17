﻿using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Artemis.Services.Identity.Stores;

/// <summary>
///     ArtemisClaimStore接口
/// </summary>
public interface IArtemisClaimStore : IStore<ArtemisClaim>
{
}

/// <summary>
///     ArtemisClaimStore
/// </summary>
public class ArtemisClaimStore : Store<ArtemisClaim>, IArtemisClaimStore
{
    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="context">数据访问上下文</param>
    /// <param name="cache">缓存依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisClaimStore(
        ArtemisIdentityContext context,
        IDistributedCache? cache = null) : base(context, cache)
    {
    }
}