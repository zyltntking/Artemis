using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 学生眼部照片模型
/// </summary>
public class StudentEyePhoto : ConcurrencyPartition, IStudentEyePhoto
{
    /// <summary>
    /// 左眼照片
    /// </summary>
    [MaxLength(256)]
    [Comment("左眼照片")]
    public string? LeftEyePhoto { get; set; }

    /// <summary>
    /// 右眼照片
    /// </summary>
    [MaxLength(256)]
    [Comment("右眼照片")]
    public string? RightEyePhoto { get; set; }

    /// <summary>
    /// 双眼照片
    /// </summary>
    [MaxLength(256)]
    [Comment("双眼照片")]
    public string? BothEyePhoto { get; set; }


    /// <summary>
    /// 学生标识
    /// </summary>
    public Guid StudentId { get; set; }

}