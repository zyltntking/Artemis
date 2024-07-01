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
    public required string SchoolName { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学校编码")]
    public required string SchoolCode { get; set; }

    #endregion
}