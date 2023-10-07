using System.Security.Cryptography;

namespace Artemis.Data.Core.Fundamental.Kit;

/// <summary>
///     标准哈希类(密码实现)
/// </summary>
internal class ArtemisHasher
{
    /// <summary>
    ///     默认实现
    /// </summary>
    private static readonly Lazy<ArtemisHasher> Default = new(() => new ArtemisHasher());

    /// <summary>
    ///     密码哈希器
    /// </summary>
    /// <param name="iterationCount">迭代计数</param>
    private ArtemisHasher(int iterationCount = 100000)
    {
        IterationCount = iterationCount;
        if (IterationCount < 1) IterationCount = 100_000;

        Rng = RandomNumberGenerator.Create();
    }

    /* =======================
     * 哈希格式
     * =======================
     * 带HMAC-SHA512的PBKDF2摘要, 128-bit 盐, 256-bit 子钥, 100000 次迭代
     * 格式: { 0x01, 伪随机 (UInt32), 迭代次数 (UInt32), 盐长度 (UInt32), 盐, 子钥 }
     * (高位优先编码)
     */

    /// <summary>
    ///     迭代计数器
    /// </summary>
    private int IterationCount { get; }

    /// <summary>
    ///     随机数生成器
    /// </summary>
    private RandomNumberGenerator Rng { get; }

    /// <summary>
    ///     创建实例
    /// </summary>
    /// <returns>ArtemisHasher</returns>
    public static ArtemisHasher Create()
    {
        return Default.Value;
    }

    /// <summary>
    ///     生成哈希
    /// </summary>
    /// <param name="input">原文</param>
    /// <returns>哈希值</returns>
    public string ComputeHash(string input)
    {
        ThrowIfNull(input);

        return Convert.ToBase64String(ComputeHashBytes(input));
    }

    /// <summary>
    ///     计算哈希字节码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private byte[] ComputeHashBytes(string input)
    {
        return ComputeHashBytes(input, Rng);
    }

    /// <summary>
    ///     计算哈希字节码
    /// </summary>
    /// <param name="input">原文</param>
    /// <param name="rng">随机数生成器</param>
    /// <returns></returns>
    private byte[] ComputeHashBytes(string input, RandomNumberGenerator rng)
    {
        return ComputeHashBytes(input, rng,
            ArtemisKeyDerivationPrf.HmacSha512,
            IterationCount,
            128 / 8,
            256 / 8);
    }

