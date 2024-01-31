using System.IO.Pipelines;
using System.Net.Sockets;

namespace Artemis.Core.Com;

/// <summary>
/// Socket连接
/// </summary>
internal class SocketConnection : Connection
{

    private const int MinBuffSize = 1024;

    /// <summary>
    /// 套接字
    /// </summary>
    private Socket Socket { get; }

    /// <summary>
    /// 接收器
    /// </summary>
    private Receiver Receiver { get; }

    /// <summary>
    /// 发送器
    /// </summary>
    private Sender? Sender { get; set; }

    /// <summary>
    /// 发送池
    /// </summary>
    private SenderPool SenderPool { get; }

    /// <summary>
    /// 接受任务
    /// </summary>
    private Task? ReceiveTask { get; set; }

    /// <summary>
    /// 发送任务
    /// </summary>
    private Task? SendTask { get; set; }

    /// <summary>
    /// 传输管道
    /// </summary>
    private Pipe TransportPipe { get; }

    /// <summary>
    /// 应用管道
    /// </summary>
    private Pipe ApplicationPipe { get; }

    /// <summary>
    /// 关闭锁
    /// </summary>
    private object ShutdownLock { get; } = new();

    private volatile bool _socketDisposed;

    /// <summary>
    ///    发送器
    /// </summary>
    public PipeWriter Output { get; }

    /// <summary>
    ///   接收器
    /// </summary>
    public PipeReader Input { get; }

    /// <summary>
    /// Socket连接构造
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="senderPool"></param>
    public SocketConnection(Socket socket, SenderPool senderPool)
    {
        Socket = socket;
        Receiver = new Receiver();
        SenderPool = senderPool;
        TransportPipe = new Pipe();
        Output = TransportPipe.Writer;
        ApplicationPipe = new Pipe();
        Input = ApplicationPipe.Reader;

    }

    #region Overrides of Connection

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
    ///     asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override async ValueTask DisposeAsync()
    {
        await TransportPipe.Reader.CompleteAsync();
        await ApplicationPipe.Writer.CompleteAsync();
        try
        {
            if (ReceiveTask != null)
            {
                await ReceiveTask;
            }

            if (SendTask != null)
            {
                await SendTask;
            }
        }
        finally
        {
            Receiver.Dispose();
            Sender?.Dispose();
        }
    }

    /// <summary>
    ///     启动连接
    /// </summary>
    public override void Start()
    {
        try
        {
            SendTask = SendWork();
            ReceiveTask = ReceiveWork();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    /// <summary>
    /// 发送任务
    /// </summary>
    /// <returns></returns>
    private async Task SendWork()
    {
        try
        {
            while (true)
            {
                var result = await TransportPipe.Reader.ReadAsync();
                if (result.IsCanceled)
                {
                    break;
                }
                var buff = result.Buffer;
                if (!buff.IsEmpty)
                {
                    Sender = SenderPool.Rent();
                    await Sender.SendAsync(Socket, result.Buffer);
                    SenderPool.Return(Sender);
                    Sender = null;
                }
                TransportPipe.Reader.AdvanceTo(buff.End);
                if (result.IsCompleted)
                {
                    break;
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
        finally
        {
            await ApplicationPipe.Writer.CompleteAsync();
            Shutdown();
        }
    }

    private async Task ReceiveWork()
    {
        try
        {
            while (true)
            {
                var buff = ApplicationPipe.Writer.GetMemory(MinBuffSize);
                var bytes = await Receiver.ReceiveAsync(Socket, buff);
                if (bytes == 0)
                {
                    break;
                }
                ApplicationPipe.Writer.Advance(bytes);
                var result = await ApplicationPipe.Writer.FlushAsync();
                if (result.IsCanceled || result.IsCompleted)
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await ApplicationPipe.Writer.CompleteAsync();
            Shutdown();
        }
    }

    /// <summary>
    ///     关闭连接
    /// </summary>
    public override void Shutdown()
    {
        lock (ShutdownLock)
        {
            if (_socketDisposed)
            {
                return;
            }
            _socketDisposed = true;
            try
            {
                Socket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                Socket.Dispose();
            }
        }
    }

    #endregion
}