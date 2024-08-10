using Artemis.Service.School.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学校实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisSchoolConfiguration))]
public sealed class ArtemisSchool : Models.School
{
    /// <summary>
    ///     学校管理的班级
    /// </summary>
    public ICollection<ArtemisClass>? Classes { get; set; }

    /// <summary>
    ///     学校管理的教师
    /// </summary>
    public ICollection<ArtemisTeacher>? Teachers { get; set; }

    /// <summary>
    ///     学校管理的学生
    /// </summary>
    public ICollection<ArtemisStudent>? Students { get; set; }

    /// <summary>
    /// 教师变动记录
    /// </summary>
    public ICollection<ArtemisTeacherChangeLog>? TeacherChangeLogs { get; set; }

    /// <summary>
    /// 学生变动记录
    /// </summary>
    public ICollection<ArtemisStudentChangeLog>? StudentChangeLogs { get; set; }
}