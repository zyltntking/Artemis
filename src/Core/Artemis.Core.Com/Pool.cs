using System.Collections.Concurrent;

namespace Artemis.Core.Com;

#region Interface

/// <summary>
///     连接池接口
/// </summary>
/// <typeparam name="TConnection">连接模板</typeparam>
internal interface IPool<TConnection>
{
    /// <summary>
    ///     租用
    /// </summary>
    /// <returns>连接对象</returns>
    TConnection Rent();

    /// <summary>
    ///     归还
    /// </summary>
    /// <param name="connection">归还连接对象</param>
    void Return(TConnection connection);
}

#endregion

/// <summary>
///     抽象连接池接口
/// </summary>
/// <typeparam name="TConnection"></typeparam>
internal abstract class Pool<TConnection> : IPool<TConnection>, IDisposable
{
    /// <summary>
    ///     池构造
    /// </summary>
    /// <param name="maxQueueSize">最大连接数</param>
    protected Pool(int maxQueueSize = 128)
    {
        MaxQueueSize = maxQueueSize;
    }

    #region IDisposable

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            while (_queue.TryDequeue(out var connection)) Release(connection);
        }
    }

    #endregion

    #region Fields

    /// <summary>
    ///     最大连接数
    /// </summary>
    private int MaxQueueSize { get; }

    /// <summary>
    ///     连接队列
    /// </summary>
    private readonly ConcurrentQueue<TConnection> _queue = new();

    private int _count;
    private bool _disposed;

    #endregion

    #region Methods

    /// <summary>
    ///     创建连接
    /// </summary>
    /// <returns>连接对象</returns>
    protected abstract TConnection Create();

    /// <summary>
    ///     重置连接
    /// </summary>
    /// <param name="connection">连接对象</param>
    protected abstract void Reset(TConnection connection);

    /// <summary>
    ///     释放连接
    /// </summary>
    /// <param name="connection">连接对象</param>
    protected abstract void Release(TConnection connection);

    #endregion

    #region IPool<TConnection> Members

    /// <summary>
    ///     租用
    /// </summary>
    /// <returns>连接对象</returns>
    public TConnection Rent()
    {
        if (_queue.TryDequeue(out var connection))
        {
            Interlocked.Decrement(ref _count);
            Reset(connection);
            return connection;
        }

        return Create();
    }

    /// <summary>
    ///     归还
    /// </summary>
    /// <param name="connection">归还连接对象</param>
    public void Return(TConnection connection)
    {
        // 如果还不回去，就直接释放掉
        if (_disposed || _count >= MaxQueueSize)
        {
            Release(connection);
        }
        else
        {
            Interlocked.Increment(ref _count);
            _queue.Enqueue(connection);
        }
    }

    #endregion
}