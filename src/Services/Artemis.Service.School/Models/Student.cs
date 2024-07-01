using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     学生模型
/// </summary>
public class Student : ConcurrencyPartition, IStudent
{
    #region Implementation of IStudent

    /// <summary>
    ///     学生名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学生名称")]
    public required string StudentName { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("学生编码")]
    public required string StudentCode { get; set; }

    #endregion
}