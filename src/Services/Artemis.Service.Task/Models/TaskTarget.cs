using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务目标模型
/// </summary>
public class TaskTarget : ConcurrencyPartition, ITaskTarget
{
    /// <summary>
    ///     任务单元Id
    /// </summary>
    [Required]
    [Comment("任务单元标识")]
    public Guid TaskUnitId { get; set; }

    /// <summary>
    /// 任务目标名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务目标名称")]
    public required string TargetName { get; set; }

    /// <summary>
    /// 任务目标编码
    /// </summary>
    [MaxLength(32)]
    [Comment("任务目标编码")]
    public string? TargetCode { get; set; }

    /// <summary>
    /// 设计编码
    /// </summary>
    [MaxLength(32)]
    [Comment("设计编码")]
    public string? DesignCode { get; set; }

    /// <summary>
    /// 任务目标类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务目标类型")]
    public required string TargetType { get; set; }

    /// <summary>
    /// 任务目标外部标识
    /// </summary>
    [MaxLength(128)]
    [Comment("任务目标外部标识")]
    public string? TargetId { get; set; }

    /// <summary>
    ///     任务目标状态
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string TargetState { get; set; }

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
    [MaxLength(128)]
    public required string? Description { get; set; }

    /// <summary>
    ///     任务目标执行
    /// </summary>
    public DateTime? ExecuteTime { get; set; }

}