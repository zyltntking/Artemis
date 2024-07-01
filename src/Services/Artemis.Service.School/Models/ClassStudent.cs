using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Artemis.Data.Shared.School;

namespace Artemis.Service.School.Models;

/// <summary>
///     班级学生关系模型
/// </summary>
public class ClassStudent : IClassStudent
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

    #region Implementation of IClassStudent

    /// <summary>
    ///     班级标识
    /// </summary>
    [Required]
    [Description("班级标识")]
    public required Guid ClassId { get; set; }

    /// <summary>
    ///     学生标识
    /// </summary>
    [Required]
    [Description("学生标识")]
    public required Guid StudentId { get; set; }

    #endregion
}