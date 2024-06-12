using System.Security.Cryptography;
using System.Text;
using Artemis.Data.Core.Fundamental.Kit.Crypto;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core;

/// <summary>
///     散列服务提供程序
/// </summary>
public static class Hash
{
    /// <summary>
    ///     默认键
    /// </summary>
    private static string DefaultKey { get; } = "Artemis";

    /// <summary>
    ///     密钥字节码
    /// </summary>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    private static byte[] KeyBytes(string? key = null)
    {
        key ??= DefaultKey;

        return Encoding.UTF8.GetBytes(key);
    }

    /// <summary>
    ///     委托，计算字符串散列
    /// </summary>
    /// <param name="func">哈希函数</param>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    private static string Compute(Func<byte[], byte[]> func, string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);

        var bytes = func(inputBytes);

        var builder = new StringBuilder();

        foreach (var b in bytes)
            builder.Append(b.ToString("x2"));

        return builder.ToString().StringNormalize();
    }

    /// <summary>
    ///     Artemis哈希
    /// </summary>
    /// <param name="input">输入原文</param>
    /// <returns></returns>
    /// <remarks>用于计算密码</remarks>
    public static string Password(string input)
    {
        var hash = ArtemisHasher.Create();

        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     哈希校验
    /// </summary>
    /// <param name="hashedText">密文</param>
    /// <param name="providedText">原文</param>
    /// <returns></returns>
    public static bool PasswordVerify(string hashedText, string providedText)
    {
        var hasher = ArtemisHasher.Create();

        return hasher.VerifyHash(hashedText, providedText);
    }

    /// <summary>
    ///     计算哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="hashType">哈希类型</param>
    /// <returns></returns>
    public static string HashData(string input, HashType hashType)
    {
        if (hashType.Equals(HashType.Unknown)) hashType = HashType.Md5;

        return Compute(HashTable[hashType], input);
    }

    /// <summary>
    ///     计算哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="hashType">哈希类型</param>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string HashData(string input, HmacHashType hashType, string? key = null)
    {
        if (hashType.Equals(HmacHashType.Unknown)) hashType = HmacHashType.HmacMd5;

        return Compute(bytes => HmacHashTable[hashType](bytes, KeyBytes(key)), input);
    }

    #region HashTable

    /// <summary>
    ///     hash委托缓存
    /// </summary>
    private static Dictionary<HashType, Func<byte[], byte[]>>? _hashTable;

    /// <summary>
    ///     Hash委托
    /// </summary>
    private static Dictionary<HashType, Func<byte[], byte[]>> HashTable =>
        _hashTable ??= new Dictionary<HashType, Func<byte[], byte[]>>
        {
            { HashType.Md5, Md5Hash },
            { HashType.Sha1, Sha1Hash },
            { HashType.Sha256, Sha256Hash },
            { HashType.Sha384, Sha384Hash },
            { HashType.Sha512, Sha512Hash },
            { HashType.Sha3_256, Sha3_256Hash },
            { HashType.Sha3_384, Sha3_384Hash },
            { HashType.Sha3_512, Sha3_512Hash }
        };

    /// <summary>
    ///     Md5哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Md5Hash(byte[] input)
    {
        return MD5.HashData(input);
    }

    /// <summary>
    ///     Sha1哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha1Hash(byte[] input)
    {
        return SHA1.HashData(input);
    }

    /// <summary>
    ///     Sha256哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha256Hash(byte[] input)
    {
        return SHA256.HashData(input);
    }

    /// <summary>
    ///     Sha384哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha384Hash(byte[] input)
    {
        return SHA384.HashData(input);
    }

    /// <summary>
    ///     Sha512哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha512Hash(byte[] input)
    {
        return SHA512.HashData(input);
    }

    /// <summary>
    ///     Sha3_256哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha3_256Hash(byte[] input)
    {
        return SHA3_256.HashData(input);
    }

    /// <summary>
    ///     Sha3_384哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha3_384Hash(byte[] input)
    {
        return SHA3_384.HashData(input);
    }

    /// <summary>
    ///     Sha3_512哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha3_512Hash(byte[] input)
    {
        return SHA3_512.HashData(input);
    }

    #endregion HashTable

    #region HMACHashTable

    /// <summary>
    ///     HMAC Hash委托缓存
    /// </summary>
    private static Dictionary<HmacHashType, Func<byte[], byte[], byte[]>>? _hmacHashTable;

    /// <summary>
    ///     HMAC Hash委托
    /// </summary>
    private static Dictionary<HmacHashType, Func<byte[], byte[], byte[]>> HmacHashTable =>
        _hmacHashTable ??= new Dictionary<HmacHashType, Func<byte[], byte[], byte[]>>
        {
            { HmacHashType.HmacMd5, HmacMd5Hash },
            { HmacHashType.HmacSha1, HmacSha1Hash },
            { HmacHashType.HmacSha256, HmacSha256Hash },
            { HmacHashType.HmacSha384, HmacSha384Hash },
            { HmacHashType.HmacSha512, HmacSha512Hash },
            { HmacHashType.HmacSha3_256, HmacSha3_256Hash },
            { HmacHashType.HmacSha3_384, HmacSha3_384Hash },
            { HmacHashType.HmacSha3_512, HmacSha3_512Hash }
        };

    /// <summary>
    ///     HMACMd5哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>以自身作为密钥对自身进行散列</remarks>
    private static byte[] HmacMd5Hash(byte[] input, byte[] key)
    {
        return HMACMD5.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha1Hash(byte[] input, byte[] key)
    {
        return HMACSHA1.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha256Hash(byte[] input, byte[] key)
    {
        return HMACSHA256.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha384Hash(byte[] input, byte[] key)
    {
        return HMACSHA384.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha512Hash(byte[] input, byte[] key)
    {
        return HMACSHA512.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha3_256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha3_256Hash(byte[] input, byte[] key)
    {
        return HMACSHA3_256.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha3_384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha3_384Hash(byte[] input, byte[] key)
    {
        return HMACSHA3_384.HashData(key, input);
    }

    /// <summary>
    ///     HMACSha3_512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static byte[] HmacSha3_512Hash(byte[] input, byte[] key)
    {
        return HMACSHA3_512.HashData(key, input);
    }

    #endregion HMACHashTable
}