using Artemis.Service.Task.Context.Configuration;
using Artemis.Service.Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务代理
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTaskAgentConfiguration))]
public sealed class ArtemisTaskAgent : TaskAgent
{
    /// <summary>
    ///     任务
    /// </summary>
    public required ArtemisTask Task { get; set; }


    /// <summary>
    ///     代理
    /// </summary>
    public required ArtemisAgent Agent { get; set; }
}