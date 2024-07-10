using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     对称算法类型
/// </summary>
[Description("对称算法类型")]
public sealed class SymmetricType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static SymmetricType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     DES
    /// </summary>
    [Description("DES")] public static SymmetricType Des = new(1, nameof(Des));

    /// <summary>
    ///     RC2
    /// </summary>
    [Description("RC2")] public static SymmetricType Rc2 = new(3, nameof(Rc2));

    /// <summary>
    ///     TripleDES
    /// </summary>
    [Description("TripleDES")] public static SymmetricType TripleDes = new(5, nameof(TripleDes));

    /// <summary>
    ///     AES
    /// </summary>
    [Description("AES")] public static SymmetricType Aes = new(7, nameof(Aes));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private SymmetricType(int id, string name) : base(id, name)
    {
    }
}