namespace Artemis.Data.Core.Fundamental.Socket;

/// <summary>
///     字节码转换器
/// </summary>
internal static class BytesConverter
{
    /// <summary>
    ///     修正字节码长度
    /// </summary>
    /// <param name="bytes">输入字节码</param>
    /// <param name="length">修正长度</param>
    /// <param name="left">自左边开始填充</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static byte[] FixedBytes(this byte[] bytes, long length, bool left = true)
    {
        var rest = length - bytes.Length;

        if (rest < 0) throw new ArgumentException("字节码长度大于给定的长度");

        if (rest == 0) return bytes;

        var result = new byte[length];

        Array.Copy(bytes, 0, result, left ? rest : 0, bytes.LongLength);

        return result;
    }

    #region byte&bytes

    /// <summary>
    ///     byte转字节码
    /// </summary>
    /// <param name="byteCode">byte</param>
    /// <returns></returns>
    public static byte[] ToBytes(this byte byteCode)
    {
        return [byteCode];
    }

    /// <summary>
    ///     自字节码转byte
    /// </summary>
    /// <param name="bytes">输入字节码</param>
    /// <returns></returns>
    public static byte ToByte(this byte[] bytes)
    {
        return bytes[0];
    }

    #endregion

    #region long&bytes

    /// <summary>
    ///     long转字节码
    /// </summary>
    /// <param name="input">long</param>
    /// <param name="useBigEndian">是否启动高位优先</param>
    /// <returns></returns>
    public static byte[] ToBytes(this long input, bool useBigEndian = true)
    {
        var bytes = BitConverter.GetBytes(input);

        return useBigEndian ? bytes.Reverse().ToArray() : bytes;
    }

    /// <summary>
    ///     自字节码转long
    /// </summary>
    /// <param name="bytes">输入字节码</param>
    /// <param name="useBigEndian">是否启用高位优先</param>
    /// <returns></returns>
    public static long ToLong(this byte[] bytes, bool useBigEndian = true)
    {
        return useBigEndian ? BitConverter.ToInt64(bytes.Reverse().ToArray(), 0) : BitConverter.ToInt64(bytes, 0);
    }

    #endregion
}