using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Service.School.Models;

/// <summary>
/// 学生变更记录模型
/// </summary>
public class StudentChangeLog : ConcurrencyModel, IStudentChangeLog
{
    #region Implementation of IChangeLogBase

    /// <summary>
    /// 变动类型
    /// </summary>
    [Comment("变动类型")]
    [MaxLength(32)]
    [Required]
    public required string ChangeType { get; set; }

    /// <summary>
    ///     变动时间
    /// </summary>
    [Comment("变动时间")]
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变动原因
    /// </summary>
    [Comment("变动原因")]
    [MaxLength(128)]
    public string? ChangeReason { get; set; }

    #endregion

    #region Implementation of ISchoolChangeLogBase

    /// <summary>
    /// 变动学校标识
    /// </summary>
    [Comment("变动学校标识")]
    public required Guid SchoolId { get; set; }

    /// <summary>
    /// 变动学校名称
    /// </summary>
    [Comment("变动学校名称")]
    [MaxLength(128)]
    public string? SchoolName { get; set; }

    /// <summary>
    /// 学校所在行政区划编码
    /// </summary>
    [Comment("学校所在行政区划编码")]
    [MaxLength(32)]
    public string? DivisionCode { get; set; }

    #endregion

    #region Implementation of IClassChangeLogBase

    /// <summary>
    /// 班级标识
    /// </summary>
    [Comment("变动班级标识")]
    public Guid? ClassId { get; set; }

    /// <summary>
    /// 班级名称
    /// </summary>
    [Comment("变动班级名称")]
    [MaxLength(128)]
    public string? ClassName { get; set; }

    /// <summary>
    /// 年级名称
    /// </summary>
    [Comment("年级名称")]
    [MaxLength(128)]
    public string? GradeName { get; set; }

    /// <summary>
    /// 班级序列
    /// </summary>
    [Comment("班级序列")]
    public int? SerialNumber { get; set; }

    #endregion

    #region Implementation of IStudentChangeLogPackage

    /// <summary>
    /// 学生标识
    /// </summary>
    [Comment("学生标识")]
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    [Comment("学生姓名")]
    [MaxLength(128)]
    public string? StudentName { get; set; }

    /// <summary>
    /// 学生生日
    /// </summary>
    [Comment("学生生日")]
    public DateTime? Birthday { get; set; }

    #endregion
}