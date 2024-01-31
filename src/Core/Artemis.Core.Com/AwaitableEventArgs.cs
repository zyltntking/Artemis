using System.Net.Sockets;
using System.Threading.Tasks.Sources;

namespace Artemis.Core.Com;

/// <summary>
///     可等待事件参数
/// </summary>
internal abstract class AwaitableEventArgs : SocketAsyncEventArgs, IValueTaskSource<int>
{
    /// <summary>
    ///     可等待事件参数源
    /// </summary>
    private ManualResetValueTaskSourceCore<int> _source;

    /// <summary>
    ///     构造函数
    /// </summary>
    protected AwaitableEventArgs() : base(true)
    {
    }

    /// <summary>
    ///     完成事件
    /// </summary>
    /// <param name="eventArgs"></param>
    protected override void OnCompleted(SocketAsyncEventArgs eventArgs)
    {
        if (SocketError != SocketError.Success) _source.SetException(new SocketException((int)SocketError));


        _source.SetResult(BytesTransferred);
    }

    #region Implementation of IValueTaskSource<out T>

    /// <summary>
    ///     Gets the result of the <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource`1" />.
    /// </summary>
    /// <param name="token">
    ///     An opaque value that was provided to the <see cref="T:System.Threading.Tasks.ValueTask" />
    ///     constructor.
    /// </param>
    /// <returns>The result of the <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource`1" />.</returns>
    public int GetResult(short token)
    {
        var result = _source.GetResult(token);
        _source.Reset();
        return result;
    }

    /// <summary>
    ///     Gets the status of the current operation.
    /// </summary>
    /// <param name="token">
    ///     Opaque value that was provided to the <see cref="T:System.Threading.Tasks.ValueTask" />'s
    ///     constructor.
    /// </param>
    /// <returns>A value that indicates the status of the current operation.</returns>
    public ValueTaskSourceStatus GetStatus(short token)
    {
        return _source.GetStatus(token);
    }

    /// <summary>
    ///     Schedules the continuation action for this <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource`1" />.
    /// </summary>
    /// <param name="continuation">The continuation to invoke when the operation has completed.</param>
    /// <param name="state">The state object to pass to <paramref name="continuation" /> when it's invoked.</param>
    /// <param name="token">
    ///     An opaque value that was provided to the <see cref="T:System.Threading.Tasks.ValueTask" />
    ///     constructor.
    /// </param>
    /// <param name="flags">The flags describing the behavior of the continuation.</param>
    public void OnCompleted(Action<object?> continuation, object? state, short token,
        ValueTaskSourceOnCompletedFlags flags)
    {
        _source.OnCompleted(continuation, state, token, flags);
    }

    #endregion
}