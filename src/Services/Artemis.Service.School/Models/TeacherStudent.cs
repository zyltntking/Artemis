using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     教师学生对应关系模型
/// </summary>
public class TeacherStudent : ITeacherStudent
{
    #region Implementation of ITransfer

    /// <summary>
    ///     转入时间
    /// </summary>
    [Required]
    [Comment("转入时间")]
    public DateTime MoveIn { get; set; }

    /// <summary>
    ///     转出时间
    /// </summary>
    [Comment("转出时间")]
    public DateTime? MoveOut { get; set; }

    #endregion

    #region Implementation of ITeacherStudent

    /// <summary>
    ///     教师标识
    /// </summary>
    [Required]
    [Comment("教师标识")]
    public Guid TeacherId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    [Required]
    [Comment("学生标识")]
    public Guid StudentId { get; set; }

    #endregion
}