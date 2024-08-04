using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 标准状态
/// </summary>
[Description("标准状态")]
public sealed class StandardState : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private StandardState(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 现行标准
    /// </summary>
    [Description("现行标准")]
    public static readonly StandardState Current = new(1, nameof(Current));

    /// <summary>
    /// 标准草案
    /// </summary>
    [Description("标准草案")]
    public static readonly StandardState Draft = new(2, nameof(Draft));

    /// <summary>
    /// 过期标准
    /// </summary>
    [Description("过期标准")]
    public static readonly StandardState Obsolete = new(3, nameof(Obsolete));
}