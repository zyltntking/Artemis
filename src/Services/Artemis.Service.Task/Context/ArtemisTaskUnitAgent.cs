using Artemis.Service.Task.Context.Configuration;
using Artemis.Service.Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务单元代理
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTaskUnitAgentConfiguration))]
public sealed class ArtemisTaskUnitAgent : TaskUnitAgent
{
    /// <summary>
    ///     任务单元
    /// </summary>
    public required ArtemisTaskUnit TaskUnit { get; set; }


    /// <summary>
    ///     代理
    /// </summary>
    public required ArtemisAgent Agent { get; set; }
}