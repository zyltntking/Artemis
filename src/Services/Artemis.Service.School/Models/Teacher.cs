using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     教师模型
/// </summary>
public class Teacher : ConcurrencyPartition, ITeacher
{
    #region Implementation of ITeacher

    /// <summary>
    ///     教师名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("教师名称")]
    public required string TeacherName { get; set; }

    /// <summary>
    ///     教师编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("教师编码")]
    public required string TeacherCode { get; set; }

    #endregion
}