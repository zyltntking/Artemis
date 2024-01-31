﻿using System.Buffers;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Artemis.Core.Com;

/// <summary>
///     发送器
/// </summary>
internal class Sender : AwaitableEventArgs
{
    private List<ArraySegment<byte>>? _buffers;
    private short _token;

    /// <summary>
    ///     发送数据
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public ValueTask<int> SendAsync(Socket socket, in ReadOnlyMemory<byte> data)
    {
        SetBuffer(MemoryMarshal.AsMemory(data));
        if (socket.SendAsync(this)) return new ValueTask<int>(this, _token++);

        var transferred = BytesTransferred;
        var err = SocketError;
        return err == SocketError.Success
            ? new ValueTask<int>(transferred)
            : ValueTask.FromException<int>(new SocketException((int)err));
    }

    /// <summary>
    ///     发送数据
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ValueTask<int> SendAsync(Socket socket, in ReadOnlySequence<byte> data)
    {
        if (data.IsSingleSegment) return SendAsync(socket, data.First);
        _buffers ??= new List<ArraySegment<byte>>();
        foreach (var buff in data)
        {
            if (!MemoryMarshal.TryGetArray(buff, out var array))
                throw new InvalidOperationException("Buffer is not backed by an array.");

            _buffers.Add(array);
        }

        BufferList = _buffers;

        if (socket.SendAsync(this)) return new ValueTask<int>(this, _token++);

        var transferred = BytesTransferred;
        var err = SocketError;
        return err == SocketError.Success
            ? new ValueTask<int>(transferred)
            : ValueTask.FromException<int>(new SocketException((int)err));
    }

    /// <summary>
    ///     清空发送器
    /// </summary>
    public void Reset()
    {
        if (BufferList != null)
        {
            BufferList = null;

            _buffers?.Clear();
        }
        else
        {
            SetBuffer(null, 0, 0);
        }
    }
}