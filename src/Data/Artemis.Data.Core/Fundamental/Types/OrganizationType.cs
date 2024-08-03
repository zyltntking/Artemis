using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 机构类型
/// </summary>
[Description("机构类型")]
public sealed class OrganizationType : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private OrganizationType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 管理机构
    /// </summary>
    [Description("管理机构")]
    public static OrganizationType Management = new(1, nameof(Management));

    /// <summary>
    /// 职能机构
    /// </summary>
    [Description("职能机构")]
    public static OrganizationType Functional = new(2, nameof(Functional));
}