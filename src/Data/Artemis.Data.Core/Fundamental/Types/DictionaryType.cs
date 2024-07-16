using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     字典类型
/// </summary>
[Description("字典类型")]
public class DictionaryType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static readonly DictionaryType Unknown = new(0, nameof(Unknown));

    /// <summary>
    ///     内部字典
    /// </summary>
    [Description("内部字典")] public static readonly DictionaryType Internal = new(1, nameof(Internal));

    /// <summary>
    ///     外部字典
    /// </summary>
    [Description("外部字典")] public static readonly DictionaryType Public = new(2, nameof(Public));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private DictionaryType(int id, string name) : base(id, name)
    {
    }
}