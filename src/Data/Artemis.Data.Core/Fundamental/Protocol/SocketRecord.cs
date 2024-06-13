namespace Artemis.Data.Core.Fundamental.Protocol;

/// <summary>
///     Socket记录
/// </summary>
public class SocketRecord<TContent> : Record where TContent : Record
{
    /// <summary>
    ///     SocketRecord实现
    /// </summary>
    /// <param name="bytes">字节码</param>
    public SocketRecord(byte[] bytes) : base(bytes)
    {
    }

    #region Implementation of ISocketRecord<out TContent>

    /// <summary>
    ///     报文头记录
    /// </summary>
    [Property(Order = 1)]
    public HeadRecord HeadRecord { get; set; } = null!;

    /// <summary>
    ///     报文内容段
    /// </summary>
    [Property(Order = 2, Length = Constants.Length.Unknown)]
    public TContent Content { get; set; } = null!;

    #endregion
}

/// <summary>
///     校验记录
/// </summary>
public class CheckRecord : Record
{
    /// <summary>
    ///     校验记录实现
    /// </summary>
    /// <param name="checkSum">求和校验位</param>
    /// <param name="checkXor">异或校验位</param>
    public CheckRecord(byte checkSum, byte checkXor)
    {
        CheckSum = checkSum;
        CheckXor = checkXor;

        SetBytes();
    }

    /// <summary>
    ///     校验记录实现
    /// </summary>
    /// <param name="bytes">输入字节码</param>
    public CheckRecord(byte[] bytes) : base(bytes)
    {
    }

    #region Implementation of ICheckRecord

    /// <summary>
    ///     求和校验位
    /// </summary>
    [Property(Order = 0)]
    public byte CheckSum { get; set; }

    /// <summary>
    ///     异或校验位
    /// </summary>
    [Property(Order = 1)]
    public byte CheckXor { get; set; }

    #endregion
}

/// <summary>
///     头记录
/// </summary>
public class HeadRecord : Record
{
    /// <summary>
    ///     头记录实现
    /// </summary>
    /// <param name="commandCategory">指令目录</param>
    /// <param name="command">指令</param>
    /// <param name="statusCategory">状态目录</param>
    /// <param name="status">状态</param>
    /// <param name="checkXor">异或校验位</param>
    /// <param name="length">内容长度</param>
    /// <param name="checkSum">求和校验位</param>
    public HeadRecord(
        byte commandCategory,
        byte command,
        byte statusCategory,
        byte status,
        byte checkSum,
        byte checkXor,
        long length)
    {
        CommandRecord = new CommandRecord(commandCategory, command);
        StatusRecord = new StatusRecord(statusCategory, status);
        CheckRecord = new CheckRecord(checkSum, checkXor);
        Length = length;
        SetBytes();
    }

    /// <summary>
    ///     HeadRecord实现
    /// </summary>
    /// <param name="bytes">字节码</param>
    public HeadRecord(byte[] bytes) : base(bytes)
    {
    }

    #region Implementation of IHeadRecord

    /// <summary>
    ///     长度
    /// </summary>
    [Property(Order = 0)]
    public long Length { get; set; }

    /// <summary>
    ///     指令记录
    /// </summary>
    [Property(Order = 1)]
    public CommandRecord CommandRecord { get; set; } = null!;

    /// <summary>
    ///     状态记录
    /// </summary>
    [Property(Order = 2)]
    public StatusRecord StatusRecord { get; set; } = null!;

    /// <summary>
    ///     报文校验段
    /// </summary>
    [Property(Order = 3)]
    public CheckRecord CheckRecord { get; set; } = null!;

    #endregion
}

/// <summary>
///     指令记录
/// </summary>
public sealed class CommandRecord : Record
{
    /// <summary>
    ///     CommandRecord实现
    /// </summary>
    /// <param name="category">指令目录</param>
    /// <param name="command">指令</param>
    public CommandRecord(byte category, byte command)
    {
        Category = category;
        Command = command;
        SetBytes();
    }

    /// <summary>
    ///     CommandRecord实现
    /// </summary>
    /// <param name="bytes">字节码</param>
    public CommandRecord(byte[] bytes) : base(bytes)
    {
    }

    #region Implementation of ICommandRecord

    /// <summary>
    ///     指令目录
    /// </summary>
    [Property(Order = 0)]
    public byte Category { get; set; }

    /// <summary>
    ///     指令项目
    /// </summary>
    [Property(Order = 1)]
    public byte Command { get; set; }

    #endregion
}

/// <summary>
///     状态记录
/// </summary>
public sealed class StatusRecord : Record
{
    /// <summary>
    ///     StatusRecord实现
    /// </summary>
    /// <param name="category"></param>
    /// <param name="status"></param>
    public StatusRecord(byte category, byte status)
    {
        Category = category;
        Status = status;
        SetBytes();
    }

    /// <summary>
    ///     StatusRecord实现
    /// </summary>
    /// <param name="bytes">字节码</param>
    public StatusRecord(byte[] bytes) : base(bytes)
    {
    }

    #region Implementation of IStatusRecord

    /// <summary>
    ///     状态目录
    /// </summary>
    [Property(Order = 0)]
    public byte Category { get; set; }

    /// <summary>
    ///     状态项目
    /// </summary>
    [Property(Order = 1)]
    public byte Status { get; set; }

    #endregion
}