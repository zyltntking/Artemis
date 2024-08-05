using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Service.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务单元模型
/// </summary>
public class TaskUnit : ConcurrencyModel, ITaskUnit
{
    /// <summary>
    ///     任务标识
    /// </summary>
    [Required]
    [Comment("任务标识")]
    public Guid TaskId { get; set; }

    /// <summary>
    ///     任务单元名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务单元名称")]
    public required string UnitName { get; set; }

    /// <summary>
    ///     标准化单元名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("标准化单元名")]
    public required string NormalizedUnitName { get; set; }

    /// <summary>
    ///     单元编码
    /// </summary>
    [MaxLength(64)]
    [Comment("单元编码")]
    public string? UnitCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    [MaxLength(64)]
    [Comment("设计编码")]
    public string? DesignCode { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务状态")]
    public required string TaskUnitState { get; set; } = TaskState.Initial;

    /// <summary>
    ///     任务模式
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务模式")]
    public required string TaskUnitMode { get; set; } = TaskMode.Normal;

    /// <summary>
    ///     任务描述
    /// </summary>
    [Comment("任务描述")]
    [MaxLength(128)]
    public string? Description { get; set; }

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