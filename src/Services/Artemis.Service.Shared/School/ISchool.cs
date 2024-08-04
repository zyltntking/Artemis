using Artemis.Data.Core;

namespace Artemis.Service.Shared.School;

/// <summary>
/// 学校接口
/// </summary>
public interface ISchool : ISchoolInfo
{
}

/// <summary>
///    学校信息接口
/// </summary>
public interface ISchoolInfo : ISchoolPackage, IKeySlot
{

}

/// <summary>
///     学校数据包接口
/// </summary>
public interface ISchoolPackage
{
    /// <summary>
    ///     学校名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    /// 绑定标记
    /// </summary>
    string? BindingTag { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    string? Type { get; set; }

    /// <summary>
    ///     组织机构代码
    /// </summary>
    string? OrganizationCode { get; set; }

    /// <summary>
    ///     学校所在地行政区划代码
    /// </summary>
    string? DivisionCode { get; set; }

    /// <summary>
    ///     学校地址
    /// </summary>
    string? Address { get; set; }

    /// <summary>
    ///     学校邮箱
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     学校网站
    /// </summary>
    string? WebSite { get; set; }

    /// <summary>
    ///     学校联系电话
    /// </summary>
    string? ContactNumber { get; set; }

    /// <summary>
    ///     学校建立时间
    /// </summary>
    DateTime? EstablishTime { get; set; }

    /// <summary>
    /// 学校简介
    /// </summary>
    string? Introduction { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    string? Remark { get; set; }
}