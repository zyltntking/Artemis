using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 教师用户绑定接口
/// </summary>
public interface ITeacherUserBinding : ITeacherUserBindingInfo
{

}

/// <summary>
/// 教师用户绑定信息接口
/// </summary>
public interface ITeacherUserBindingInfo : ITeacherUserBindingPackage, IKeySlot
{

}

/// <summary>
/// 教师用户绑定数据包接口
/// </summary>
public interface ITeacherUserBindingPackage
{
    /// <summary>
    /// 用户标识
    /// </summary>
    Guid UserId { get; set; }

    /// <summary>
    /// 教师标识
    /// </summary>
    Guid TeacherId { get; set; }
}