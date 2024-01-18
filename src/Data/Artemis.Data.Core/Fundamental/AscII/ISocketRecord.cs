namespace Artemis.Data.Core.Socket;

/// <summary>
///     Socket记录接口
/// </summary>
public interface ISocketRecord
{
    /// <summary>
    ///     报文开始位
    /// </summary>
    byte Stx { get; init; }

    /// <summary>
    ///     报文头段
    /// </summary>
    byte[]? Head { get; set; }

    /// <summary>
    ///     报文头记录
    /// </summary>
    IHeadRecord? HeadRecord { get; set; }

    /// <summary>
    ///     报文内容段
    /// </summary>
    byte[]? Content { get; set; }

    /// <summary>
    ///     报文校验段
    /// </summary>
    byte[]? Check { get; set; }

    /// <summary>
    ///     报文校验段
    /// </summary>
    ICheckRecord? CheckRecord { get; set; }

    /// <summary>
    ///     报文结束位
    /// </summary>
    byte Etx { get; init; }
}

/// <summary>
///     校验记录接口
/// </summary>
public interface ICheckRecord
{
    /// <summary>
    ///     校验字节段
    /// </summary>
    byte[] Check { get; set; }

    /// <summary>
    ///     求和校验位
    /// </summary>
    byte CheckSum { get; set; }

    /// <summary>
    ///     异或校验位
    /// </summary>
    byte CheckXor { get; set; }
}

/// <summary>
///     头记录接口
/// </summary>
public interface IHeadRecord
{
    /// <summary>
    ///     内容长度字节段
    /// </summary>
    byte[] ContentLength { get; init; }

    /// <summary>
    ///     长度
    /// </summary>
    long Length { get; init; }

    /// <summary>
    ///     指令记录
    /// </summary>
    ICommandRecord CommandRecord { get; init; }

    /// <summary>
    ///     状态记录
    /// </summary>
    IStatusRecord StatusRecord { get; init; }
}

/// <summary>
///     指令记录接口
/// </summary>
public interface ICommandRecord
{
    /// <summary>
    ///     指令字节段
    /// </summary>
    byte[] Command { get; init; }

    /// <summary>
    ///     指令目录
    /// </summary>
    byte CommandCategory { get; init; }

    /// <summary>
    ///     指令项目
    /// </summary>
    byte CommandItem { get; init; }
}

/// <summary>
///     状态记录接口
/// </summary>
public interface IStatusRecord
{
    /// <summary>
    ///     状态字节段
    /// </summary>
    byte[] Status { get; init; }

    /// <summary>
    ///     状态目录
    /// </summary>
    byte StatusCategory { get; init; }

    /// <summary>
    ///     状态项目
    /// </summary>
    byte StatusItem { get; init; }
}