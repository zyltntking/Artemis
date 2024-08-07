using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 学生用户亲属关系绑定接口
/// </summary>
public interface IStudentRelationBinding : IStudentRelationBindingInfo;

/// <summary>
/// 学生用户亲属关系绑定信息接口
/// </summary>
public interface IStudentRelationBindingInfo : IStudentRelationBindingPackage, IKeySlot;

/// <summary>
/// 学生用户亲属关系绑定数据包接口
/// </summary>
public interface IStudentRelationBindingPackage
{
    /// <summary>
    /// 用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// 学生标识
    /// </summary>
    Guid StudentId { get; set; }

    /// <summary>
    /// 关系
    /// </summary>
    string Relation { get; set; }
}