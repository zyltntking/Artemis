namespace Artemis.Core.Com;

#region Interface

/// <summary>
///     连接接口
/// </summary>
internal interface IConnection : IAsyncDisposable
{
    /// <summary>
    ///     启动连接
    /// </summary>
    void Start();

    /// <summary>
    ///     关闭连接
    /// </summary>
    void Shutdown();
}

#endregion

/// <summary>
///     抽象连接实现
/// </summary>
internal abstract class Connection : IConnection
{
    #region IDisposable

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
    ///     asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public abstract ValueTask DisposeAsync();

    #endregion

    #region Implementation of IConnection

    /// <summary>
    ///     启动连接
    /// </summary>
    public abstract void Start();

    /// <summary>
    ///     关闭连接
    /// </summary>
    public abstract void Shutdown();

    #endregion
}