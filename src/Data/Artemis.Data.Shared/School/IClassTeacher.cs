namespace Artemis.Data.Shared.School;

/// <summary>
/// 班级教师对应接口
/// </summary>
public interface IClassTeacher : ITransfer
{
    /// <summary>
    /// 班级标识
    /// </summary>
    Guid ClassId { get; set; }

    /// <summary>
    /// 教师标识
    /// </summary>
    Guid TeacherId { get; set; }
}