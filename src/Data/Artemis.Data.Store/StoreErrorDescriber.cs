namespace Artemis.Data.Store;

/// <summary>
///     存储异常描述器
/// </summary>
public sealed record StoreErrorDescriber : IStoreErrorDescriber
{
    /// <summary>
    ///     生成默认异常
    /// </summary>
    /// <returns>默认异常</returns>
    public StoreError DefaultError()
    {
        return new StoreError
        {
            Code = nameof(DefaultError),
            Description = Resources.DefaultError
        };
    }

    /// <summary>
    ///     提示发生并发失败
    /// </summary>
    /// <returns></returns>
    public StoreError ConcurrencyFailure()
    {
        return new StoreError
        {
            Code = nameof(ConcurrencyFailure),
            Description = Resources.ConcurrencyFailure
        };
    }

    /// <summary>
    ///     提示未找到Id失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public StoreError NotFoundId(string? id)
    {
        return new StoreError
        {
            Code = nameof(NotFoundId),
            Description = Formatter.FormatNotFoundId(id)
        };
    }

    /// <summary>
    ///     提示已允许具缓存策略
    /// </summary>
    /// <returns></returns>
    public StoreError EnableCache()
    {
        return new StoreError
        {
            Code = nameof(EnableCache),
            Description = Resources.EnableCache
        };
    }

    /// <summary>
    ///     实体未找到
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public StoreError EntityNotFound(string? entity, string? flag)
    {
        return new StoreError
        {
            Code = nameof(EntityNotFound),
            Description = Formatter.FormatEntityNotFound(entity, flag)
        };
    }

    /// <summary>
    ///     实体已存在
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public StoreError EntityHasBeenSet(string? entity, string? flag)
    {
        return new StoreError
        {
            Code = nameof(EntityHasBeenSet),
            Description = Formatter.FormatEntityHasBeenSet(entity, flag)
        };
    }
}