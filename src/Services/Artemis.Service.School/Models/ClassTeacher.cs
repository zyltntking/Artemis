using System.ComponentModel.DataAnnotations;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     班级教师关系模型
/// </summary>
public class ClassTeacher : IClassTeacher
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

    #region Implementation of IClassTeacher

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