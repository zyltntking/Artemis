using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学生模型
/// </summary>
public class Student : ConcurrencyPartition, IStudent
{
    #region Implementation of IStudent

    /// <summary>
    ///     学生名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学生名称")]
    public required string Name { get; set; }

    /// <summary>
    ///    学生性别
    /// </summary>
    [Required]
    [MaxLength(8)]
    [Comment("学生性别")]
    public required string Gender { get; set; }

    /// <summary>
    /// 专业
    /// </summary>
    [MaxLength(128)]
    [Comment("专业")]
    public string? Major { get; set; }

    /// <summary>
    ///    学生生日
    /// </summary>
    [Required]
    [Comment("学生生日")]
    public required DateOnly Birthday { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    [MaxLength(32)]
    [Comment("民族")]
    public string? Nation { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学生编码")]
    public required string Code { get; set; }

    /// <summary>
    /// 学籍号
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学籍号")]
    public required string StudentNumber { get; set; }

    /// <summary>
    /// 证件号码
    /// </summary>
    [MaxLength(32)]
    [Comment("证件号码")]
    public string? Cert { get; set; }

    /// <summary>
    /// 入学时间
    /// </summary>
    [Required]
    [Comment("入学时间")]
    public required DateTime EnrollmentDate { get; set; }

    /// <summary>
    ///  住址区划代码
    /// </summary>
    [MaxLength(32)]
    [Comment("住址区划代码")]
    public string? DivisionCode { get; set; }

    /// <summary>
    /// 住址
    /// </summary>
    [MaxLength(256)]
    [Comment("住址")]
    public string? Address { get; set; }

    #endregion
}