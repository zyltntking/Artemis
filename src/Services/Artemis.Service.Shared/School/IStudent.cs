namespace Artemis.Service.Shared.School;

/// <summary>
///     学生接口
/// </summary>
public interface IStudent
{
    /// <summary>
    ///     学生名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     学生性别
    /// </summary>
    string Gender { get; set; }

    /// <summary>
    ///     专业
    /// </summary>
    string? Major { get; set; }

    /// <summary>
    ///     学生生日
    /// </summary>
    DateOnly Birthday { get; set; }

    /// <summary>
    ///     民族
    /// </summary>
    string? Nation { get; set; }

    /// <summary>
    ///     学生编码
    /// </summary>
    string Code { get; set; }

    /// <summary>
    ///     学籍号
    /// </summary>
    string StudentNumber { get; set; }

    /// <summary>
    ///     证件号码
    /// </summary>
    string? Cert { get; set; }

    /// <summary>
    ///     入学时间
    /// </summary>
    DateTime EnrollmentDate { get; set; }

    /// <summary>
    ///     住址区划代码
    /// </summary>
    string? DivisionCode { get; set; }

    /// <summary>
    ///     住址
    /// </summary>
    string? Address { get; set; }
}