using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     排序类型
/// </summary>
[Description("排序类型")]
public class OrderType : Enumeration
{
    /// <summary>
    ///     正序
    /// </summary>
    [Description("正序")] public static OrderType Asc = new(0, nameof(Asc));

    /// <summary>
    ///     倒序
    /// </summary>
    [Description("倒序")] public static OrderType Desc = new(1, nameof(Desc));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private OrderType(int id, string name) : base(id, name)
    {
    }
}