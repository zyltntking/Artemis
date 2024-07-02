using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学校学生关系模型
/// </summary>
public class SchoolStudent : ISchoolStudent
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

    #region Implementation of ISchoolStudent

    /// <summary>
    ///     学校标识
    /// </summary>
    [Required]
    [Comment("学校标识")]
    public required Guid SchoolId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    [Required]
    [Comment("学生标识")]
    public required Guid StudentId { get; set; }

    #endregion
}