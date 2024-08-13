using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
/// 教师变动记录模型
/// </summary>
public class TeacherChangeLog : ConcurrencyModel, ITeacherChangeLog
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

    #region Implementation of ITeacherChangeLogPackage

    /// <summary>
    /// 教师标识
    /// </summary>
    [Comment("教师标识")]
    public required Guid TeacherId { get; set; }

    /// <summary>
    /// 教师姓名
    /// </summary>
    [Comment("教师姓名")]
    [MaxLength(128)]
    public string? TeacherName { get; set; }

    #endregion
}