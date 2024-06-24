using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     过滤操作类型
/// </summary>
public class FilterOperationType : Enumeration
{
    /// <summary>
    ///     等于
    /// </summary>
    [Description("等于")] public static FilterOperationType Equal = new(0, nameof(Equal));

    /// <summary>
    ///     不等于
    /// </summary>
    [Description("不等于")] public static FilterOperationType NotEqual = new(1, nameof(NotEqual));

    /// <summary>
    ///     大于
    /// </summary>
    [Description("大于")] public static FilterOperationType GreaterThan = new(2, nameof(GreaterThan));

    /// <summary>
    ///     大于等于
    /// </summary>
    [Description("大于等于")] public static FilterOperationType GreaterThanOrEqual = new(3, nameof(GreaterThanOrEqual));

    /// <summary>
    ///     小于
    /// </summary>
    [Description("小于")] public static FilterOperationType LessThan = new(4, nameof(LessThan));

    /// <summary>
    ///     小于等于
    /// </summary>
    [Description("小于等于")] public static FilterOperationType LessThanOrEqual = new(5, nameof(LessThanOrEqual));

    /// <summary>
    ///     包含
    /// </summary>
    [Description("包含")] public static FilterOperationType Like = new(18, nameof(Like));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private FilterOperationType(int id, string name) : base(id, name)
    {
    }
}