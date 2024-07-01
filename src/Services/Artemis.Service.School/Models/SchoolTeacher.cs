using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.School;

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
    [Description("转入时间")]
    public required DateTime MoveIn { get; set; }

    /// <summary>
    ///     转出时间
    /// </summary>
    [Description("转出时间")]
    public DateTime? MoveOut { get; set; }

    #endregion

    #region Implementation of ISchoolTeacher

    /// <summary>
    ///     班级标识
    /// </summary>
    [Required]
    [Description("学校标识")]
    public required Guid SchoolId { get; set; }

    /// <summary>
    ///     教师标识
    /// </summary>
    [Required]
    [Description("教师标识")]
    public required Guid TeacherId { get; set; }

    #endregion
}