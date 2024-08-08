using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 学生眼部照片接口
/// </summary>
public interface IStudentEyePhoto : IStudentEyePhotoInfo
{
}

/// <summary>
/// 学生眼部照片信息接口
/// </summary>
public interface IStudentEyePhotoInfo : IStudentEyePhotoPackage, IKeySlot
{
    /// <summary>
    /// 学生标识
    /// </summary>
    Guid StudentId { get; set; }
}


/// <summary>
/// 学生眼部照片数据包接口
/// </summary>
public interface IStudentEyePhotoPackage
{
    /// <summary>
    /// 左眼照片
    /// </summary>
    string? LeftEyePhoto { get; set; }

    /// <summary>
    /// 右眼照片
    /// </summary>
    string? RightEyePhoto { get; set; }

    /// <summary>
    /// 双眼照片
    /// </summary>
    string? BothEyePhoto { get; set; }
}