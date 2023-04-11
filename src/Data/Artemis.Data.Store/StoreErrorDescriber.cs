namespace Artemis.Data.Store;

/// <summary>
///     存储异常描述器
/// </summary>
public sealed class StoreErrorDescriber : IStoreErrorDescriber
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
            Description = Resources.FomateNotFoundId(id)
        };
    }
}