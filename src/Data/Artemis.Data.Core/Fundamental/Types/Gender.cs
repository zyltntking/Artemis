using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     性别类型
/// </summary>
[Description("性别类型")]
public class Gender : Enumeration
{
    /// <summary>
    ///     未知
    /// </summary>
    [Description("未知性别")] public static Gender Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     男性
    /// </summary>
    [Description("男性")] public static Gender Male = new(0, nameof(Male));

    /// <summary>
    ///     女性
    /// </summary>
    [Description("女性")] public static Gender Female = new(1, nameof(Female));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private Gender(int id, string name) : base(id, name)
    {
    }
}