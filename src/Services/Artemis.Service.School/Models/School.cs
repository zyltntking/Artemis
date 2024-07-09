using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学校模型
/// </summary>
public class School : ConcurrencyPartition, ISchool
{
    #region Implementation of ISchool

    /// <summary>
    ///     学校名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学校名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学校编码")]
    public required string Code { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("学校类型")]
    public required string Type { get; set; }

    #endregion
}