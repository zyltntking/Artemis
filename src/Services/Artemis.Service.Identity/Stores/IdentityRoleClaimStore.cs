﻿using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Identity.Stores;

#region Interface

/// <summary>
///     认证角色凭据存储接口
/// </summary>
public interface IIdentityRoleClaimStore : IStore<IdentityRoleClaim, int, Guid>;

#endregion

/// <summary>
///     认证角色凭据存储
/// </summary>
public class IdentityRoleClaimStore : Store<IdentityRoleClaim, int, Guid>, IIdentityRoleClaimStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public IdentityRoleClaimStore(
        DbContext context,
        IStoreOptions? storeOptions = null,
        IDistributedCache? cache = null,
        ILogger? logger = null) : base(context, storeOptions, cache, logger)
    {
    }
}