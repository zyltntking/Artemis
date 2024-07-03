using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Task;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Models;

/// <summary>
///     代理模型
/// </summary>
public class Agent : ConcurrencyPartition, IAgent
{
    /// <summary>
    ///     代理名称
    /// </summary>
    [Required]
    [MaxLength(256)]
    [Comment("代理名称")]
    public required string AgentName { get; set; }

    /// <summary>
    ///     代理类型
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("代理类型")]
    public required string AgentType { get; set; }

    /// <summary>
    ///     代理编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("代理编码")]
    public required string AgentCode { get; set; }
}