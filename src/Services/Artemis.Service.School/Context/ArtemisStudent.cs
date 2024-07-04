using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学生实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentConfiguration))]
public sealed class ArtemisStudent : Student
{
    /// <summary>
    ///     班级学生对应关系
    /// </summary>
    public ICollection<ArtemisClassStudent>? ClassStudents { get; set; }

    /// <summary>
    ///     学生所在的班级
    /// </summary>
    public ICollection<ArtemisClass>? Classes { get; set; }

    /// <summary>
    ///     学校学生对应关系
    /// </summary>
    public ICollection<ArtemisSchoolStudent>? SchoolStudents { get; set; }

    /// <summary>
    ///     学生所在的学校
    /// </summary>
    public ICollection<ArtemisSchool>? Schools { get; set; }

    /// <summary>
    ///     学生所属关系
    /// </summary>
    public ArtemisStudentCurrentAffiliation CurrentAffiliation { get; set; }
}