namespace Artemis.Data.Core.Socket;

/// <summary>
///     Socket记录
/// </summary>
public record SocketRecord : ISocketRecord
{
    #region Implementation of ISocketRecord

    /// <summary>
    ///     报文开始位
    /// </summary>
    public byte Stx { get; init; }

    /// <summary>
    ///     报文头段
    /// </summary>
    public byte[]? Head { get; set; }

    /// <summary>
    ///     报文头记录
    /// </summary>
    public IHeadRecord? HeadRecord { get; set; }

    /// <summary>
    ///     报文内容段
    /// </summary>
    public byte[]? Content { get; set; }

    /// <summary>
    ///     报文校验段
    /// </summary>
    public byte[]? Check { get; set; }

    /// <summary>
    ///     报文校验段
    /// </summary>
    public ICheckRecord? CheckRecord { get; set; }

    /// <summary>
    ///     报文结束位
    /// </summary>
    public byte Etx { get; init; }

    #endregion
}

/// <summary>
///     校验记录
/// </summary>
public record CheckRecord : ICheckRecord
{
    #region Implementation of ICheckRecord

    /// <summary>
    ///     校验字节段
    /// </summary>
    public byte[] Check { get; set; } = null!;

    /// <summary>
    ///     求和校验位
    /// </summary>
    public byte CheckSum { get; set; }

    /// <summary>
    ///     异或校验位
    /// </summary>
    public byte CheckXor { get; set; }

    #endregion
}

/// <summary>
///     头记录
/// </summary>
public record HeadRecord : IHeadRecord
{
    #region Implementation of IContentLengthRecord

    /// <summary>
    ///     内容长度字节段
    /// </summary>
    public byte[] ContentLength { get; init; } = null!;

    /// <summary>
    ///     长度
    /// </summary>
    public long Length { get; init; }

    /// <summary>
    ///     指令记录
    /// </summary>
    public ICommandRecord CommandRecord { get; init; } = null!;

    /// <summary>
    ///     状态记录
    /// </summary>
    public IStatusRecord StatusRecord { get; init; } = null!;

    #endregion
}

/// <summary>
///     指令记录
/// </summary>
public record CommandRecord : ICommandRecord
{
    /// <summary>
    ///     指令字节段
    /// </summary>
    public byte[] Command { get; init; } = null!;

    /// <summary>
    ///     指令目录
    /// </summary>
    public byte CommandCategory { get; init; }

    /// <summary>
    ///     指令项目
    /// </summary>
    public byte CommandItem { get; init; }
}

/// <summary>
///     状态记录
/// </summary>
public record StatusRecord : IStatusRecord
{
    /// <summary>
    ///     状态字节段
    /// </summary>
    public byte[] Status { get; init; } = null!;

    /// <summary>
    ///     状态目录
    /// </summary>
    public byte StatusCategory { get; init; }

    /// <summary>
    ///     状态项目
    /// </summary>
    public byte StatusItem { get; init; }
}