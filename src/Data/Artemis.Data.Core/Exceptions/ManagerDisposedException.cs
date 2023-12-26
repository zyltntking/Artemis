namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     管理器已被释放异常
/// </summary>
public sealed class ManagerDisposedException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="storeType"></param>
    public ManagerDisposedException(Type storeType) : base($"管理器：{storeType.FullName}已被释放。",
        ExceptionCode.ManagerDisposedException)
    {
    }

    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="errorMessage">错误消息</param>
    /// <param name="errorCode">错误编码</param>
    public ManagerDisposedException(string errorMessage, int errorCode = ExceptionCode.ManagerDisposedException) : base(
        errorMessage, errorCode)
    {
    }
}