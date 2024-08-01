using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 组织机构树
/// </summary>
public record OrganizationInfoTree : OrganizationInfo, ITreeInfoSlot<OrganizationInfoTree>
{
    /// <summary>
    ///     子节点
    /// </summary>
    public ICollection<OrganizationInfoTree>? Children { get; set; }
}

/// <summary>
/// 组织机构信息
/// </summary>
public record OrganizationInfo : OrganizationPackage, IOrganizationInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     父标识
    /// </summary>
    public Guid? ParentId { get; set; }
}

/// <summary>
/// 组织机构数据包
/// </summary>
public record OrganizationPackage : IOrganizationPackage
{
    /// <summary>
    ///     机构名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     机构编码
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    ///     机构类型
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    ///     机构成立时间
    /// </summary>
    public DateTime? EstablishTime { get; set; }

    /// <summary>
    ///     机构邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     机构网站
    /// </summary>
    public string? WebSite { get; set; }

    /// <summary>
    ///     机构联系电话
    /// </summary>
    public string? ContactNumber { get; set; }

    /// <summary>
    ///     机构邮编
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    ///     机构状态
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    ///     机构所在地行政区划代码
    /// </summary>
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     机构地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     机构描述
    /// </summary>
    public string? Description { get; set; }
}