using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     学段
/// </summary>
[Description("学段")]
public sealed class StudyPhase : Enumeration
{
    /// <summary>
    ///     学前阶段(幼儿园)
    /// </summary>
    [Description("学前阶段(幼儿园)")] public static readonly StudyPhase PreSchool = new(1, nameof(PreSchool));

    /// <summary>
    ///     小学阶段
    /// </summary>
    [Description("小学阶段")] public static readonly StudyPhase Primary = new(1, nameof(Primary));

    /// <summary>
    ///     初中阶段
    /// </summary>
    [Description("初中阶段")] public static readonly StudyPhase Junior = new(2, nameof(Junior));

    /// <summary>
    ///     高中阶段
    /// </summary>
    [Description("高中阶段")] public static readonly StudyPhase Senior = new(3, nameof(Senior));

    /// <summary>
    ///     大学阶段
    /// </summary>
    [Description("大学阶段")] public static readonly StudyPhase University = new(4, nameof(University));

    /// <summary>
    /// 职业高中
    /// </summary>
    [Description("职业高中")] public static readonly StudyPhase VocationalHigh = new(5, nameof(VocationalHigh));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private StudyPhase(int id, string name) : base(id, name)
    {
    }
}