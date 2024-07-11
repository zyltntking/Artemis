using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学校老师关系模型
/// </summary>
public class SchoolTeacher : ISchoolTeacher
{
    #region Implementation of ITransfer

    /// <summary>
    ///     转入时间
    /// </summary>
    [Required]
    [Comment("转入时间")]
    public required DateTime MoveIn { get; set; }

    /// <summary>
    ///     转出时间
    /// </summary>
    [Comment("转出时间")]
    public DateTime? MoveOut { get; set; }

    #endregion

    #region Implementation of ISchoolTeacher

    /// <summary>
    ///     班级标识
    /// </summary>
    [Required]
    [Comment("学校标识")]
    public required Guid SchoolId { get; set; }

    /// <summary>
    ///     教师标识
    /// </summary>
    [Required]
    [Comment("教师标识")]
    public required Guid TeacherId { get; set; }

    #endregion
}