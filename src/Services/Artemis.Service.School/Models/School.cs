using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学校模型
/// </summary>
public class School : ConcurrencyModel, ISchool
{
    /// <summary>
    ///     学校名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学校名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    [MaxLength(128)]
    [Comment("学校编码")]
    public string? Code { get; set; }

    /// <summary>
    ///     绑定标记
    /// </summary>
    [MaxLength(64)]
    [Comment("绑定标记")]
    public string? BindingTag { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    [MaxLength(128)]
    [Comment("学校类型")]
    public string? Type { get; set; }

    /// <summary>
    ///     组织机构代码
    /// </summary>
    [MaxLength(128)]
    [Comment("组织机构代码")]
    public string? OrganizationCode { get; set; }

    /// <summary>
    ///     学校所在地行政区划代码
    /// </summary>
    [MaxLength(32)]
    [Comment("学校所在地行政区划代码")]
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     学校地址
    /// </summary>
    [MaxLength(128)]
    [Comment("学校地址")]
    public string? Address { get; set; }

    /// <summary>
    ///     学校邮箱
    /// </summary>
    [MaxLength(128)]
    [Comment("学校邮箱")]
    public string? Email { get; set; }

    /// <summary>
    ///     学校网站
    /// </summary>
    [MaxLength(128)]
    [Comment("学校网站")]
    public string? WebSite { get; set; }

    /// <summary>
    ///     学校联系电话
    /// </summary>
    [MaxLength(32)]
    [Comment("学校联系电话")]
    public string? ContactNumber { get; set; }

    /// <summary>
    ///     学校建立时间
    /// </summary>
    [Comment("学校建立时间")]
    public DateTime? EstablishTime { get; set; }

    /// <summary>
    ///     学校简介
    /// </summary>
    [MaxLength(512)]
    [Comment("学校简介")]
    public string? Introduction { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    [MaxLength(128)]
    [Comment("备注")]
    public string? Remark { get; set; }
}