using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource;

/// <summary>
///     组织机构接口
/// </summary>
public interface IOrganization : IOrganizationInfo;

/// <summary>
///     组织机构信息接口
/// </summary>
public interface IOrganizationInfo : IOrganizationPackage, IKeySlot, IParentKeySlot;

/// <summary>
///     组织机构数据包接口
/// </summary>
public interface IOrganizationPackage
{
    /// <summary>
    ///     机构名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     机构编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    ///     机构设计编码
    /// </summary>
    string DesignCode { get; set; }

    /// <summary>
    ///     机构类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     机构成立时间
    /// </summary>
    DateTime? EstablishTime { get; set; }

    /// <summary>
    ///     机构邮箱
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     机构网站
    /// </summary>
    string? WebSite { get; set; }

    /// <summary>
    ///     机构联系电话
    /// </summary>
    string? ContactNumber { get; set; }

    /// <summary>
    ///     机构邮编
    /// </summary>
    string? PostCode { get; set; }

    /// <summary>
    ///     机构状态
    /// </summary>
    string Status { get; set; }

    /// <summary>
    ///     机构所在地行政区划代码
    /// </summary>
    string? DivisionCode { get; set; }

    /// <summary>
    ///     机构地址
    /// </summary>
    string? Address { get; set; }

    /// <summary>
    ///     机构描述
    /// </summary>
    string? Description { get; set; }
}