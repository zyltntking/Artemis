using Artemis.Data.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     提供用于管理具存储模型的存储器的API接口
/// </summary>
public interface IManager : IDisposable;

#endregion

/// <summary>
///     提供用于管理具存储模型的存储器的API
/// </summary>
public abstract class Manager : IManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    protected Manager(
        IManagerOptions? options = null,
        ILogger? logger = null)
    {
        Options = options ?? new ManagerOptions();
        Logger = Options.EnableLogger ? null : logger;
    }

    #region Action

    /// <summary>
    ///     异步函数执行前
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected void OnAsyncActionExecuting(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        cancellationToken.ThrowIfCancellationRequested();
    }

    #endregion

    #region Properties

    /// <summary>
    ///     具键存储管理器配置接口
    /// </summary>
    private IManagerOptions Options { get; }

    /// <summary>
    ///     日志依赖访问器
    /// </summary>
    protected ILogger? Logger { get; }

    #endregion

    #region IDisposable

    /// <summary>
    ///     标记资源是否已被或是否需要释放
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     Releases all resources used by the user manager.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources; false to release only unmanaged
    ///     resources.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;
        StoreDispose();
        _disposed = true;
    }

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected abstract void StoreDispose();

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new ManagerDisposedException(GetType().Name);
    }

    #endregion
}