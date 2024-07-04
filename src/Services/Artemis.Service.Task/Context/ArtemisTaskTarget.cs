using Artemis.Service.Task.Context.Configuration;
using Artemis.Service.Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务目标实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTaskTargetConfiguration))]
public class ArtemisTaskTarget : TaskTarget
{
    /// <summary>
    ///     任务单元
    /// </summary>
    public required ArtemisTaskUnit TaskUnit { get; set; }
}