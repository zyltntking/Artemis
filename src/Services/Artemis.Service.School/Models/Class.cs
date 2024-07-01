using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     班级模型
/// </summary>
public class Class : ConcurrencyPartition, IClass
{
    #region Implementation of IClass

    /// <summary>
    ///     班级名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("班级名称")]
    public required string ClassName { get; set; }

    /// <summary>
    ///     班级编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("班级编码")]
    public required string ClassCode { get; set; }

    #endregion
}