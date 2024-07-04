using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     教师当前所属关系模型
/// </summary>
public class TeacherCurrentAffiliation : ITeacherCurrentAffiliation
{
    #region Implementation of ITeacherCurrentAffiliation

    /// <summary>
    ///     学校标识
    /// </summary>
    [Required]
    [Comment("学校标识")]
    public required Guid SchoolId { get; set; }

    /// <summary>
    ///     班级标识
    /// </summary>
    [Required]
    [Comment("班级标识")]
    public required Guid ClassId { get; set; }

    /// <summary>
    ///     教师标识
    /// </summary>
    [Required]
    [Comment("教师标识")]
    public required Guid TeacherId { get; set; }

    #endregion
}