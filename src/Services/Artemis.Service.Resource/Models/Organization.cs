using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     组织机构
/// </summary>
public class Organization : ConcurrencyModel, IOrganization
{
    /// <summary>
    ///     机构名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("机构名称")]
    public required string Name { get; set; }

    /// <summary>
    ///    机构编码
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("机构编码")]
    public required string Code { get; set; }

    /// <summary>
    ///     机构类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("机构类型")]
    public required string Type { get; set; }

    /// <summary>
    ///    机构成立时间
    /// </summary>
    [Comment("机构成立时间")]
    public DateTime? EstablishTime { get; set; }

    /// <summary>
    ///     机构邮箱
    /// </summary>
    [MaxLength(128)]
    [Comment("机构邮箱")]
    public string? Email { get; set; }

    /// <summary>
    /// 机构网站
    /// </summary>
    [MaxLength(128)]
    [Comment("机构网站")]
    public string? WebSite { get; set; }

    /// <summary>
    /// 机构联系电话
    /// </summary>
    [MaxLength(32)]
    [Comment("机构联系电话")]
    public string? ContactNumber { get; set; }

    /// <summary>
    ///     机构邮编
    /// </summary>
    [MaxLength(32)]
    [Comment("机构邮编")]
    public string? PostCode { get; set; }

    /// <summary>
    ///     机构状态
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Comment("机构状态")]
    public required string Status { get; set; }

    /// <summary>
    ///    机构所在地行政区划代码
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     机构地址
    /// </summary>
    [MaxLength(256)]
    [Comment("机构地址")]
    public string? Address { get; set; }

    /// <summary>
    ///     机构描述
    /// </summary>
    [MaxLength(512)]
    [Comment("机构描述")]
    public string? Description { get; set; }

    /// <summary>
    ///     父级机构标识
    /// </summary>
    [Comment("父级机构标识")]
    public Guid ParentId { get; set; } = Guid.Empty;
}