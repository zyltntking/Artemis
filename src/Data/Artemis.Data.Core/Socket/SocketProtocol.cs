namespace Artemis.Data.Core.Socket;

/// <summary>
///     Socket协议
/// </summary>
public class SocketProtocol
{
    /// <summary>
    ///     生成指令
    /// </summary>
    /// <param name="commandCategory">指令目录</param>
    /// <param name="commandItem">指令项目</param>
    /// <returns>生成的指令</returns>
    public static byte[] GeneratorCommand(byte commandCategory, byte commandItem)
    {
        var command = new byte[ProtocolSetting.CommandLength];
        command[0] = commandCategory;
        command[1] = commandItem;
        return command;
    }

    /// <summary>
    ///     生成状态
    /// </summary>
    /// <param name="statusCategory">状态目录</param>
    /// <param name="statusItem">状态项目</param>
    /// <returns>生成的状态</returns>
    public static byte[] GeneratorStatus(byte statusCategory, byte statusItem)
    {
        var status = new byte[ProtocolSetting.StatusLength];
        status[0] = statusCategory;
        status[1] = statusItem;
        return status;
    }

    /// <summary>
    ///     生成校验段
    /// </summary>
    /// <param name="checkSum">求和校验位</param>
    /// <param name="checkXor">异或校验位</param>
    /// <returns></returns>
    /// <remarks>|CHECKSUM|CHECKXOR|</remarks>
    public static byte[] GeneratorCheck(byte checkSum, byte checkXor)
    {
        var check = new byte[ProtocolSetting.CheckLength];
        check[0] = checkSum;
        check[1] = checkXor;
        return check;
    }

    /// <summary>
    ///     分离头
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">报文长度异常</exception>
    /// <remarks>|STX|HEAD|OTHER|</remarks>
    private static byte[] SeparateHead(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        const int offset = ProtocolSetting.StxLength;

        var length = ProtocolSetting.HeadLength;

        var result = new byte[length];

        Array.Copy(message.ToArray(), offset, result, 0, length);

        return result;
    }

    /// <summary>
    ///     分离Body
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">报文长度异常</exception>
    /// <remarks>|STX|HEAD|BODY|CHECK|ETX|</remarks>
    private static byte[] SeparateBody(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        var offset = ProtocolSetting.StxLength + ProtocolSetting.HeadLength;

        var length = message.LongCount() - ProtocolSetting.CheckLength - ProtocolSetting.EtxLength;

        var result = new byte[length];

        Array.Copy(message.ToArray(), offset, result, 0, length);

        return result;
    }

    /// <summary>
    ///     分离头和报文内容
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">报文长度异常</exception>
    /// <remarks>|STX|HEAD|OTHER|</remarks>
    private static byte[] SeparateHeadAndBody(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        const int offset = ProtocolSetting.StxLength;

        var length = message.LongCount() - ProtocolSetting.CheckLength - ProtocolSetting.EtxLength;

        var result = new byte[length];

        Array.Copy(message.ToArray(), offset, result, 0, length);

        return result;
    }

    /// <summary>
    ///     分离校验位
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <remarks>|OTHER|CHECK|ETX|</remarks>
    private static byte[] SeparateCheck(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        var offset = message.LongCount() - ProtocolSetting.CheckLength - ProtocolSetting.EtxLength;

        var length = ProtocolSetting.CheckLength;

        var result = new byte[length];

        Array.Copy(message.ToArray(), offset, result, 0, length);

        return result;
    }

    /// <summary>
    ///     判断报文头
    /// </summary>
    /// <param name="message">报文</param>
    /// <returns>判断结果</returns>
    private static bool IsStx(IReadOnlyList<byte> message)
    {
        return message[0] == AsciiControl.Stx;
    }

    /// <summary>
    ///     判断报文尾
    /// </summary>
    /// <param name="message">报文</param>
    /// <returns>判断结果</returns>
    private static bool IsEtx(IReadOnlyList<byte> message)
    {
        return message[^1] == AsciiControl.Etx;
    }

