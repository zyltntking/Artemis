using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学校模型
/// </summary>
public class School : ConcurrencyPartition, ISchool
{
    #region Implementation of ISchool

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
    [Required]
    [MaxLength(128)]
    [Comment("学校编码")]
    public required string Code { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学校类型")]
    public required string Type { get; set; }

    /// <summary>
    /// 组织机构代码
    /// </summary>
    public string? OrganizationCode { get; set; }

    /// <summary>
    /// 学校所在地行政区划代码
    /// </summary>
    public string? DivisionCode { get; set; }

    /// <summary>
    ///    学校地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 学校邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 学校网站
    /// </summary>
    public string? WebSite { get; set; }

    /// <summary>
    /// 学校联系电话
    /// </summary>
    public string? ContactNumber { get; set; }

    /// <summary>
    /// 学校建立时间
    /// </summary>
    public DateTime? EstablishTime { get; set; }

    #endregion
}