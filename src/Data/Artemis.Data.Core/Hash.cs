using Artemis.Data.Core.Fundamental.Kit;
using System.Security.Cryptography;
using System.Text;

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

        return builder.ToString();
    }

    /// <summary>
    /// Artemis哈希
    /// </summary>
    /// <param name="input">输入原文</param>
    /// <returns></returns>
    /// <remarks>用于计算密码</remarks>
    public static string ArtemisHash(string input)
    {
        var hash = ArtemisHasher.Create();

        return hash.ComputeHash(input);
    }

    /// <summary>
    /// 哈希校验
    /// </summary>
    /// <param name="hashedText">密文</param>
    /// <param name="providedText">原文</param>
    /// <returns></returns>
    public static bool ArtemisHashVerify(string hashedText, string providedText)
    {
        var hasher = ArtemisHasher.Create();

        var (success, _) = hasher.VerifyHash(hashedText, providedText);

        return success;
    }

    /// <summary>
    ///     Md5哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>输出字符串</returns>
    public static string Md5Hash(string input)
    {
        return Compute(Md5Hash, input);
    }

    /// <summary>
    ///     Md5哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public static byte[] Md5Hash(byte[] input)
    {
        using var hash = MD5.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static string Sha1Hash(string input)
    {
        return Compute(Sha1Hash, input);
    }

    /// <summary>
    ///     Sha1哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha1Hash(byte[] input)
    {
        using var hash = SHA1.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha256哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static string Sha256Hash(string input)
    {
        return Compute(Sha256Hash, input);
    }

    /// <summary>
    ///     Sha256哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha256Hash(byte[] input)
    {
        using var hash = SHA256.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha384哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static string Sha384Hash(string input)
    {
        return Compute(Sha384Hash, input);
    }

    /// <summary>
    ///     Sha384哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha384Hash(byte[] input)
    {
        using var hash = SHA384.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha512哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public static string Sha512Hash(string input)
    {
        return Compute(Sha512Hash, input);
    }

    /// <summary>
    ///     Sha512哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    private static byte[] Sha512Hash(byte[] input)
    {
        using var hash = SHA512.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     HMACMd5哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>以自身作为密钥对自身进行散列</remarks>
    public static string HmacMd5Hash(string input, string? key = null)
    {
        using var hash = new HMACMD5(KeyBytes(key));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACMd5哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>以自身作为密钥对自身进行散列</remarks>
    public static byte[] HmacMd5Hash(byte[] input, byte[] key)
    {
        using var hash = new HMACMD5(key);
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     HMACSha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string HmacSha1Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA1(KeyBytes(key));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] HmacSha1Hash(byte[] input, byte[] key)
    {
        using var hash = new HMACSHA1(key);
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     HMACSha256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string HmacSha256Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA256(KeyBytes(key));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] HmacSha256Hash(byte[] input, byte[] key)
    {
        using var hash = new HMACSHA256(key);
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     HMACSha384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string HmacSha384Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA384(KeyBytes(key));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] HmacSha384Hash(byte[] input, byte[] key)
    {
        using var hash = new HMACSHA384(key);
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     HMACSha512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string HmacSha512Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA512(KeyBytes(key));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] HmacSha512Hash(byte[] input, byte[] key)
    {
        using var hash = new HMACSHA512(key);
        return hash.ComputeHash(input);
    }
}