    /// <summary>
    ///     获取求和校验位
    /// </summary>
    /// <param name="content">报文内容</param>
    /// <returns></returns>
    private static byte GetCheckSum(IEnumerable<byte> content)
    {
        var hash = new HashProvider();
        var md5 = hash.Md5Hash(content.ToArray());
        var length = md5.Length;
        var checkSum = 0;
        for (var i = 0; i < length; i++) checkSum += md5[i];
        checkSum /= 0x100;
        return (byte)checkSum;
    }

    /// <summary>
    ///     获取异或校验位
    /// </summary>
    /// <param name="headAndContent">报文内容</param>
    /// <returns></returns>
    private static byte GetCheckXor(IEnumerable<byte> headAndContent)
    {
        var hash = new HashProvider();
        var md5 = hash.Md5Hash(headAndContent.ToArray());
        var length = md5.Length;
        var checkXor = 0;
        for (var i = 0; i < length; i++) checkXor ^= md5[i];
        return (byte)checkXor;
    }

    /// <summary>
    ///     验证报文
    /// </summary>
    /// <param name="message">报文</param>
    /// <returns></returns>
    public static bool Verify(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        if (!IsStx(message)) return false;

        if (!IsEtx(message)) return false;

        var head = SeparateHead(message);

        var headRecord = GetHeadRecord(head);

        var content = SeparateBody(message);

        if (headRecord.Length != content.LongLength) return false;

        var check = SeparateCheck(message);

        var checkRecord = GetCheckRecord(check);

        var checkSum = GetCheckSum(content);

        if (checkRecord.CheckSum != checkSum) return false;

        var headAndBody = SeparateHeadAndBody(message);

        var checkXor = GetCheckXor(headAndBody);

        if (checkRecord.CheckXor != checkXor) return false;

        return true;
    }

    /// <summary>
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public static bool Verify(ISocketRecord record)
    {
        if (record.Stx != AsciiControl.Stx) return false;

        if (record.Etx != AsciiControl.Etx) return false;

        if (record.Head == null) return false;

        if (record.Check == null) return false;

        if (record is { Content: not null, HeadRecord: not null } &&
            record.HeadRecord.Length != record.Content.LongLength) return false;

        byte checkSum = 0;

        if (record.Content != null) checkSum = GetCheckSum(record.Content);

        if (record.CheckRecord != null && record.CheckRecord.CheckSum != checkSum) return false;

        var length = record.Head.Length + record.Content?.LongLength ?? 0;

        var headAndBodyArray = new byte[length];

        Array.Copy(record.Head, 0, headAndBodyArray, 0, record.Head.Length);

        if (record.Content != null)
            Array.Copy(record.Content, 0, headAndBodyArray, record.Head.Length, record.Content.LongLength);

        var checkXor = GetCheckXor(headAndBodyArray);

        if (record.CheckRecord != null && record.CheckRecord.CheckXor != checkXor) return false;

        return true;
    }

    /// <summary>
    ///     获取SocketRecord
    /// </summary>
    /// <param name="message">报文</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <remarks>|STX|HEAD|BODY|CHECK|ETX|</remarks>
    public static ISocketRecord GetSocketRecord(IReadOnlyList<byte> message)
    {
        if (message.LongCount() < ProtocolSetting.Shortest) throw new ArgumentException("报文长度不正确", nameof(message));

        var record = new SocketRecord
        {
            Stx = message[0],
            Etx = message[^1]
        };

        if (record is not { Stx: AsciiControl.Stx, Etx: AsciiControl.Etx }) return record;

        long offset = ProtocolSetting.StxLength;
        long length = ProtocolSetting.HeadLength;
        var headArray = new byte[length];
        Array.Copy(message.ToArray(), offset, headArray, 0, length);

        record.Head = headArray;
        record.HeadRecord = GetHeadRecord(headArray);

        var count = ProtocolSetting.Shortest + record.HeadRecord.Length;

        if (message.LongCount() != count) return record;

        offset += length;
        length = record.HeadRecord.Length;
        var bodyArray = new byte[length];
        Array.Copy(message.ToArray(), offset, bodyArray, 0, length);

        record.Content = bodyArray;

        offset += length;
        length = ProtocolSetting.CheckLength;
        var checkArray = new byte[length];
        Array.Copy(message.ToArray(), offset, checkArray, 0, length);

        record.Check = checkArray;
        record.CheckRecord = GetCheckRecord(checkArray);

        return record;
    }

