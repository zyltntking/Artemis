using System.Security.Cryptography;
using System.Text;

namespace Artemis.Data.Core;

/// <summary>
///     散列服务提供程序
/// </summary>
public class HashProvider
{
    /// <summary>
    ///     密钥字节码
    /// </summary>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    private byte[] KeyBytes(string key)
    {
        return Encoding.UTF8.GetBytes(key);
    }

    /// <summary>
    ///     委托，计算字符串散列
    /// </summary>
    /// <param name="func">散列函数</param>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    private string Compute(Func<byte[], byte[]> func, string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);

        var data = func(inputBytes);

        var builder = new StringBuilder();

        foreach (var b in data)
            builder.Append(b.ToString("x2"));

        return builder.ToString();
    }

    /// <summary>
    ///     Md5哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>输出字符串</returns>
    public string Md5Hash(string input)
    {
        using var hash = MD5.Create();
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     Md5哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public byte[] Md5Hash(byte[] input)
    {
        using var hash = MD5.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public string Sha1Hash(string input)
    {
        using var hash = SHA1.Create();
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     Sha1哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public byte[] Sha1Hash(byte[] input)
    {
        using var hash = SHA1.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha256哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public string Sha256Hash(string input)
    {
        using var hash = SHA256.Create();
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     Sha256哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public byte[] Sha256Hash(byte[] input)
    {
        using var hash = SHA256.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha384哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public string Sha384Hash(string input)
    {
        using var hash = SHA384.Create();
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     Sha384哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public byte[] Sha384Hash(byte[] input)
    {
        using var hash = SHA384.Create();
        return hash.ComputeHash(input);
    }

    /// <summary>
    ///     Sha512哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns></returns>
    public string Sha512Hash(string input)
    {
        using var hash = SHA512.Create();
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     Sha512哈希
    /// </summary>
    /// <param name="input">输入字节码</param>
    /// <returns>输出字节码</returns>
    public byte[] Sha512Hash(byte[] input)
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
    public string HmacMd5Hash(string input, string? key = null)
    {
        using var hash = new HMACMD5(KeyBytes(key ?? input));
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
    public string HmacMd5Hash(string input, byte[] key)
    {
        using var hash = new HMACMD5(key);
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha1Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA1(KeyBytes(key ?? input));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha1哈希
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha1Hash(string input, byte[] key)
    {
        using var hash = new HMACSHA1(key);
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha256Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA256(KeyBytes(key ?? input));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha256哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha256Hash(string input, byte[] key)
    {
        using var hash = new HMACSHA256(key);
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha384Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA384(KeyBytes(key ?? input));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha384哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha384Hash(string input, byte[] key)
    {
        using var hash = new HMACSHA384(key);
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha512Hash(string input, string? key = null)
    {
        using var hash = new HMACSHA512(KeyBytes(key ?? input));
        var result = Compute(hash.ComputeHash, input);

        return result;
    }

    /// <summary>
    ///     HMACSha512哈希
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string HmacSha512Hash(string input, byte[] key)
    {
        using var hash = new HMACSHA512(key);
        var result = Compute(hash.ComputeHash, input);

        return result;
    }
}