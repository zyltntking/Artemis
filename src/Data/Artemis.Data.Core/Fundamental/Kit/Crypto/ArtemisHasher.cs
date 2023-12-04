using System.Security.Cryptography;

namespace Artemis.Data.Core.Fundamental.Kit.Crypto;

/*
 * =======
 * 摘要规则
 * =======
 * 格式：{版本号, 摘要算法, 迭代次数, 盐长度, 盐, 摘要}
 * 长度：{8bit, 8bit, 8bit, 32bit, 32bit, -, -}
 * 字节位：{0, 1-4, 5-8, 9-12, 13-x, x-y}
 * (高位优先编码)
 */

/// <summary>
///     密码摘要算法
/// </summary>
public class ArtemisHasher
{
    /// <summary>
    ///     默认实现
    /// </summary>
    private static readonly Lazy<ArtemisHasher> Default = new(() => new ArtemisHasher());

    /// <summary>
    ///     密码哈希器
    /// </summary>
    private ArtemisHasher()
    {
    }

    private int Iterations => RandomNumberGenerator.GetInt32(100_000, 500_000);

    private int SaltSize => RandomNumberGenerator.GetInt32(16, 32);

    private int DerivedKeyLength => RandomNumberGenerator.GetInt32(32, 64);

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
        ArgumentNullException.ThrowIfNull(input);

        return Convert.ToBase64String(ComputeHashBytes(input));
    }

    /// <summary>
    ///     计算哈希字节码
    /// </summary>
    /// <param name="input">原文</param>
    /// <returns></returns>
    private byte[] ComputeHashBytes(string input)
    {
        return ComputeHashBytes(input,
            KeyDerivationPrf.Sha512,
            Iterations,
            SaltSize,
            DerivedKeyLength);
    }

    /// <summary>
    ///     生成哈希字节码
    /// </summary>
    /// <param name="input">输入原文</param>
    /// <param name="prf">摘要算法标识</param>
    /// <param name="iterations">迭代计数器</param>
    /// <param name="saltSize">盐长度</param>
    /// <param name="outputLength">摘要长度</param>
    /// <returns></returns>
    private byte[] ComputeHashBytes(
        string input,
        KeyDerivationPrf prf,
        int iterations,
        int saltSize,
        int outputLength)
    {
        // 生成盐
        var salt = RandomNumberGenerator.GetBytes(saltSize);

        // 派生密钥
        var derivedKey = Pbkdf2(input, salt, prf, iterations, outputLength);

        // 密钥版本
        var version = new byte[] { 0x01 };

        // 摘要算法
        var algorithm = ByteUtility.UIntToBytes((uint)prf);

        // 迭代次数
        var iterationBytes = ByteUtility.UIntToBytes((uint)iterations);

        // 盐长度
        var saltSizeBytes = ByteUtility.UIntToBytes((uint)saltSize);

        // 生成密码
        var password = version // 版本号
            .Concat(algorithm) // 摘要算法 
            .Concat(iterationBytes) // 迭代次数
            .Concat(saltSizeBytes) // 盐长度
            .Concat(salt) // 盐
            .Concat(derivedKey) // 派生密钥
            .ToArray();

        return password;
    }

    /// <summary>
    ///     检验哈希码
    /// </summary>
    /// <param name="hashedText">哈希后的值</param>
    /// <param name="providedText">比较字符串</param>
    /// <returns></returns>
    /// <remarks>保障时间一致</remarks>
    public bool VerifyHash(
        string hashedText,
        string providedText)
    {
        ArgumentNullException.ThrowIfNull(hashedText);
        ArgumentNullException.ThrowIfNull(providedText);

        var decodeHashedText = Convert.FromBase64String(hashedText);

        // 编码是否正确
        if (decodeHashedText.Length == 0) return false;

        return VerifyHash(decodeHashedText, providedText);
    }

    private bool VerifyHash(byte[] hashedPassword, string password)
    {
        try
        {
            // 读取头信息
            // 版本号
            // var version = hashedPassword[0];
            // 摘要算法
            var algorithmBytes = hashedPassword.Skip(1).Take(4).ToArray();
            var algorithm = (KeyDerivationPrf)ByteUtility.BytesToUInt(algorithmBytes);
            // 迭代次数
            var iterationBytes = hashedPassword.Skip(5).Take(4).ToArray();
            var iterations = (int)ByteUtility.BytesToUInt(iterationBytes);
            // 盐长度
            var saltLengthBytes = hashedPassword.Skip(9).Take(4).ToArray();
            var saltLength = (int)ByteUtility.BytesToUInt(saltLengthBytes);
            // 盐
            var salt = hashedPassword.Skip(13).Take(saltLength).ToArray();
            // 摘要
            var derivedKey = hashedPassword.Skip(13 + saltLength).ToArray();
            var derivedKeyLength = derivedKey.Length;

            // 摘要输入段
            var actualSubKey = Pbkdf2(password, salt, algorithm, iterations, derivedKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, derivedKey);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     派生算法标识
    /// </summary>
    private enum KeyDerivationPrf
    {
        /// <summary>
        ///     Md5
        /// </summary>
        Md5,

        /// <summary>
        ///     sha1
        /// </summary>
        Sha1,

        /// <summary>
        ///     sha256
        /// </summary>
        Sha256,

        /// <summary>
        ///     sha256
        /// </summary>
        Sha384,

        /// <summary>
        ///     sha512
        /// </summary>
        Sha512
    }

    #region KeyDerivation

    /// <summary>使用 PBKDF2 算法执行密钥派生</summary>
    /// <param name="input">原文</param>
    /// <param name="salt">盐</param>
    /// <param name="prf">伪随机函数(pseudo-random function)</param>
    /// <param name="iterations">哈希过程中prf的迭代次数</param>
    /// <param name="outputLength">密钥长度(字节长度)</param>
    /// <returns>派生密钥</returns>
    /// <remarks>PBKDF2 算法在 RFC 2898 中指定。</remarks>
    private byte[] Pbkdf2(
        string input,
        byte[] salt,
        KeyDerivationPrf prf,
        int iterations,
        int outputLength)
    {
        // 参数检查
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(salt);

        if (prf is < KeyDerivationPrf.Md5 or > KeyDerivationPrf.Sha512)
            throw new ArgumentOutOfRangeException(nameof(prf));
        if (iterations <= 0)
            throw new ArgumentOutOfRangeException(nameof(iterations));
        if (outputLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(outputLength));

        return DeriveKey(input, salt, prf, iterations, outputLength);
    }

    /// <summary>
    ///     派生密钥
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="salt">盐</param>
    /// <param name="prf">派生算法标识</param>
    /// <param name="iterations">迭代次数</param>
    /// <param name="outputLength">输出长度</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private byte[] DeriveKey(
        string input,
        byte[] salt,
        KeyDerivationPrf prf,
        int iterations,
        int outputLength)
    {
        var hashAlgorithm = prf switch
        {
            KeyDerivationPrf.Md5 => HashAlgorithmName.MD5,
            KeyDerivationPrf.Sha1 => HashAlgorithmName.SHA1,
            KeyDerivationPrf.Sha256 => HashAlgorithmName.SHA256,
            KeyDerivationPrf.Sha384 => HashAlgorithmName.SHA384,
            KeyDerivationPrf.Sha512 => HashAlgorithmName.SHA512,
            _ => throw new ArgumentOutOfRangeException(nameof(prf))
        };
        return Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, hashAlgorithm, outputLength);
    }

    #endregion
}