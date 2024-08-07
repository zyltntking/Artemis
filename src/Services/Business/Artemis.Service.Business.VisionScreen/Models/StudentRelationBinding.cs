using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 学生用户亲属关系绑定模型
/// </summary>
public class StudentRelationBinding : ConcurrencyPartition, IStudentRelationBinding
{
    #region Implementation of IStudentRelationBindingPackage

    /// <summary>
    /// 用户标识
    /// </summary>
    [Comment("用户标识")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 学生标识
    /// </summary>
    [Comment("学生标识")]
    public Guid StudentId { get; set; }

    /// <summary>
    /// 关系
    /// </summary>
    [Comment("关系")]
    public required string Relation { get; set; }

    #endregion
}