namespace Artemis.Data.Core;

/// <summary>
///     实体代理接口
/// </summary>
public interface IEntityProxy
{
    /// <summary>
    ///     是否允许缓存代理
    /// </summary>
    bool EnableCacheProxy { get; set; }

    /// <summary>
    ///     缓存代理
    /// </summary>
    IEntityCacheProxy CacheProxy { get; set; }

    /// <summary>
    ///     是否允许操作者代理
    /// </summary>
    bool EnableHandlerProxy { get; set; }

    /// <summary>
    ///     操作者代理
    /// </summary>
    THandler HandlerProxy<THandler>();
}

/// <summary>
///     缓存代理接口
/// </summary>
public interface IEntityCacheProxy;