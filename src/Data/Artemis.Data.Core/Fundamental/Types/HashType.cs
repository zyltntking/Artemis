using System.ComponentModel;

// ReSharper disable InconsistentNaming

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     Hash类型
/// </summary>
[Description("Hash类型")]
public sealed class HashType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("Unknown")] public static HashType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     Md5
    /// </summary>
    [Description("Md5")] public static HashType Md5 = new(0, nameof(Md5));

    /// <summary>
    ///     Sha1
    /// </summary>
    [Description("Sha1")] public static HashType Sha1 = new(1, nameof(Sha1));

    /// <summary>
    ///     Sha256
    /// </summary>
    [Description("Sha256")] public static HashType Sha256 = new(2, nameof(Sha256));

    /// <summary>
    ///     Sha384
    /// </summary>
    [Description("Sha384")] public static HashType Sha384 = new(3, nameof(Sha384));

    /// <summary>
    ///     Sha512
    /// </summary>
    [Description("Sha512")] public static HashType Sha512 = new(4, nameof(Sha512));

    /// <summary>
    ///     Sha3_256
    /// </summary>
    [Description("Sha3_256")] public static HashType Sha3_256 = new(5, nameof(Sha3_256));

    /// <summary>
    ///     Sha3_384
    /// </summary>
    [Description("Sha3_384")] public static HashType Sha3_384 = new(6, nameof(Sha3_384));

    /// <summary>
    ///     Sha3_512
    /// </summary>
    [Description("Sha3_512")] public static HashType Sha3_512 = new(7, nameof(Sha3_512));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private HashType(int id, string name) : base(id, name)
    {
    }
}

/// <summary>
///     HMAC哈希类型
/// </summary>
[Description("HMACHash类型")]
public sealed class HmacHashType : Enumeration
{
    /// <summary>
    ///     未知
    /// </summary>
    [Description("Unknown")] public static HmacHashType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     HMACMd5
    /// </summary>
    [Description("HMACMd5")] public static HmacHashType HmacMd5 = new(0, nameof(HmacMd5));

    /// <summary>
    ///     HMACSha1
    /// </summary>
    [Description("HMACSha1")] public static HmacHashType HmacSha1 = new(1, nameof(HmacSha1));

    /// <summary>
    ///     HMACSha256
    /// </summary>
    [Description("HMACSha256")] public static HmacHashType HmacSha256 = new(2, nameof(HmacSha256));

    /// <summary>
    ///     HMACSha384
    /// </summary>
    [Description("HMACSha384")] public static HmacHashType HmacSha384 = new(3, nameof(HmacSha384));

    /// <summary>
    ///     HMACSha512
    /// </summary>
    [Description("HMACSha512")] public static HmacHashType HmacSha512 = new(4, nameof(HmacSha512));

    /// <summary>
    ///     HMACSha3_256
    /// </summary>
    [Description("HMACSha3_256")] public static HmacHashType HmacSha3_256 = new(5, nameof(HmacSha3_256));

    /// <summary>
    ///     HMACSha3_384
    /// </summary>
    [Description("HMACSha3_384")] public static HmacHashType HmacSha3_384 = new(6, nameof(HmacSha3_384));

    /// <summary>
    ///     HMACSha3_512
    /// </summary>
    [Description("HMACSha3_512")] public static HmacHashType HmacSha3_512 = new(7, nameof(HmacSha3_512));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private HmacHashType(int id, string name) : base(id, name)
    {
    }
}