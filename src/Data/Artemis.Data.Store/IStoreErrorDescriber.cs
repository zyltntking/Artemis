namespace Artemis.Data.Store;

/// <summary>
/// 存储异常描述接口
/// </summary>
public interface IStoreErrorDescriber
{
    /// <summary>
    /// 生成默认异常
    /// </summary>
    /// <returns></returns>
    StoreError DefaultError();

    /// <summary>
    /// 提示并发失败异常
    /// </summary>
    /// <returns></returns>
    StoreError ConcurrencyFailure();

    /// <summary>
    /// 提示未找到Id失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    StoreError NotFoundId(string? id);
}