    /// <summary>
    ///     生成哈希字节码
    /// </summary>
    /// <param name="input">输入原文</param>
    /// <param name="rng">随机数生成器</param>
    /// <param name="prf">PRF</param>
    /// <param name="iterationCount">迭代计数器</param>
    /// <param name="saltSize">盐长度</param>
    /// <param name="numBytesRequested">请求长度</param>
    /// <returns></returns>
    private static byte[] ComputeHashBytes(
        string input,
        RandomNumberGenerator rng,
        ArtemisKeyDerivationPrf prf,
        int iterationCount,
        int saltSize,
        int numBytesRequested)
    {
        // 生成摘要
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = Pbkdf2(input, salt, prf, iterationCount, numBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        // 摘要格式标记
        outputBytes[0] = 0x01;
        WriteBytesOrder(outputBytes, 1, (uint)prf);
        WriteBytesOrder(outputBytes, 5, (uint)iterationCount);
        WriteBytesOrder(outputBytes, 9, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
        return outputBytes;
    }

    /// <summary>
    ///     检验哈希码
    /// </summary>
    /// <param name="hashedText">哈希后的值</param>
    /// <param name="providedText">原文</param>
    /// <returns>bool success, bool needRehash</returns>
    /// <remarks>时间一致</remarks>
    public (bool success, bool needRehash) VerifyHash(
        string hashedText,
        string providedText)
    {
        ThrowIfNull(hashedText);
        ThrowIfNull(providedText);

        var decodeHashedText = Convert.FromBase64String(hashedText);

        // 读取散列格式标记
        if (decodeHashedText.Length == 0) return (false, false);
        switch (decodeHashedText[0])
        {
            case 0x01:
                if (VerifyHash(decodeHashedText, providedText, out var embeddedIterationCount, out var prf))
                {
                    // 比较迭代计数器
                    if (embeddedIterationCount < IterationCount) return (true, true);

                    // SHA512. 比较PRF标记
                    return prf is ArtemisKeyDerivationPrf.HmacSha1 or ArtemisKeyDerivationPrf.HmacSha256
                        ? (true, true)
                        : (true, false);
                }

                return (false, false);

            default:
                //未知格式标记
                return (false, false);
        }
    }

    /// <summary>
    ///     校验密码哈希
    /// </summary>
    /// <param name="hashedPassword">密码哈希</param>
    /// <param name="password">密码</param>
    /// <param name="iterationCount">迭代计数器</param>
    /// <param name="prf">PRF</param>
    /// <returns></returns>
    private static bool VerifyHash(
        byte[] hashedPassword,
        string password,
        out int iterationCount,
        out ArtemisKeyDerivationPrf prf)
    {
        iterationCount = default;
        prf = default;

        try
        {
            // 读取头信息
            prf = (ArtemisKeyDerivationPrf)ReadBytesOrder(hashedPassword, 1);
            iterationCount = (int)ReadBytesOrder(hashedPassword, 5);
            var saltLength = (int)ReadBytesOrder(hashedPassword, 9);

            // 读取盐: 不超过128比特
            if (saltLength < 128 / 8) return false;
            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            // 读取子钥 (剩余载荷): 不超过128比特
            var subKeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subKeyLength < 128 / 8) return false;
            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            // 摘要输入段并验证
            var actualSubKey = Pbkdf2(password, salt, prf, iterationCount, subKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            // 载荷格式错误时才会发生...
            return false;
        }
    }

    /// <summary>
    ///     若参数为空则抛出异常
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void ThrowIfNull(object? argument, string? paramName = null)
    {
        if (argument is null) throw new ArgumentNullException(paramName);
    }

    #region ArtemisKeyDerivationPrf

    /// <summary>
    ///     指定应用于密钥派生算法的 PRF。
    /// </summary>
    private enum ArtemisKeyDerivationPrf
    {
        /// <summary>
        ///     HMAC (RFC 2104) SHA1 (FIPS 180-4).
        /// </summary>
        HmacSha1,

        /// <summary>
        ///     HMAC (RFC 2104) SHA256 (FIPS 180-4).
        /// </summary>
        HmacSha256,

        /// <summary>
        ///     HMAC (RFC 2104) SHA512 (FIPS 180-4).
        /// </summary>
        HmacSha512
    }

    #endregion

    #region KeyDerivation

    /// <summary>使用 PBKDF2 算法执行密钥派生</summary>
    /// <param name="input">原文</param>
    /// <param name="salt">盐</param>
    /// <param name="prf">伪随机函数(pseudo-random function)</param>
    /// <param name="iterationCount">哈希过程中prf的迭代次数</param>
    /// <param name="numBytesRequested">密钥长度(字节长度)</param>
    /// <returns>派生密钥</returns>
    /// <remarks>PBKDF2 算法在 RFC 2898 中指定。</remarks>
    private static byte[] Pbkdf2(
        string input,
        byte[] salt,
        ArtemisKeyDerivationPrf prf,
        int iterationCount,
        int numBytesRequested)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));
        if (salt == null)
            throw new ArgumentNullException(nameof(salt));
        if (prf is < ArtemisKeyDerivationPrf.HmacSha1 or > ArtemisKeyDerivationPrf.HmacSha512)
            throw new ArgumentOutOfRangeException(nameof(prf));
        if (iterationCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(iterationCount));
        if (numBytesRequested <= 0)
            throw new ArgumentOutOfRangeException(nameof(numBytesRequested));
        return DeriveKey(input, salt, prf, iterationCount, numBytesRequested);
    }

    /// <summary>
    ///     派生密钥
    /// </summary>
    /// <param name="input"></param>
    /// <param name="salt"></param>
    /// <param name="prf"></param>
    /// <param name="iterationCount"></param>
    /// <param name="numBytesRequested"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static byte[] DeriveKey(
        string input,
        byte[] salt,
        ArtemisKeyDerivationPrf prf,
        int iterationCount,
        int numBytesRequested)
    {
        var hashAlgorithm = prf switch
        {
            ArtemisKeyDerivationPrf.HmacSha1 => HashAlgorithmName.SHA1,
            ArtemisKeyDerivationPrf.HmacSha256 => HashAlgorithmName.SHA256,
            ArtemisKeyDerivationPrf.HmacSha512 => HashAlgorithmName.SHA512,
            _ => throw new ArgumentOutOfRangeException(nameof(prf))
        };
        return Rfc2898DeriveBytes.Pbkdf2(input, salt, iterationCount, hashAlgorithm, numBytesRequested);
    }

    #endregion

    #region ReqdAndWrite

    /// <summary>
    ///     读取字节序
    /// </summary>
    /// <param name="buffer">接收器</param>
    /// <param name="offset">偏移量</param>
    /// <returns></returns>
    private static uint ReadBytesOrder(IReadOnlyList<byte> buffer, int offset)
    {
        return ((uint)buffer[offset + 0] << 24)
               | ((uint)buffer[offset + 1] << 16)
               | ((uint)buffer[offset + 2] << 8)
               | buffer[offset + 3];
    }

    /// <summary>
    ///     写入字节序
    /// </summary>
    /// <param name="buffer">接收器</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    private static void WriteBytesOrder(IList<byte> buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }

    #endregion
}