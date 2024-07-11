using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学生当前所属关系模型
/// </summary>
public class StudentCurrentAffiliation : IStudentCurrentAffiliation
{
    #region Implementation of IStudentCurrentAffiliation

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
    ///     学生标识
    /// </summary>
    [Required]
    [Comment("学生标识")]
    public required Guid StudentId { get; set; }

    #endregion
}