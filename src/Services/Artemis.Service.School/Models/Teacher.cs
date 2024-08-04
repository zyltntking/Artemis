using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.School;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Models;

/// <summary>
///     教师模型
/// </summary>
public class Teacher : ConcurrencyModel, ITeacher
{
    /// <summary>
    ///     学校标识
    /// </summary>
    [Comment("学校标识")]
    public Guid? SchoolId { get; set; }

    /// <summary>
    ///     教师名称
    /// </summary>
    [Required]
    [Comment("教师名称")]
    [MaxLength(32)]
    public required string Name { get; set; }

    /// <summary>
    ///     教师编码
    /// </summary>
    [Comment("教师编码")]
    [MaxLength(32)]
    public string? Code { get; set; }

    /// <summary>
    ///     入职时间
    /// </summary>
    [Comment("入职时间")]
    public DateTime? EntryTime { get; set; }

    /// <summary>
    ///     教师性别
    /// </summary>
    [Comment("教师性别")]
    [MaxLength(32)]
    public string? Gender { get; set; }

    /// <summary>
    ///     教师职称
    /// </summary>
    [Comment("教师职称")]
    [MaxLength(32)]
    public string? Title { get; set; }

    /// <summary>
    ///     教师学历
    /// </summary>
    [Comment("教师学历")]
    [MaxLength(32)]
    public string? Education { get; set; }

    /// <summary>
    ///     教师身份证号
    /// </summary>
    [Comment("教师身份证号")]
    [MaxLength(32)]
    public string? IdCard { get; set; }

    /// <summary>
    ///     教师籍贯
    /// </summary>
    [Comment("教师籍贯")]
    [MaxLength(32)]
    public string? NativePlace { get; set; }

    /// <summary>
    ///     政治面貌
    /// </summary>
    [Comment("政治面貌")]
    [MaxLength(32)]
    public string? PoliticalStatus { get; set; }

    /// <summary>
    ///     家庭住址
    /// </summary>
    [Comment("家庭住址")]
    [MaxLength(128)]
    public string? Address { get; set; }

    /// <summary>
    ///     生日
    /// </summary>
    [Comment("生日")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    ///     联系电话
    /// </summary>
    [Comment("联系电话")]
    [MaxLength(32)]
    public string? Phone { get; set; }

    /// <summary>
    ///     邮箱
    /// </summary>
    [Comment("邮箱")]
    [MaxLength(128)]
    public string? Email { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    [Comment("备注")]
    [MaxLength(128)]
    public string? Remark { get; set; }
}