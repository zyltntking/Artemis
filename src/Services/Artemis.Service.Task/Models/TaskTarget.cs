using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务目标模型
/// </summary>
public class TaskUnitTarget : ConcurrencyModel, ITaskUnitTarget
{
    /// <summary>
    ///     任务单元Id
    /// </summary>
    [Required]
    [Comment("任务单元标识")]
    public Guid TaskUnitId { get; set; }

    /// <summary>
    ///     任务目标名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("任务目标名称")]
    public required string TargetName { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    [MaxLength(64)]
    [Comment("任务目标编码")]
    public string? TargetCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    [MaxLength(64)]
    [Comment("设计编码")]
    public string? DesignCode { get; set; }

    /// <summary>
    ///     任务目标类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("任务目标类型")]
    public required string TargetType { get; set; }

    /// <summary>
    ///     绑定标记
    /// </summary>
    [MaxLength(128)]
    [Comment("绑定标记")]
    public string? BindingTag { get; set; }

    /// <summary>
    ///     任务目标状态
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string TargetState { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    [Comment("任务描述")]
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    ///     任务目标执行时间
    /// </summary>
    [Comment("任务目标执行时间")]
    public DateTime? ExecuteTime { get; set; }
}