using Artemis.Service.Task.Context.Configuration;
using Artemis.Service.Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务单元实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTaskUnitConfiguration))]
public sealed class ArtemisTaskUnit : TaskUnit
{
    /// <summary>
    ///     任务
    /// </summary>
    public required ArtemisTask Task { get; set; }

    /// <summary>
    ///     任务目标
    /// </summary>
    public ICollection<ArtemisTaskUnitTarget>? TaskUnitTargets { get; set; }

    /// <summary>
    ///     任务代理
    /// </summary>
    public ICollection<ArtemisTaskUnitAgent>? TaskUnitAgents { get; set; }

    /// <summary>
    ///     代理
    /// </summary>
    public ICollection<ArtemisAgent>? Agents { get; set; }
}