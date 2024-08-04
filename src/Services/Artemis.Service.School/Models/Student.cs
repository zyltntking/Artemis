using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学生模型
/// </summary>
public class Student : ConcurrencyModel, IStudent
{
    /// <summary>
    ///     学校标识
    /// </summary>
    [Comment("学校标识")]
    public Guid? SchoolId { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    [Comment("班级标识")]
    public Guid? ClassId { get; set; }

    /// <summary>
    ///     学生名称
    /// </summary>
    [Required]
    [Comment("学生名称")]
    [MaxLength(32)]
    public required string Name { get; set; }

    /// <summary>
    ///     学生性别
    /// </summary>
    [Comment("学生性别")]
    [MaxLength(32)]
    public string? Gender { get; set; }

    /// <summary>
    ///     学生生日
    /// </summary>
    [Comment("学生生日")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     民族
    /// </summary>
    [Comment("民族")]
    [MaxLength(32)]
    public string? Nation { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    [Comment("学生编码")]
    [MaxLength(32)]
    public string? Code { get; set; }

    /// <summary>
    ///     学籍号
    /// </summary>
    [Comment("学籍号")]
    [MaxLength(32)]
    public required string StudentNumber { get; set; }

    /// <summary>
    ///     证件号码
    /// </summary>
    [Comment("证件号码")]
    [MaxLength(32)]
    public string? Cert { get; set; }

    /// <summary>
    ///     入学时间
    /// </summary>
    [Comment("入学时间")]
    public DateTime? EnrollmentDate { get; set; }

    /// <summary>
    ///     住址区划代码
    /// </summary>
    [Comment("住址区划代码")]
    [MaxLength(32)]
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     住址
    /// </summary>
    [Comment("住址")]
    [MaxLength(128)]
    public string? Address { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    [Comment("备注")]
    [MaxLength(128)]
    public string? Remark { get; set; }
}