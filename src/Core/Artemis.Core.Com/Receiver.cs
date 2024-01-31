using System.Net.Sockets;

namespace Artemis.Core.Com;

/// <summary>
///     接收器
/// </summary>
internal class Receiver : AwaitableEventArgs
{
    private short _token;

    /// <summary>
    ///     接收数据
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="memory"></param>
    /// <returns></returns>
    public ValueTask<int> ReceiveAsync(Socket socket, Memory<byte> memory)
    {
        SetBuffer(memory);
        if (socket.ReceiveAsync(this)) return new ValueTask<int>(this, _token++);

        var transferred = BytesTransferred;
        var err = SocketError;
        return err == SocketError.Success
            ? new ValueTask<int>(transferred)
            : ValueTask.FromException<int>(new SocketException((int)err));
    }
}