    /// <summary>
    ///     获取报文头记录
    /// </summary>
    /// <param name="head">报文头字节码</param>
    /// <returns>报文头记录</returns>
    /// <exception cref="ArgumentException">指令长度异常</exception>
    /// <remarks>|ContentLength|CommandCategory|CommandItem|StatusCategory|StatusItem|</remarks>
    private static IHeadRecord GetHeadRecord(IReadOnlyList<byte> head)
    {
        if (head.Count != ProtocolSetting.HeadLength) throw new ArgumentException("指令长度不正确", nameof(head));

        var offset = 0;

        var length = ProtocolSetting.ContentCountLength;

        var contentLength = head.Skip(offset).Take(length).ToArray();

        offset += length;

        var command = head.Skip(offset).Take(length).ToArray();

        offset += length;

        var status = head.Skip(offset).Take(length).ToArray();

        return new HeadRecord
        {
            ContentLength = contentLength,
            Length = ByteParser.BytesToLong(contentLength),
            CommandRecord = GetCommandRecord(command),
            StatusRecord = GetStatusRecord(status)
        };
    }

    /// <summary>
    ///     获取指令记录
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <remarks>|CommandCategory|CommandItem|</remarks>
    private static ICommandRecord GetCommandRecord(IReadOnlyList<byte> command)
    {
        return new CommandRecord
        {
            Command = command.ToArray(),
            CommandCategory = command[0],
            CommandItem = command[1]
        };
    }

    /// <summary>
    ///     获取状态记录
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <remarks>|StatusCategory|StatusItem|</remarks>
    private static IStatusRecord GetStatusRecord(IReadOnlyList<byte> status)
    {
        return new StatusRecord
        {
            Status = status.ToArray(),
            StatusCategory = status[0],
            StatusItem = status[1]
        };
    }

    /// <summary>
    ///     获取校验记录
    /// </summary>
    /// <param name="check"></param>
    /// <returns></returns>
    /// <remarks>|CheckSum|CheckXor|</remarks>
    private static ICheckRecord GetCheckRecord(IReadOnlyList<byte> check)
    {
        return new CheckRecord
        {
            Check = check.ToArray(),
            CheckSum = check[0],
            CheckXor = check[1]
        };
    }
}

/// <summary>
///     协议配置
/// </summary>
internal static class ProtocolSetting
{
    /// <summary>
    ///     起始标识长度
    /// </summary>
    public const int StxLength = 1;

    /// <summary>
    ///     报文内容长度
    /// </summary>
    internal const int ContentCountLength = 8;

    /// <summary>
    ///     指令目录长度
    /// </summary>
    internal const int CommandCategoryLength = 1;

    /// <summary>
    ///     指令项目长度
    /// </summary>
    internal const int CommandItemLength = 1;

    /// <summary>
    ///     状态目录长度
    /// </summary>
    internal const int StatusCategoryLength = 1;

    /// <summary>
    ///     状态项目长度
    /// </summary>
    internal const int StatusItemLength = 1;

    /// <summary>
    ///     求和校验位长度
    /// </summary>
    internal const int CheckSumLength = 1;

    /// <summary>
    ///     异或校验位长度
    /// </summary>
    internal const int CheckXorLength = 1;

    /// <summary>
    ///     结束标识
    /// </summary>
    public const int EtxLength = 1;

    /// <summary>
    ///     指令长度
    /// </summary>
    internal static readonly int CommandLength = CommandCategoryLength + CommandItemLength;

    /// <summary>
    ///     状态长度
    /// </summary>
    internal static readonly int StatusLength = StatusCategoryLength + StatusItemLength;

    /// <summary>
    ///     报文头长度
    /// </summary>
    public static readonly int HeadLength = ContentCountLength + CommandLength + StatusLength;

    /// <summary>
    ///     校验位长度
    /// </summary>
    public static readonly int CheckLength = CheckSumLength + CheckXorLength;

    /// <summary>
    ///     最短长度
    /// </summary>
    public static readonly int Shortest = StxLength + HeadLength + CheckLength + EtxLength;
}