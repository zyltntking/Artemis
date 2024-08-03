using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 机构状态
/// </summary>
[Description("机构状态")]
public sealed class OrganizationStatus : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private OrganizationStatus(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 运营中
    /// </summary>
    [Description("运营中")]
    public static OrganizationStatus InOperation = new (1, nameof(InOperation));

    /// <summary>
    /// 停止运营
    /// </summary>
    [Description("停止运营")]
    public static OrganizationStatus CeaseOperation = new (2, nameof(CeaseOperation));
}