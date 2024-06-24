using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     过滤操作类型
/// </summary>
[Description("操作类型")]
internal class OperationType : Enumeration
{
    /// <summary>
    ///     等于
    /// </summary>
    [Description("等于")] public static OperationType Equal = new(0, nameof(Equal));

    /// <summary>
    ///     不等于
    /// </summary>
    [Description("不等于")] public static OperationType NotEqual = new(1, nameof(NotEqual));

    /// <summary>
    ///     大于
    /// </summary>
    [Description("大于")] public static OperationType GreaterThan = new(2, nameof(GreaterThan));

    /// <summary>
    ///     大于等于
    /// </summary>
    [Description("大于等于")] public static OperationType GreaterThanOrEqual = new(3, nameof(GreaterThanOrEqual));

    /// <summary>
    ///     小于
    /// </summary>
    [Description("小于")] public static OperationType LessThan = new(4, nameof(LessThan));

    /// <summary>
    ///     小于等于
    /// </summary>
    [Description("小于等于")] public static OperationType LessThanOrEqual = new(5, nameof(LessThanOrEqual));

    /// <summary>
    ///     包含
    /// </summary>
    [Description("包含")] public static OperationType Contains = new(6, nameof(Contains));

    /// <summary>
    ///     不包含
    /// </summary>
    [Description("不包含")] public static OperationType NotContains = new(7, nameof(NotContains));

    /// <summary>
    ///     以...开始
    /// </summary>
    [Description("以...开始")] public static OperationType StartsWith = new(8, nameof(StartsWith));

    /// <summary>
    ///     不以...开始
    /// </summary>
    [Description("不以...开始")] public static OperationType NotStartsWith = new(9, nameof(NotStartsWith));

    /// <summary>
    ///     以...结束
    /// </summary>
    [Description("以...结束")] public static OperationType EndsWith = new(10, nameof(EndsWith));

    /// <summary>
    ///     不以...结束
    /// </summary>
    [Description("不以...结束")] public static OperationType NotEndsWith = new(11, nameof(NotEndsWith));

    /// <summary>
    ///     在...之间
    /// </summary>
    [Description("在...之间")] public static OperationType In = new(12, nameof(In));

    /// <summary>
    ///     不在...之间
    /// </summary>
    [Description("不在...之间")] public static OperationType NotIn = new(13, nameof(NotIn));

    /// <summary>
    ///     为空
    /// </summary>
    [Description("为空")] public static OperationType IsNull = new(14, nameof(IsNull));

    /// <summary>
    ///     不为空
    /// </summary>
    [Description("不为空")] public static OperationType IsNotNull = new(15, nameof(IsNotNull));

    /// <summary>
    ///     在...之间
    /// </summary>
    [Description("在...之间")] public static OperationType Between = new(16, nameof(Between));

    /// <summary>
    ///     不在...之间
    /// </summary>
    [Description("不在...之间")] public static OperationType NotBetween = new(17, nameof(NotBetween));

    /// <summary>
    ///     包含
    /// </summary>
    [Description("包含")] public static OperationType Like = new(18, nameof(Like));

    /// <summary>
    ///     不包含
    /// </summary>
    [Description("不包含")] public static OperationType NotLike = new(19, nameof(NotLike));

    /// <summary>
    ///     为空
    /// </summary>
    [Description("为空")] public static OperationType IsEmpty = new(20, nameof(IsEmpty));

    /// <summary>
    ///     不为空
    /// </summary>
    [Description("不为空")] public static OperationType IsNotEmpty = new(21, nameof(IsNotEmpty));

    /// <summary>
    ///     为真
    /// </summary>
    [Description("为真")] public static OperationType IsTrue = new(22, nameof(IsTrue));

    /// <summary>
    ///     为假
    /// </summary>
    [Description("为假")] public static OperationType IsFalse = new(23, nameof(IsFalse));

    /// <summary>
    ///     为空或空白
    /// </summary>
    [Description("为空或空白")] public static OperationType IsNullOrWhiteSpace = new(24, nameof(IsNullOrWhiteSpace));

    /// <summary>
    ///     不为空或空白
    /// </summary>
    [Description("不为空或空白")] public static OperationType IsNotNullOrWhiteSpace = new(25, nameof(IsNotNullOrWhiteSpace));

    /// <summary>
    ///     为空或Null
    /// </summary>
    [Description("为空或Null")] public static OperationType IsNullOrEmpty = new(26, nameof(IsNullOrEmpty));

    /// <summary>
    ///     不为空或Null
    /// </summary>
    [Description("不为空或Null")] public static OperationType IsNotNullOrEmpty = new(27, nameof(IsNotNullOrEmpty));

    /// <summary>
    ///     为空或默认值
    /// </summary>
    [Description("为空或默认值")] public static OperationType IsNullOrDefault = new(28, nameof(IsNullOrDefault));

    /// <summary>
    ///     不为空或默认值
    /// </summary>
    [Description("不为空或默认值")] public static OperationType IsNotNullOrDefault = new(29, nameof(IsNotNullOrDefault));

    /// <summary>
    ///     为空或0
    /// </summary>
    [Description("为空或0")] public static OperationType IsNullOrZero = new(30, nameof(IsNullOrZero));

    /// <summary>
    ///     不为空或0
    /// </summary>
    [Description("不为空或0")] public static OperationType IsNotNullOrZero = new(31, nameof(IsNotNullOrZero));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private OperationType(int id, string name) : base(id, name)
    {
    }
}