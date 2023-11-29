namespace Artemis.Data.Core.Fundamental.Kit;

/// <summary>
///     字节码转换
/// </summary>
public static class ByteUtility
{
    #region Boolean

    /// <summary>
    ///     bool转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <returns>4bit bytes</returns>
    public static byte[] BooleanToBytes(bool value)
    {
        return BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转bool
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>bool</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static bool BytesToBoolean(byte[] bytes)
    {
        if (bytes.Length != 1) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.ToBoolean(bytes);
    }

    #endregion

    #region Short

    /// <summary>
    ///     short转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte)(value >> 8),
    ///     (byte)value
    /// };
    /// ]]>
    /// </code>
    /// <returns>16bit bytes</returns>
    public static byte[] ShortToBytes(short value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     short转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte)(value >> 8),
    ///     (byte)value
    /// };
    /// ]]>
    /// </code>
    /// <returns>16bit bytes</returns>
    public static byte[] UShortToBytes(ushort value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转short
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return (bytes[0] << 24) |
    ///        bytes[1] << 16;
    /// ]]>
    /// </code>
    /// <returns>short</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static short BytesToShort(byte[] bytes)
    {
        if (bytes.Length != 2) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToInt16(bytes.Reverse().ToArray())
            : BitConverter.ToInt16(bytes);
    }

    /// <summary>
    ///     bytes转short
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return (bytes[0] << 24) |
    ///        bytes[1] << 16;
    /// ]]>
    /// </code>
    /// <returns>short</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ushort BytesToUShort(byte[] bytes)
    {
        if (bytes.Length != 2) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToUInt16(bytes.Reverse().ToArray())
            : BitConverter.ToUInt16(bytes);
    }

    #endregion

    #region Int

    /// <summary>
    ///     int转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte)(value >> 24),
    ///     (byte)(value >> 16),
    ///     (byte)(value >> 8),
    ///     (byte)value
    /// };
    /// ]]>
    /// </code>
    /// <returns>32bit bytes</returns>
    public static byte[] IntToBytes(int value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     uint转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte)(value >> 24),
    ///     (byte)(value >> 16),
    ///     (byte)(value >> 8),
    ///     (byte)value
    /// };
    /// ]]>
    /// </code>
    /// <returns>32bit bytes</returns>
    public static byte[] UIntToBytes(uint value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转int
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return (bytes[0] << 24) |
    ///        (bytes[1] << 16) |
    ///        (bytes[2] << 8) |
    ///        bytes[3];
    /// ]]>
    /// </code>
    /// <returns>integer</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static int BytesToInt(byte[] bytes)
    {
        if (bytes.Length != 4) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToInt32(bytes.Reverse().ToArray())
            : BitConverter.ToInt32(bytes);
    }

    /// <summary>
    ///     bytes转uint
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return (uint)((bytes[0] << 24) |
    ///               (bytes[1] << 16) |
    ///               (bytes[2] << 8) |
    ///               bytes[3]);
    /// ]]>
    /// </code>
    /// <returns>integer</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static uint BytesToUInt(byte[] bytes)
    {
        if (bytes.Length != 4) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToUInt32(bytes.Reverse().ToArray())
            : BitConverter.ToUInt32(bytes);
    }

    #endregion

    #region Long

    /// <summary>
    ///     long转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte) (value >> 56),
    ///     (byte) (value >> 48),
    ///     (byte) (value >> 40),
    ///     (byte) (value >> 32),
    ///     (byte) (value >> 24),
    ///     (byte) (value >> 16),
    ///     (byte) (value >> 8),
    ///     (byte) value
    /// };
    /// ]]>
    /// </code>
    /// <returns>64bit bytes</returns>
    public static byte[] LongToBytes(long value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     ulong转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <code>
    /// <![CDATA[
    /// return new[]
    /// {
    ///     (byte)(value >> 56),
    ///     (byte)(value >> 48),
    ///     (byte)(value >> 40),
    ///     (byte)(value >> 32),
    ///     (byte)(value >> 24),
    ///     (byte)(value >> 16),
    ///     (byte)(value >> 8),
    ///     (byte)value
    /// };]]>
    /// </code>
    /// <returns>64bit bytes</returns>
    public static byte[] ULongToBytes(ulong value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转long
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return ((long)bytes[0] << 56) |
    ///        ((long)bytes[1] << 48) |
    ///        ((long)bytes[2] << 40) |
    ///        ((long)bytes[3] << 32) |
    ///        ((long)bytes[4] << 24) |
    ///        ((long)bytes[5] << 16) |
    ///        ((long)bytes[6] << 8) |
    ///        bytes[7];
    /// ]]>
    /// </code>
    /// <returns>long</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static long BytesToLong(byte[] bytes)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToInt64(bytes.Reverse().ToArray())
            : BitConverter.ToInt64(bytes);
    }

    /// <summary>
    ///     bytes转ulong
    /// </summary>
    /// <param name="bytes"></param>
    /// <code>
    /// <![CDATA[
    /// return ((ulong)bytes[0] << 56) |
    ///        ((ulong)bytes[1] << 48) |
    ///        ((ulong)bytes[2] << 40) |
    ///        ((ulong)bytes[3] << 32) |
    ///        ((ulong)bytes[4] << 24) |
    ///        ((ulong)bytes[5] << 16) |
    ///        ((ulong)bytes[6] << 8) |
    ///        bytes[7];
    /// ]]>
    /// </code>
    /// <returns>long</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ulong BytesToULong(byte[] bytes)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToUInt64(bytes.Reverse().ToArray())
            : BitConverter.ToUInt64(bytes);
    }

    #endregion

    #region float

    /// <summary>
    ///     float转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <returns>64bit bytes</returns>
    public static byte[] FloatToBytes(float value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转float
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>single</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static float BytesToFloat(byte[] bytes)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToSingle(bytes.Reverse().ToArray())
            : BitConverter.ToSingle(bytes);
    }

    #endregion

    #region double

    /// <summary>
    ///     float转bytes
    /// </summary>
    /// <param name="value"></param>
    /// <returns>64bit bytes</returns>
    public static byte[] DoubleToBytes(double value)
    {
        return BitConverter.IsLittleEndian
            ? BitConverter.GetBytes(value).Reverse().ToArray()
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     bytes转float
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>double</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static double BytesToDouble(byte[] bytes)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes));

        return BitConverter.IsLittleEndian
            ? BitConverter.ToDouble(bytes.Reverse().ToArray())
            : BitConverter.ToDouble(bytes);
    }

    #endregion
}