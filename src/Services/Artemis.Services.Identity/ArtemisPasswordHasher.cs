using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Artemis.Services.Identity;

/// <summary>
/// 标准密码哈希类
/// </summary>
public class ArtemisPasswordHasher
{
    /* =======================
     * HASHED PASSWORD FORMATS
     * =======================
     *
     * Version 3:
     * PBKDF2 with HMAC-SHA512, 128-bit salt, 256-bit subKey, 100000 iterations.
     * Format: { 0x01, prf (UInt32), iteration count (UInt32), salt length (UInt32), salt, subKey }
     * (All UInt32s are stored big-endian.)
     */

    /// <summary>
    /// 迭代计数器
    /// </summary>
    private int IterationCount { get; }

    /// <summary>
    /// 随机数生成器
    /// </summary>
    private RandomNumberGenerator Rng { get; }

    /// <summary>
    /// 密码哈希器
    /// </summary>
    /// <param name="iterationCount">迭代计数</param>
    public ArtemisPasswordHasher(int iterationCount = 100000)
    {
        IterationCount = iterationCount;
        if (IterationCount < 1)
        {
            IterationCount = 100_000;
        }

        Rng = RandomNumberGenerator.Create();
    }

    /// <summary>
    /// 生成密码哈希
    /// </summary>
    /// <param name="password">密码原文</param>
    /// <returns>密码哈希值</returns>
    public virtual string HashPassword(string password)
    {
        ThrowIfNull(password);

        return Convert.ToBase64String(HashPassword(password, Rng));
    }

    /// <summary>
    /// 计算密码哈希
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="rng">随机数生成器</param>
    /// <returns></returns>
    private byte[] HashPassword(string password, RandomNumberGenerator rng)
    {
        return HashPassword(password, rng,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: IterationCount,
            saltSize: 128 / 8,
            numBytesRequested: 256 / 8);
    }

    /// <summary>
    ///  生成密码哈希
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="rng">随机数生成器</param>
    /// <param name="prf">PRF</param>
    /// <param name="iterationCount">迭代计数器</param>
    /// <param name="saltSize">盐长度</param>
    /// <param name="numBytesRequested">请求长度</param>
    /// <returns></returns>
    private static byte[] HashPassword(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterationCount, int saltSize, int numBytesRequested)
    {
        // Produce a version 3 (see comment above) text hash.
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, numBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01; // format marker
        WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 5, (uint)iterationCount);
        WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
        return outputBytes;
    }

    /// <summary>
    /// 检验密码哈希
    /// </summary>
    /// <param name="hashedPassword">用于存储的密码哈希值</param>
    /// <param name="providedPassword">为比较而提供的密码</param>
    /// <returns>HashVerificationResult</returns>
    /// <remarks>时间一致</remarks>
    public virtual HashVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        ThrowIfNull(hashedPassword);
        ThrowIfNull(providedPassword);

        var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        // read the format marker from the hashed password
        if (decodedHashedPassword.Length == 0)
        {
            return HashVerificationResult.Failed;
        }
        switch (decodedHashedPassword[0])
        {
            case 0x01:
                if (VerifyHashedPassword(decodedHashedPassword, providedPassword, out var embeddedIterationCount, out var prf))
                {
                    // If this hasher was configured with a higher iteration count, change the entry now.
                    if (embeddedIterationCount < IterationCount)
                    {
                        return HashVerificationResult.SuccessRehashNeeded;
                    }

                    // V3 now requires SHA512. If the old PRF is SHA1 or SHA256, upgrade to SHA512 and rehash.
                    if (prf == KeyDerivationPrf.HMACSHA1 || prf == KeyDerivationPrf.HMACSHA256)
                    {
                        return HashVerificationResult.SuccessRehashNeeded;
                    }

                    return HashVerificationResult.Success;
                }
                else
                {
                    return HashVerificationResult.Failed;
                }

            default:
                return HashVerificationResult.Failed; // unknown format marker
        }
    }

    /// <summary>
    /// 校验密码哈希
    /// </summary>
    /// <param name="hashedPassword">密码哈希</param>
    /// <param name="password">密码</param>
    /// <param name="iterationCount">迭代计数器</param>
    /// <param name="prf">PRF</param>
    /// <returns></returns>
    private static bool VerifyHashedPassword(byte[] hashedPassword, string password, out int iterationCount, out KeyDerivationPrf prf)
    {
        iterationCount = default;
        prf = default;

        try
        {
            // Read header information
            prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
            iterationCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            // Read the subKey (the rest of the payload): must be >= 128 bits
            var subKeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subKeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            // Hash the incoming password and verify it
            var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, subKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);

        }
        catch
        {
            // This should never occur except in the case of a malformed payload, where
            // we might go off the end of the array. Regardless, a malformed payload
            // implies verification failed.
            return false;
        }
    }

    /// <summary>
    /// 拂去网络字节序
    /// </summary>
    /// <param name="buffer">接收器</param>
    /// <param name="offset">偏移量</param>
    /// <returns></returns>
    private static uint ReadNetworkByteOrder(IReadOnlyList<byte> buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
            | ((uint)(buffer[offset + 1]) << 16)
            | ((uint)(buffer[offset + 2]) << 8)
            | buffer[offset + 3];
    }

    /// <summary>
    /// 写入网络字节序
    /// </summary>
    /// <param name="buffer">接收器</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    private static void WriteNetworkByteOrder(IList<byte> buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }

    /// <summary>
    /// 若参数为空则抛出异常
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void ThrowIfNull(object? argument, string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}

/// <summary>
/// 密码验证结果
/// </summary>
public enum HashVerificationResult
{
    /// <summary>
    /// 密码验证失败
    /// </summary>
    Failed = 0,

    /// <summary>
    /// 密码验证成功
    /// </summary>
    Success = 1,

    /// <summary>
    /// 密码验证成功，但需要重新哈希
    /// </summary>
    SuccessRehashNeeded = 2
}