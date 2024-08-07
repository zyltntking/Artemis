using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 亲属关系
/// </summary>
[Description("亲属关系")]
public sealed class DomesticRelation : Enumeration
{
    /// <summary>
    /// 父亲
    /// </summary>
    [Description("父亲")]
    public static readonly DomesticRelation Father = new(1, nameof(Father));

    /// <summary>
    /// 母亲
    /// </summary>
    [Description("母亲")]
    public static readonly DomesticRelation Mother = new(2, nameof(Mother));

    /// <summary>
    /// 爷爷
    /// </summary>
    [Description("爷爷")]
    public static readonly DomesticRelation Grandfather = new(3, nameof(Grandfather));

    /// <summary>
    /// 奶奶
    /// </summary>
    [Description("奶奶")]
    public static readonly DomesticRelation Grandmother = new(4, nameof(Grandmother));

    /// <summary>
    /// 外公
    /// </summary>
    [Description("外公")]
    public static readonly DomesticRelation MaternalGrandfather = new(5, nameof(MaternalGrandfather));

    /// <summary>
    /// 外婆
    /// </summary>
    [Description("外婆")]
    public static readonly DomesticRelation MaternalGrandmother = new(6, nameof(MaternalGrandmother));

    /// <summary>
    /// 哥哥
    /// </summary>
    [Description("哥哥")]
    public static readonly DomesticRelation ElderBrother = new(7, nameof(ElderBrother));

    /// <summary>
    /// 姐姐
    /// </summary>
    [Description("姐姐")]
    public static readonly DomesticRelation ElderSister = new(8, nameof(ElderSister));


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private DomesticRelation(int id, string name) : base(id, name)
    {
    }
}