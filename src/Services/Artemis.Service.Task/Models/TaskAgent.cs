using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     任务代理模型
/// </summary>
public class TaskAgent : ConcurrencyPartition, ITaskAgent
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
    ///     代理Id
    /// </summary>
    [Required]
    [Comment("代理标识")]
    public Guid AgentId { get; set; }
}