namespace Artemis.Data.Core.AscII;

/// <summary>
///     ASCII控制
/// </summary>
public static class AsciiControl
{
    /// <summary>
    ///     空字符
    /// </summary>
    /// <remarks>NULL</remarks>
    /// <value>^@</value>
    public const byte NUL = 0x00;

    /// <summary>
    ///     标题开始
    /// </summary>
    /// <remarks>Start Of Headling</remarks>
    /// <value>^A</value>
    public const byte SOH = 0x01;

    /// <summary>
    ///     正文开始
    /// </summary>
    /// <remarks>Start Of Text</remarks>
    /// <value>^B</value>
    public const byte STX = 0x02;

    /// <summary>
    ///     正文结束
    /// </summary>
    /// <remarks>End Of Text</remarks>
    /// <value>^C</value>
    public const byte ETX = 0x03;

    /// <summary>
    ///     传输结束
    /// </summary>
    /// <remarks>End Of Transmission</remarks>
    /// <value>^D</value>
    public const byte EOT = 0x04;

    /// <summary>
    ///     请求
    /// </summary>
    /// <remarks>Enquiry</remarks>
    /// <value>^E</value>
    public const byte ENQ = 0x05;

    /// <summary>
    ///     回应/响应/收到通知
    /// </summary>
    /// <remarks>Acknowledge</remarks>
    /// <value>^F</value>
    public const byte ACK = 0x06;

    /// <summary>
    ///     响铃
    /// </summary>
    /// <remarks>Bell</remarks>
    /// <value>^G</value>
    public const byte BEL = 0x07;

    /// <summary>
    ///     退格
    /// </summary>
    /// <remarks>Backspace</remarks>
    /// <value>^H</value>
    public const byte BS = 0x08;

    /// <summary>
    ///     水平制表符
    /// </summary>
    /// <remarks>Horizontal Tab</remarks>
    /// <value>^I</value>
    public const byte HT = 0x09;

    /// <summary>
    ///     换行键
    /// </summary>
    /// <remarks>Line Feed</remarks>
    /// <see cref="NL" />
    /// <value>^J</value>
    public const byte LF = 0x0A;

    /// <summary>
    ///     换行键
    /// </summary>
    /// <remarks>New Line</remarks>
    /// <see cref="LF" />
    /// <value>^J</value>
    public const byte NL = 0x0A;

    /// <summary>
    ///     垂直制表符
    /// </summary>
    /// <remarks>Vertical Tab</remarks>
    /// <value>^K</value>
    public const byte VT = 0x0B;

    /// <summary>
    ///     换页键
    /// </summary>
    /// <remarks>Form Feed</remarks>
    /// <see cref="NP" />
    /// <value>^L</value>
    public const byte FF = 0x0C;

    /// <summary>
    ///     换页键
    /// </summary>
    /// <remarks>New Page</remarks>
    /// <see cref="FF" />
    /// <value>^L</value>
    public const byte NP = 0x0C;

    /// <summary>
    ///     回车键
    /// </summary>
    /// <remarks>Carriage Return</remarks>
    /// <value>^M</value>
    public const byte CR = 0x0D;

    /// <summary>
    ///     不用切换
    /// </summary>
    /// <remarks>Shift Out</remarks>
    /// <value>^N</value>
    public const byte SO = 0x0E;

    /// <summary>
    ///     启用切换
    /// </summary>
    /// <remarks>Shift In</remarks>
    /// <value>^O</value>
    public const byte SI = 0x0F;

    /// <summary>
    ///     数据链路转义
    /// </summary>
    /// <remarks>Data Link Escape</remarks>
    /// <value>^P</value>
    public const byte DLE = 0x10;

    /// <summary>
    ///     设备控制1
    /// </summary>
    /// <remarks>Device Control 1</remarks>
    /// <see cref="XON" />
    /// <value>^Q</value>
    public const byte DC1 = 0x11;

    /// <summary>
    ///     传输开始
    /// </summary>
    /// <remarks>Transmission O</remarks>
    /// <see cref="DC1" />
    /// <value>^Q</value>
    public const byte XON = 0x11;

    /// <summary>
    ///     设备控制2
    /// </summary>
    /// <remarks>Device Control 2</remarks>
    /// <value>^R</value>
    public const byte DC2 = 0x12;

    /// <summary>
    ///     设备控制3
    /// </summary>
    /// <remarks>Device Control 3</remarks>
    /// <see cref="XOFF" />
    /// <value>^S</value>
    public const byte DC3 = 0x13;

    /// <summary>
    ///     传输中断
    /// </summary>
    /// <remarks>Transmission Off</remarks>
    /// <see cref="DC3" />
    /// <value>^S</value>
    public const byte XOFF = 0x13;

    /// <summary>
    ///     设备控制4
    /// </summary>
    /// <remarks>Device Control 3</remarks>
    /// <value>^T</value>
    public const byte DC4 = 0x14;

    /// <summary>
    ///     无响应/非正常响应/拒绝接收
    /// </summary>
    /// <remarks>Negative Acknowledge</remarks>
    /// <value>^U</value>
    public const byte NAK = 0x15;

    /// <summary>
    ///     同步空闲
    /// </summary>
    /// <remarks>Synchronous Idle</remarks>
    /// <value>^V</value>
    public const byte SYN = 0x16;

    /// <summary>
    ///     传输块结束/块传输终止
    /// </summary>
    /// <remarks>End of Transmission Block</remarks>
    /// <value>^W</value>
    public const byte ETB = 0x17;

    /// <summary>
    ///     取消
    /// </summary>
    /// <remarks>Cancel</remarks>
    /// <value>^X</value>
    public const byte CAN = 0x18;

    /// <summary>
    ///     已到介质末端/介质存储已满/介质中断
    /// </summary>
    /// <remarks>End of Medium</remarks>
    /// <value>^Y</value>
    public const byte EM = 0x19;

    /// <summary>
    ///     替补/替换
    /// </summary>
    /// <remarks>Substitute</remarks>
    /// <value>^Z</value>
    public const byte SUB = 0x1A;

    /// <summary>
    ///     逃离/取消
    /// </summary>
    /// <remarks>Escape</remarks>
    /// <value>^[</value>
    public const byte ESC = 0x1B;

    /// <summary>
    ///     文件分割符
    /// </summary>
    /// <remarks>File Separator</remarks>
    /// <value>^\</value>
    public const byte FS = 0x1C;

    /// <summary>
    ///     组分隔符/分组符
    /// </summary>
    /// <remarks>Group Separator</remarks>
    /// <value>^]</value>
    public const byte GS = 0x1D;

    /// <summary>
    ///     记录分离符
    /// </summary>
    /// <remarks>Record Separator</remarks>
    /// <value>^^</value>
    public const byte RS = 0x1E;

    /// <summary>
    ///     单元分隔符
    /// </summary>
    /// <remarks>Unit Separator</remarks>
    /// <value>^_</value>
    public const byte US = 0x1F;

    /// <summary>
    ///     空格
    /// </summary>
    /// <remarks>Space</remarks>
    public const byte SP = 0x20;

    /// <summary>
    ///     删除
    /// </summary>
    /// <remarks>Delete</remarks>
    /// <value>^?</value>
    public const byte DEL = 0x7F;
}