namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     存储已被释放异常
/// </summary>
public sealed class StoreDisposedException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="storeType"></param>
    public StoreDisposedException(Type storeType) : base($"存储：{storeType.FullName}已被释放。",
        ExceptionCode.StoreDisposedException)
    {
    }

    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public StoreDisposedException(string errorMessage, int errorCode = ExceptionCode.StoreDisposedException) : base(
        errorMessage, errorCode)
    {
    }
}