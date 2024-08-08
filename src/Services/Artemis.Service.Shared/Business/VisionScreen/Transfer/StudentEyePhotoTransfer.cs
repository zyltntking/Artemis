namespace Artemis.Service.Shared.Business.VisionScreen.Transfer;

/// <summary>
/// 学生眼部照片信息
/// </summary>
public record StudentEyePhotoInfo : StudentEyePhotoPackage, IStudentEyePhotoInfo
{
    #region Implementation of IKeySlot<Guid>

    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    #endregion

    #region Implementation of IStudentEyePhotoInfo

    /// <summary>
    /// 学生标识
    /// </summary>
    public Guid StudentId { get; set; }

    #endregion
}

/// <summary>
/// 学生眼部照片数据包
/// </summary>
public record StudentEyePhotoPackage : IStudentEyePhotoPackage
{
    #region Implementation of IStudentEyePhotoPackage

    /// <summary>
    /// 左眼照片
    /// </summary>
    public string? LeftEyePhoto { get; set; }

    /// <summary>
    /// 右眼照片
    /// </summary>
    public string? RightEyePhoto { get; set; }

    /// <summary>
    /// 双眼照片
    /// </summary>
    public string? BothEyePhoto { get; set; }

    #endregion
}