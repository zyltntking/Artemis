namespace Artemis.Core.Com;

/// <summary>
///     发送池
/// </summary>
internal class SenderPool : Pool<Sender>
{
    /// <summary>
    ///     发送池构造
    /// </summary>
    /// <param name="maxQueueSize">最大连接数</param>
    public SenderPool(int maxQueueSize = 128) : base(maxQueueSize)
    {
    }

    #region Overrides of Pool<Sender>

    /// <summary>
    ///     创建连接
    /// </summary>
    /// <returns>连接对象</returns>
    protected override Sender Create()
    {
        return new Sender();
    }

    /// <summary>
    ///     重置连接
    /// </summary>
    /// <param name="connection">连接对象</param>
    protected override void Reset(Sender connection)
    {
        connection.Reset();
    }

    /// <summary>
    ///     释放连接
    /// </summary>
    /// <param name="connection">连接对象</param>
    protected override void Release(Sender connection)
    {
        connection.Dispose();
    }

    #endregion
}