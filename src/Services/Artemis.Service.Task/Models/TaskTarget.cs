using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务目标模型
/// </summary>
public class TaskTarget : ConcurrencyPartition, ITaskTarget
{
    /// <summary>
    ///     任务Id
    /// </summary>
    [Required]
    [Comment("任务标识")]
    public Guid TaskId { get; set; }

    /// <summary>
    ///     任务单元Id
    /// </summary>
    [Required]
    [Comment("任务单元标识")]
    public Guid TaskUnitId { get; set; }

    /// <summary>
    ///     目标Id
    /// </summary>
    [Required]
    [Comment("目标标识")]
    public Guid TargetId { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务状态")]
    public required string TaskStatus { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    [Comment("任务描述")]
    public required string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    [Required]
    [Comment("任务开始时间")]
    public required DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    [Comment("任务结束时间")]
    public DateTime? EndTime { get; set; }
}