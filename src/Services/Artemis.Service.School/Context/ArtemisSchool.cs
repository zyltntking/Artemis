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
    ///     学校学生对应关系
    /// </summary>
    public ICollection<ArtemisSchoolStudent>? SchoolStudents { get; set; }

    /// <summary>
    ///     学校管理的学生
    /// </summary>
    public ICollection<ArtemisStudent>? Students { get; set; }

    /// <summary>
    ///     学校老师对应关系
    /// </summary>
    public ICollection<ArtemisSchoolTeacher>? SchoolTeachers { get; set; }

    /// <summary>
    ///     学校管理的教师
    /// </summary>
    public ICollection<ArtemisTeacher>? Teachers { get; set; }
}