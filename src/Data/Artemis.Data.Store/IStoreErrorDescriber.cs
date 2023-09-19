namespace Artemis.Data.Store;

/// <summary>
///     存储异常描述接口
/// </summary>
public interface IStoreErrorDescriber
{
    /// <summary>
    ///     生成默认异常
    /// </summary>
    /// <returns></returns>
    StoreError DefaultError();

    /// <summary>
    ///     提示并发失败异常
    /// </summary>
    /// <returns></returns>
    StoreError ConcurrencyFailure();

    /// <summary>
    ///     提示未找到Id失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    StoreError NotFoundId(string? id);

    /// <summary>
    ///     提示已允许具缓存策略
    /// </summary>
    /// <returns></returns>
    StoreError EnableCache();

    /// <summary>
    ///     实体未找到
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    StoreError EntityNotFound(string? entity, string? flag);

    /// <summary>
    ///     实体已存在
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    StoreError EntityHasBeenSet(string? entity, string? flag);
}