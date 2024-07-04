using Artemis.Service.Task.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTaskConfiguration))]
public sealed class ArtemisTask : Models.Task
{
    /// <summary>
    ///     父任务
    /// </summary>
    public ArtemisTask? Parent { get; set; }

    /// <summary>
    ///     子任务
    /// </summary>
    public ICollection<ArtemisTask>? Children { get; set; }

    /// <summary>
    ///     任务单元
    /// </summary>
    public ICollection<ArtemisTaskUnit>? TaskUnits { get; set; }

    /// <summary>
    ///     任务代理
    /// </summary>
    public ICollection<ArtemisTaskAgent>? TaskAgents { get; set; }

    /// <summary>
    ///     代理
    /// </summary>
    public ICollection<ArtemisAgent>? Agents { get; set; }
}