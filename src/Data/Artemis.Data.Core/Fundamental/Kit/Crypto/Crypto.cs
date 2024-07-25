using System.Security.Cryptography;
using System.Text;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core.Fundamental.Kit.Crypto;

/// <summary>
///     对称算法
/// </summary>
public static class Crypto
{
    #region Symmetric

    /// <summary>
    ///     对称加密串
    /// </summary>
    /// <param name="input">输入段</param>
    /// <param name="key">密钥</param>
    /// <param name="symmetricType">对称算法类型</param>
    /// <returns></returns>
    public static string Encrypt(string input, string key, SymmetricType symmetricType)
    {
        var (k, iv) = GenerateSymmetricKey(key, symmetricType);

        try
        {
            var inputBytes = Encoding.Unicode.GetBytes(input);

            var encryptBytes = Encrypt(inputBytes, k, iv, symmetricType);

            return Convert.ToBase64String(encryptBytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///     对称解密串
    /// </summary>
    /// <param name="input">输入段</param>
    /// <param name="key">密钥</param>
    /// <param name="symmetricType">对称算法类型</param>
    /// <returns></returns>
    public static string Decrypt(string input, string key, SymmetricType symmetricType)
    {
        var (k, iv) = GenerateSymmetricKey(key, symmetricType);

        try
        {
            var inputBytes = Convert.FromBase64String(input);

            var decryptBytes = Decrypt(inputBytes, k, iv, symmetricType);

            return Encoding.Unicode.GetString(decryptBytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///     对称加密
    /// </summary>
    /// <param name="input">输入段</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">向量</param>
    /// <param name="symmetricType">对称算法类型</param>
    /// <returns></returns>
    public static byte[] Encrypt(byte[] input, byte[] key, byte[] iv, SymmetricType symmetricType)
    {
        using var encryptor = EncryptorTable[symmetricType](key, iv);

        return encryptor.TransformFinalBlock(input, 0, input.Length);
    }

    /// <summary>
    ///     对称解密
    /// </summary>
    /// <param name="input">输入段</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">向量</param>
    /// <param name="symmetricType">对称算法类型</param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] input, byte[] key, byte[] iv, SymmetricType symmetricType)
    {
        using var decryptor = DecryptorTable[symmetricType](key, iv);

        return decryptor.TransformFinalBlock(input, 0, input.Length);
    }

    #region EncryptorTable

    /// <summary>
    ///     加密器表
    /// </summary>
    private static Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>>? _encryptorTable;

    /// <summary>
    ///     加密器表
    /// </summary>
    private static Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>> EncryptorTable =>
        _encryptorTable ??= new Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>>
        {
            { SymmetricType.Des, (key, iv) => DES.Create().CreateEncryptor(key, iv) },
            { SymmetricType.Rc2, (key, iv) => RC2.Create().CreateEncryptor(key, iv) },
            { SymmetricType.TripleDes, (key, iv) => TripleDES.Create().CreateEncryptor(key, iv) },
            { SymmetricType.Aes, (key, iv) => Aes.Create().CreateEncryptor(key, iv) }
        };

    #endregion

    #region DecryptorTable

    /// <summary>
    ///     解密器表
    /// </summary>
    private static Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>>? _decryptorTable;

    /// <summary>
    ///     解密器表
    /// </summary>
    private static Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>> DecryptorTable =>
        _decryptorTable ??= new Dictionary<SymmetricType, Func<byte[], byte[]?, ICryptoTransform>>
        {
            { SymmetricType.Des, (key, iv) => DES.Create().CreateDecryptor(key, iv) },
            { SymmetricType.Rc2, (key, iv) => RC2.Create().CreateDecryptor(key, iv) },
            { SymmetricType.TripleDes, (key, iv) => TripleDES.Create().CreateDecryptor(key, iv) },
            { SymmetricType.Aes, (key, iv) => Aes.Create().CreateDecryptor(key, iv) }
        };

    #endregion

    #region SymmetricKey

    /// <summary>
    ///     生成堆成密钥和向量对
    /// </summary>
    /// <param name="input">输入字符密钥</param>
    /// <param name="symmetricType">对称算法类型</param>
    /// <returns></returns>
    private static (byte[] kye, byte[] iv) GenerateSymmetricKey(string input, SymmetricType symmetricType)
    {
        var hash = Hash.HashData(input, HashType.Sha512);

        var hashBytes = Encoding.ASCII.GetBytes(hash);

        var hashLength = hashBytes.Length;

        var keyLength = KeyLengthTable[symmetricType].KeyLength;

        var ivLength = KeyLengthTable[symmetricType].IvLength;

        var key = new byte[keyLength];

        var keyBlock = keyLength + symmetricType;

        var iv = new byte[ivLength];

        var ivBlock = ivLength + symmetricType;

        for (var i = 0; i < keyLength; i++)
        {
            var keyOffset = keyBlock * i % hashLength;

            key[i] = hashBytes[keyOffset];
        }

        for (var i = 0; i < ivLength; i++)
        {
            var ivOffset = ivBlock * i % hashLength;

            iv[i] = hashBytes[ivOffset];
        }

        return (key, iv);
    }

    /// <summary>
    ///     密钥长度表
    /// </summary>
    private static Dictionary<SymmetricType, (int KeyLength, int IvLength)>? _keyLengthTable;

    /// <summary>
    ///     密钥长度表
    /// </summary>
    private static Dictionary<SymmetricType, (int KeyLength, int IvLength)> KeyLengthTable =>
        _keyLengthTable ??= new Dictionary<SymmetricType, (int KeyLength, int IvLength)>
        {
            { SymmetricType.Des, new(8, 8) },
            { SymmetricType.Rc2, new(16, 8) },
            { SymmetricType.TripleDes, new(24, 8) },
            { SymmetricType.Aes, new(32, 16) }
        };

    #endregion SymmetricKey

    #endregion Symmetric
}