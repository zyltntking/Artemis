﻿using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证用户档案存储接口
/// </summary>
public interface IIdentityUserProfileStore : IKeyLessStore<IdentityUserProfile>;

#endregion

/// <summary>
///     认证用户档案存储
/// </summary>
public sealed class IdentityUserProfileStore : KeyLessStore<IdentityUserProfile>, IIdentityUserProfileStore
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityUserProfileStore(
        IdentityContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        IDistributedCache? cache = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cache, logger, describer)
    {
    }

    /// <summary>
    ///     实体键生成委托
    /// </summary>
    protected override Func<IdentityUserProfile, string>? EntityKey { get; init; } =
        userProfile => $"{userProfile.UserId}:{userProfile.Key}";
}