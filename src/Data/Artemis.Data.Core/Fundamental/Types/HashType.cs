namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///    Hash类型
/// </summary>
public class HashType : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private HashType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 未知类型
    /// </summary>
    public static HashType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///   Md5
    /// </summary>
    public static HashType Md5 = new(0, nameof(Md5));

    /// <summary>
    ///   Sha1
    /// </summary>
    public static HashType Sha1 = new(1, nameof(Sha1));

    /// <summary>
    ///  Sha256
    /// </summary>
    public static HashType Sha256 = new(2, nameof(Sha256));

    /// <summary>
    ///  Sha384
    /// </summary>
    public static HashType Sha384 = new(3, nameof(Sha384));

    /// <summary>
    ///  Sha512
    /// </summary>
    public static HashType Sha512 = new(4, nameof(Sha512));
}