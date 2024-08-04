using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 标准类型
/// </summary>
[Description("标准类型")]
public sealed class StandardType : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private StandardType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 视力标准
    /// </summary>
    [Description("视力标准")]
    public static StandardType VisualStandard = new(1, nameof(VisualStandard));

}