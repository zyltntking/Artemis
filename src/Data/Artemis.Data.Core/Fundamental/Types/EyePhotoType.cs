using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 眼部照片类型
/// </summary>
[Description("眼部照片类型")]
public sealed class EyePhotoType : Enumeration
{
    /// <summary>
    ///  左眼
    /// </summary>
    [Description("左眼")]
    public static readonly EyePhotoType LeftEye = new(1, nameof(LeftEye));

    /// <summary>
    /// 右眼
    /// </summary>
    [Description("右眼")]
    public static readonly EyePhotoType RightEye = new(2, nameof(RightEye));

    /// <summary>
    /// 双眼
    /// </summary>
    [Description("双眼")]
    public static readonly EyePhotoType BothEye = new(3, nameof(BothEye));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private EyePhotoType(int id, string name) : base(id, name)
    {
    }
}