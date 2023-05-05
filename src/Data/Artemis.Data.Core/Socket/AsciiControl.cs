namespace Artemis.Data.Core.Socket;

/// <summary>
///     ASCII控制
/// </summary>
public static class AsciiControl
{
    /// <summary>
    ///     空字符
    /// </summary>
    /// <remarks>NULL</remarks>
    public const byte Nul = 0x00;

    /// <summary>
    ///     标题开始
    /// </summary>
    /// <remarks>Start Of Headling</remarks>
    public const byte Soh = 0x01;

    /// <summary>
    ///     正文开始
    /// </summary>
    /// <remarks>Start Of Text</remarks>
    public const byte Stx = 0x02;

    /// <summary>
    ///     正文结束
    /// </summary>
    /// <remarks>End Of Text</remarks>
    public const byte Etx = 0x03;

    /// <summary>
    ///     传输结束
    /// </summary>
    /// <remarks>End Of Transmission</remarks>
    public const byte Eot = 0x04;

    /// <summary>
    ///     请求
    /// </summary>
    /// <remarks>Enquiry</remarks>
    public const byte Enq = 0x05;

    /// <summary>
    ///     回应/响应/收到通知
    /// </summary>
    /// <remarks>Acknowledge</remarks>
    public const byte Ack = 0x06;

    /// <summary>
    ///     响铃
    /// </summary>
    /// <remarks>Bell</remarks>
    public const byte Bel = 0x07;

    /// <summary>
    ///     退格
    /// </summary>
    /// <remarks>Backspace</remarks>
    public const byte Bs = 0x08;

    /// <summary>
    ///     水平制表符
    /// </summary>
    /// <remarks>Horizontal Tab</remarks>
    public const byte Ht = 0x09;

    /// <summary>
    ///     换行键
    /// </summary>
    /// <remarks>Line Feed</remarks>
    /// <see cref="Nl" />
    public const byte Lf = 0x0a;

    /// <summary>
    ///     换行键
    /// </summary>
    /// <remarks>New Line</remarks>
    /// <see cref="Lf" />
    public const byte Nl = 0x0a;

    /// <summary>
    ///     垂直制表符
    /// </summary>
    /// <remarks>Vertical Tab</remarks>
    public const byte Vt = 0x0b;

    /// <summary>
    ///     换页键
    /// </summary>
    /// <remarks>Form Feed</remarks>
    /// <see cref="Np" />
    public const byte Ff = 0x0c;

    /// <summary>
    ///     换页键
    /// </summary>
    /// <remarks>New Page</remarks>
    /// <see cref="Ff" />
    public const byte Np = 0x0c;

    /// <summary>
    ///     回车键
    /// </summary>
    /// <remarks>Carriage Return</remarks>
    public const byte Cr = 0x0d;

    /// <summary>
    ///     不用切换
    /// </summary>
    /// <remarks>Shift Out</remarks>
    public const byte So = 0x0e;

    /// <summary>
    ///     启用切换
    /// </summary>
    /// <remarks>Shift In</remarks>
    public const byte Si = 0x0f;

    /// <summary>
    ///     数据链路转义
    /// </summary>
    /// <remarks>Data Link Escape</remarks>
    public const byte Dle = 0x10;

    /// <summary>
    ///     设备控制1
    /// </summary>
    /// <remarks>Device Control 1</remarks>
    /// <see cref="Xon" />
    public const byte Dc1 = 0x11;

    /// <summary>
    ///     传输开始
    /// </summary>
    /// <remarks>Transmission O</remarks>
    /// <see cref="Dc1" />
    public const byte Xon = 0x11;

    /// <summary>
    ///     设备控制2
    /// </summary>
    /// <remarks>Device Control 2</remarks>
    public const byte Dc2 = 0x12;

    /// <summary>
    ///     设备控制3
    /// </summary>
    /// <remarks>Device Control 3</remarks>
    /// <see cref="XOff" />
    public const byte Dc3 = 0x13;

    /// <summary>
    ///     传输中断
    /// </summary>
    /// <remarks>Transmission Off</remarks>
    /// <see cref="Dc3" />
    public const byte XOff = 0x13;

    /// <summary>
    ///     设备控制4
    /// </summary>
    public const byte Dc4 = 0x14;

    /// <summary>
    ///     无响应/非正常响应/拒绝接收
    /// </summary>
    /// <remarks>Negative Acknowledge</remarks>
    public const byte Nak = 0x15;

    /// <summary>
    ///     同步空闲
    /// </summary>
    /// <remarks>Synchronous Idle</remarks>
    public const byte Syn = 0x16;

    /// <summary>
    ///     传输块结束/块传输终止
    /// </summary>
    /// <remarks>End of Transmission Block</remarks>
    public const byte Etb = 0x17;

    /// <summary>
    ///     取消
    /// </summary>
    /// <remarks>Cancel</remarks>
    public const byte Can = 0x18;

    /// <summary>
    ///     已到介质末端/介质存储已满/介质中断
    /// </summary>
    /// <remarks>End of Medium</remarks>
    public const byte Em = 0x19;

    /// <summary>
    ///     替补/替换
    /// </summary>
    /// <remarks>Substitute</remarks>
    public const byte Sub = 0x1a;

    /// <summary>
    ///     逃离/取消
    /// </summary>
    /// <remarks>Escape</remarks>
    public const byte Esc = 0x1b;

    /// <summary>
    ///     文件分割符
    /// </summary>
    /// <remarks>File Separator</remarks>
    public const byte Fs = 0x1c;

    /// <summary>
    ///     组分隔符/分组符
    /// </summary>
    /// <remarks>Group Separator</remarks>
    public const byte Gs = 0x1d;

    /// <summary>
    ///     记录分离符
    /// </summary>
    /// <remarks>Record Separator</remarks>
    public const byte Rs = 0x1e;

    /// <summary>
    ///     单元分隔符
    /// </summary>
    /// <remarks>Unit Separator</remarks>
    public const byte Us = 0x1f;

    /// <summary>
    ///     空格
    /// </summary>
    /// <remarks>Space</remarks>
    public const byte Sp = 0x20;

    /// <summary>
    ///     删除
    /// </summary>
    /// <remarks>Delete</remarks>
    public const byte Del = 0x7f;
}