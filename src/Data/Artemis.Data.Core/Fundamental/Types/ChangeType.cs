using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 变更记录类型
/// </summary>
[Description("变更记录类型")]
public sealed class ChangeType : Enumeration
{
    /// <summary>
    /// 转入
    /// </summary>
    [Description("转入学校")]
    public static ChangeType MoveInSchool { get; set; } = new ChangeType(1, nameof(MoveInSchool));

    /// <summary>
    /// 转出
    /// </summary>
    [Description("转出学校")]
    public static ChangeType MoveOutSchool { get; set; } = new ChangeType(2, nameof(MoveOutSchool));

    /// <summary>
    /// 转入
    /// </summary>
    [Description("转入班级")]
    public static ChangeType MoveInClass { get; set; } = new ChangeType(3, nameof(MoveInClass));

    /// <summary>
    /// 转出
    /// </summary>
    [Description("转出班级")]
    public static ChangeType MoveOutClass { get; set; } = new ChangeType(4, nameof(MoveOutClass));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ChangeType(int id, string name) : base(id, name)
    {
    }
}