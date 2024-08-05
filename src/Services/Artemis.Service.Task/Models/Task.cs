using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务模型
/// </summary>
public class Task : ConcurrencyModel, ITask
{
    /// <summary>
    ///     任务名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务名称")]
    public required string TaskName { get; set; }

    /// <summary>
    ///     任务编码
    /// </summary>
    [MaxLength(64)]
    [Comment("任务编码")]
    public string? TaskCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    [MaxLength(64)]
    [Comment("设计编码")]
    public string? DesignCode { get; set; }

    /// <summary>
    ///     标准化任务名
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务名称")]
    public required string NormalizedTaskName { get; set; }

    /// <summary>
    ///     任务归属
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务归属")]
    public required string TaskShip { get; set; } = Data.Core.Fundamental.Types.TaskShip.Normal;

    /// <summary>
    ///     任务模式
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务模式")]
    public required string TaskMode { get; set; } = Data.Core.Fundamental.Types.TaskMode.Normal;

    /// <summary>
    ///     任务状态
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务状态")]
    public required string TaskState { get; set; } = Data.Core.Fundamental.Types.TaskState.Initial;

    /// <summary>
    ///     任务描述
    /// </summary>
    [Comment("任务描述")]
    [MaxLength(256)]
    public string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    [Required]
    [Comment("任务开始时间")]
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    [Comment("任务结束时间")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    ///     父任务标识
    /// </summary>
    [Comment("父任务标识")]
    public Guid? ParentId { get; set; } = Guid.Empty;
}