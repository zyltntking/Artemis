using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     班级模型
/// </summary>
public class Class : ConcurrencyPartition, IClass
{
    #region Implementation of IClass

    /// <summary>
    ///     学校标识
    /// </summary>
    [Required]
    [Comment("学校标识")]
    public Guid SchoolId { get; set; }

    /// <summary>
    ///     班主任标识
    /// </summary>
    [Required]
    [Comment("班主任标识")]
    public Guid HeadTeacherId { get; set; }

    /// <summary>
    ///     班级名称
    /// </summary>
    [MaxLength(32)]
    [Comment("班级名称")]
    public string? Name { get; set; }

    /// <summary>
    ///     年级名称
    /// </summary>
    [MaxLength(16)]
    public string? GradeName { get; set; }

    /// <summary>
    ///     班级类型
    /// </summary>
    [MaxLength(32)]
    [Comment("班级类型")]
    public string? Type { get; set; }

    /// <summary>
    ///     所学专业
    /// </summary>
    [MaxLength(128)]
    [Comment("所学专业")]
    public string? Major { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("班级编码")]
    public required string Code { get; set; }

    /// <summary>
    ///     学段
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学段")]
    public required string StudyPhase { get; set; }

    /// <summary>
    ///     学制
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学制")]
    public required string SchoolLength { get; set; }

    /// <summary>
    ///     学制长度
    /// </summary>
    [Required]
    [Comment("学制长度")]
    public required int Length { get; set; }

    /// <summary>
    ///     班级序号
    /// </summary>
    [Required]
    [Comment("班级序号")]
    public required int SerialNumber { get; set; }

    /// <summary>
    ///     班级创建时间
    /// </summary>
    [Required]
    [Comment("班级创建时间")]
    public required DateTime EstablishTime { get; set; }

    #endregion
}