using Artemis.Service.Task.Context.Configuration;
using Artemis.Service.Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     代理实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisAgentConfiguration))]
public sealed class ArtemisAgent : Agent
{
    /// <summary>
    ///     任务代理
    /// </summary>
    public ICollection<ArtemisTaskAgent>? TaskAgents { get; set; }
}